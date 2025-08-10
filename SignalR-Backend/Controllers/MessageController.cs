using App.Core.App.Message.Query;
using Common.Dtos.ConversationDtos;
using Common.Dtos.MessageDtos;
using Common.GenericResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SignalR_Backend.Controllers
{
    [Route("api/message")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("conversationId/{conversationId}/getMessages")]
        public async Task<IActionResult> GetMessagesByConversationId(int conversationId)
        {
            try
            {
                var result = await _mediator.Send(new GetConversationMessageQuery { ConversationId = conversationId });
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, AppResponse.Fail<List<GetMessagesResponseDto>>(null, ex.Message, HttpStatusCodes.InternalServerError));
            }
        }
    }
}
