using App.Core.Interface.IServices;
using Common.Dtos.ConversationDtos;
using Common.GenericResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Conversation.Command
{
    public class CreateConversationCommand : IRequest<AppResponse>
    {
        public CreateConversationRequest CreateConversation {  get; set; }
    }

    internal class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, AppResponse>
    {
        private readonly IConversationService _conversationService;

        public CreateConversationCommandHandler(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        public Task<AppResponse> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            return _conversationService.CreateConversationAsync(request.CreateConversation);
        }
    }
}
