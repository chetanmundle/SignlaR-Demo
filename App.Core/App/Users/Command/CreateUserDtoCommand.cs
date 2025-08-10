using App.Core.Interface.IRepositories;
using Common.Dtos.ResponseDtos;
using Common.GenericResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Users.Command
{
    public class CreateUserDtoCommand : IRequest<AppResponse>
    {
        public CreateUserDto CreateUserDto { get; set; }
    }

    internal class CreateUserCommandHandler : IRequestHandler<CreateUserDtoCommand, AppResponse>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<AppResponse> Handle(CreateUserDtoCommand request, CancellationToken cancellationToken)
        {
            //try
            //{
                var response = await _userRepository.CreateUserAsync(request.CreateUserDto);
                return response;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
    }
}
