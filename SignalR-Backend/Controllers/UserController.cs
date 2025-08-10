using App.Core.App.Users.Command;
using App.Core.App.Users.Query;
using App.Core.Validation;
using Common.Dtos.ResponseDtos;
using Common.GenericResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SignalR_Backend.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createUser")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                var createUserDtoValidator = new CreateUserDtoValidator();
                var validate = createUserDtoValidator.Validate(createUserDto);

                if(validate.Errors.Any())
                {
                    var errorMessage = validate.Errors[0].ErrorMessage;
                    return BadRequest(AppResponse.Response(false, errorMessage, HttpStatusCodes.BadRequest));
                }

                var result = await _mediator.Send(new CreateUserDtoCommand() { CreateUserDto = createUserDto });
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, AppResponse.Response(false, ex.Message, HttpStatusCodes.InternalServerError));
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(LoginUserDto loginUserDto)
        {
            try
            {
                var result = await _mediator.Send(new LoginQuery { LoginUserDto = loginUserDto });
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, AppResponse.Fail<LoginUserResponseDto>(null, ex.Message, HttpStatusCodes.InternalServerError));
            }
        }

        [HttpGet("{userId}/GetUserById")]
        public async Task<IActionResult> GetLoggedUser(int userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserQuery { UserId = userId });
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, AppResponse.Fail<GetUserResponseDto>(null, ex.Message, HttpStatusCodes.InternalServerError));
            }
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _mediator.Send(new GetAllUserQuery());
                return StatusCode((int)result.StatusCode,result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, AppResponse.Fail<List<GetUserResponseDto>>(null, ex.Message, HttpStatusCodes.InternalServerError));
            }
        }


    }
}
