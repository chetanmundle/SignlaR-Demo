using Common.Dtos.ResponseDtos;
using Common.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interface.IRepositories
{
    public interface IUserRepository
    {
        Task<AppResponse> CreateUserAsync(CreateUserDto userDto);
        Task<AppResponse<List<GetUserResponseDto>>> GetAllUserAsync();
        Task<AppResponse<GetUserResponseDto>> GetUserByIdAsync(int userId);
        Task<AppResponse<LoginUserResponseDto>> LoginAsync(LoginUserDto loginUserDto);
    }
}
