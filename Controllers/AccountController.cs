using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using backend.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{

    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
        public string UserName { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _conf;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            UserDbContext context,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _conf = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Credentials credentials)
        {
            var user = _mapper.Map<IdentityUser>(credentials);
            var result = await _userManager.CreateAsync(user, credentials.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(CreateToken(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            var result = await _signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, false, false);

            if (!result.Succeeded) return BadRequest();

            var user = await _userManager.FindByEmailAsync(credentials.Email);

            return Ok(CreateToken(user));
        }

        private TokenResponse CreateToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["secret"]));
            var signingCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new TokenResponse
            {
                Token = encodedJwt,
                UserName = user.UserName
            };

            return response;
        }
    }
}
