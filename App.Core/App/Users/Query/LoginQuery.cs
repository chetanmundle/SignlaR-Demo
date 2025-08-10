using App.Core.Interface.IRepositories;
using Common.Dtos.ResponseDtos;
using Common.GenericResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Users.Query
{
    public class LoginQuery : IRequest<AppResponse<LoginUserResponseDto>>
    {
       public LoginUserDto LoginUserDto { get; set; }
    }

    internal class LoginQueryHandler : IRequestHandler<LoginQuery, AppResponse<LoginUserResponseDto>>
    {
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AppResponse<LoginUserResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var serviceResponse = await _userRepository.LoginAsync(request.LoginUserDto);
            return serviceResponse;
        }
    }
}
