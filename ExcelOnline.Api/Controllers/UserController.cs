using AutoMapper;
using ExcelOnline.Api.Options;
using ExcelOnline.Api.Services;
using ExcelOnline.Api.Transfers;
using ExcelOnline.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExcelOnline.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public UserController(
            IMapper mapper,
            IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
        }

        [HttpGet("users")]
        public async Task<ActionResult<QueryResponse<UserTransOut>>> GetUsers([FromQuery] UserQueryOption option)
        {
            var users = await this.userService.GetUsers(option);
            var usersTransOut = users.Select(i => this.mapper.Map<UserTransOut>(i)).ToList();

            return Ok(QueryResponse.New(option, usersTransOut));
        }

        [HttpGet("users/{userId:int}")]
        public async Task<ActionResult<UserTransOut>> GetUsers(int userId)
        {
            var user = await this.userService.GetUser(userId);
            var userTransOut = this.mapper.Map<UserTransOut>(user);

            return Ok(userTransOut);
        }

        [HttpGet("users/salerlist")]
        public async Task<ActionResult<UserTransOut>> GetSalerList()
        {
            var users = await this.userService.GetUsers(new UserQueryOption());
            var userTransOut = users.Where(c => c.Role == "saler").Select(i => this.mapper.Map<UserTransOut>(i)).ToList();

            return Ok(userTransOut);
        }

        [HttpPost("users")]
        public async Task<ActionResult> CreateUser([FromBody] UserTransIn transIn)
        {
            var result = this.mapper.Map<User>(transIn);
            await this.userService.AddUser(result);
            return Ok();
        }

        [HttpPost("users/validate")]
        public async Task<ActionResult<bool>> ValidateUser([FromBody] UserQueryOption option)
        {
            var result = await this.userService.ValidateUser(option);
            return Ok(result);
        }

    }
}
