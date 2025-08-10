
using App.Core.Interface;
using App.Core.Interface.IRepositories;
using App.Core.Interfaces;
using Common.Dtos.ResponseDtos;
using Common.GenericResponse;
using Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IJwtService _jwtService;

        public UserRepository(
            IAppDbContext appDbContext,
            IJwtService jwtService
        )
        {
            _appDbContext = appDbContext;
            _jwtService = jwtService;
        }
        public async Task<AppResponse> CreateUserAsync(CreateUserDto userDto)
        {
            //try
            //{
            // checking the user with email already exist or not
            var isUserExist = await _appDbContext.Users
                    .AnyAsync(u => u.Email == userDto.Email);

            if (isUserExist)
            {
                return AppResponse.Response(false, "User with this Email Already Exist", HttpStatusCodes.Conflict);
            }
                var user = userDto.Adapt<Domain.Entities.User>();
                await _appDbContext.Users.AddAsync(user);

                await _appDbContext.SaveChangesAsync();

                return AppResponse.Response(true, "User Created Successfully", HttpStatusCodes.OK);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        public async Task<AppResponse<List<GetUserResponseDto>>> GetAllUserAsync()
        {
            var users = await _appDbContext.Users
                .AsNoTracking()
                .Where(u => u.IsActive && !u.IsDeleted)
                .Select(u => new GetUserResponseDto
                {
                    Email = u.Email,
                    Name = u.Name,
                    UserId = u.UserId,
                })
                .ToListAsync();

            return AppResponse.Success(users);
        }

        public async Task<AppResponse<GetUserResponseDto>> GetUserByIdAsync(int userId)
        {
            var userResponse = await _appDbContext.Users
                .AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => new GetUserResponseDto
                {
                    UserId = userId,
                    Email = u.Email,
                    Name = u.Name,
                }).FirstOrDefaultAsync();

            if (userResponse is null)
            {
                return AppResponse.Fail<GetUserResponseDto>(null, "Email or Password is Wrong", HttpStatusCodes.NotFound);
            }

            return AppResponse.Success(userResponse);

        }

        public async Task<AppResponse<LoginUserResponseDto>> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _appDbContext.Users
                .AsNoTracking()
                .Where(u => u.Email == loginUserDto.Email && u.Password == loginUserDto.Password)
                .FirstOrDefaultAsync();

            if(user is null)
            {
                return AppResponse.Fail<LoginUserResponseDto>(null, "Email or Password is Wrong", HttpStatusCodes.NotFound);
            }

            var token = _jwtService.Authenticate(user.UserId, user.Name, user.Email, "User");

            var response = new LoginUserResponseDto
            {
                UserId = user.UserId,
                Email = loginUserDto.Email,
                Name = user.Name,
                AccessToken = token,
            };


            return AppResponse.Success(response, "User Login Successfully");
        }
    }
}
