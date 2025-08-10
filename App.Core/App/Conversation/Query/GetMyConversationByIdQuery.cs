using App.Core.Interface.IServices;
using Common.Dtos.ConversationDtos;
using Common.GenericResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Conversation.Query
{
    public class GetMyConversationByIdQuery :IRequest<AppResponse<List<GetMyConversationResponseDto>>>
    {
        public int UserId { get; set; }
    }

internal class GetMyConversationByIdQueryHandler : IRequestHandler<GetMyConversationByIdQuery, AppResponse<List<GetMyConversationResponseDto>>>
    {
        private readonly IConversationService _conversationService;

        public GetMyConversationByIdQueryHandler(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        public async Task<AppResponse<List<GetMyConversationResponseDto>>> Handle(GetMyConversationByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _conversationService.GetMyConversationAsync(request.UserId);
            return result;
        }
    }
}
