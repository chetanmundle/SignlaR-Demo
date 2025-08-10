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
    public class GetAllUserQuery : IRequest<AppResponse<List<GetUserResponseDto>>>
    {
    }

    internal class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, AppResponse<List<GetUserResponseDto>>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<AppResponse<List<GetUserResponseDto>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = _userRepository.GetAllUserAsync();
            return users;
        }
    }
}
