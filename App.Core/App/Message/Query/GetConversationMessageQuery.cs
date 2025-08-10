using App.Core.Interface.IServices;
using Common.Dtos.MessageDtos;
using Common.GenericResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Message.Query
{
    public class GetConversationMessageQuery : IRequest<AppResponse<List<GetMessagesResponseDto>>>
    {
        public int ConversationId { get; set; }
    }

    internal class GetConversationMessageQueryHandler : IRequestHandler<GetConversationMessageQuery, AppResponse<List<GetMessagesResponseDto>>>
    {
        private readonly IMessageService _messageService;

        public GetConversationMessageQueryHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public Task<AppResponse<List<GetMessagesResponseDto>>> Handle(GetConversationMessageQuery request, CancellationToken cancellationToken)
        {
            return _messageService.GetMessagesByConversationIdAsync(request.ConversationId);
        }
    }
}
