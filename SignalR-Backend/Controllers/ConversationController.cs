using App.Core.App.Conversation.Command;
using App.Core.App.Conversation.Query;
using Common.Dtos.ConversationDtos;
using Common.GenericResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SignalR_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConversationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-conversation")]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationRequest request)
        {
            try
            {
                var result = await _mediator.Send(new CreateConversationCommand { CreateConversation = request });
                return StatusCode((int) result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, AppResponse.Response(false, ex.Message, HttpStatusCodes.InternalServerError));
            }
        }

        [HttpGet("{userId}/get-My-conversation")]
        public async Task<IActionResult> GetMyConversation(int userId)
        {
            try
            {
                var result = await _mediator.Send(new GetMyConversationByIdQuery { UserId = userId });
                return StatusCode((int) result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, AppResponse.Fail<GetMyConversationResponseDto>(null, ex.Message, HttpStatusCodes.InternalServerError));
            }
        }
    }
}
