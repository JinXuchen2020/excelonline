using AutoMapper;
using ExcelOnline.Api.Services;
using ExcelOnline.Api.Transfers;
using ExcelOnline.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExcelOnline.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IConfiguration configuration;

        public LoginController(
            IMapper mapper,
            IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
        }

        [HttpGet("userToken/phoneNumber")]
        public async Task<ActionResult<UserTokenTransOut>> GetUserTokenByPhoneNumber([FromQuery] string phoneNumber)
        {
            UserTokenTransOut result;
            try
            {
                var userInfo = await this.userService.GetUserByPhone(phoneNumber);
                if (userInfo != null)
                {
                    result = GenerateToken(userInfo);
                }
                else
                {
                    result = new UserTokenTransOut
                    {
                        Code = -1,
                        Message = "此用户没有权限登录，请联系管理员",
                        Token = ""
                    };
                }
            }
            catch (Exception e)
            {
                result = new UserTokenTransOut
                {
                    Code = -1,
                    Message = e.Message,
                    Token = ""
                };
            }            

            return Ok(result);
        }

        private UserTokenTransOut GenerateToken(User user)
        {
            string secretKey = "YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv";
            string validIssuer = "saleinfo";
            string validAudience = "saleinfo";

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.PhoneNumber)
            };

            //对称秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            //签名证书(秘钥，加密算法)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //生成token  [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
            var token = new JwtSecurityToken(
                validIssuer,
                validAudience,
                claims,
                DateTime.Now,
                DateTime.Now.AddDays(1),
                creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserTokenTransOut
            {
                Code = 1,
                Id = user.Id,
                Message = "Ok",
                Token = tokenString,
                User = this.mapper.Map<UserTransOut>(user)
            };
        }
    }
}
