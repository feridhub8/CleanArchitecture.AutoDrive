using AutoDrive.Application.CQRS_Design_Pattern.Commands.RefreshTokens;
using AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;
using AutoDrive.Application.CQRS_Design_Pattern.Queries;
using AutoDrive.Application.DTOs.RefreshToken;
using AutoDrive.Application.DTOs.Response;
using AutoDrive.Application.DTOs.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoDrive.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(registerUserDto);
            var userId = await _mediator.Send(command, cancellationToken);
            return Ok(new RegisterResponse(userId, "User registered successfully"));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(loginUserDto);
            var loginResponse = await _mediator.Send(command, cancellationToken);
            return Ok(loginResponse);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await _mediator.Send(new RefreshTokenLoginCommand(refreshTokenDto));
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] UserForgotPasswordDto userForgotPasswordDto, CancellationToken cancellationToken)
        {
            var command = new UserForgotPasswordCommand(userForgotPasswordDto);
            var message = await _mediator.Send(command, cancellationToken);
            return Ok(new
            {
                Message = message
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDto userResetPasswordDto, CancellationToken cancellationToken)
        {
            var command = new UserResetPasswordCommand(userResetPasswordDto);
            var message = await _mediator.Send(command, cancellationToken);
            return Ok(new
            {
                Message = message
            });
        }

        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand(id);
            var message = await _mediator.Send(command, cancellationToken);
            return Ok(new
            {
                Message = message
            });
        }

        [HttpPatch]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteOwnProfile(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User id claim not found");
            var command = new DeleteUserCommand(Guid.Parse(userId));
            var message = await _mediator.Send(command, cancellationToken);
            return Ok(new
            {
                Message = message
            });
        }

        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchUser([FromRoute] Guid id, [FromBody] PatchUserDto patchUserDto, CancellationToken cancellationToken)
        {
            var command = new PatchUserCommand(id, patchUserDto);
            var message = await _mediator.Send(command, cancellationToken);
            return Ok(new
            {
                Message = message
            });
        }

        [HttpPatch]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PatchOwnProfile([FromBody] PatchUserDto patchUserDto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User id claim not found");
            var command = new PatchUserCommand(Guid.Parse(userId), patchUserDto);
            var message = await _mediator.Send(command, cancellationToken);
            return Ok(new
            {
                Message = message
            });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query, cancellationToken);
            return Ok(users);
        }

        [HttpGet("{email}")]
        [Authorize]
        public async Task<IActionResult> GetUserByEmail([FromRoute] string email, CancellationToken cancellationToken)
        {
            var query = new GetUserByEmailQuery(email);
            var user = await _mediator.Send(query, cancellationToken);
            return Ok(user);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(id);
            var user = await _mediator.Send(query, cancellationToken);
            return Ok(user);
        }
    }
}
