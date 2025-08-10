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
    public class GetUserQuery : IRequest<AppResponse<GetUserResponseDto>>
    {
        public int UserId { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, AppResponse<GetUserResponseDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<AppResponse<GetUserResponseDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return _userRepository.GetUserByIdAsync(request.UserId);
        }
    }
}
