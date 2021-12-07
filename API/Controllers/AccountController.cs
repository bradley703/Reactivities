using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
        TokenService tokenService)
        {
            _tokenService = tokenService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> LoginDto(LoginDto loginDto){
           var user = await this.userManager.FindByEmailAsync(loginDto.Email);
           if(user == null) return Unauthorized();
           var result = await this.signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

           if(result.Succeeded){
               return CreateUserObject(user);
           }
           return Unauthorized();
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegistersDto registerDto){
           if(await this.userManager.Users.AnyAsync(x => x.Email == registerDto.Email)){
                return BadRequest("Email taken");
            }
             if(await this.userManager.Users.AnyAsync(x => x.Email == registerDto.Username)){
                return BadRequest("Username taken");
            }
            var user = new AppUser{
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username
            };
            var result = await this.userManager.CreateAsync(user, registerDto.Password);
            if(result.Succeeded){
                 return CreateUserObject(user);
            }
            return BadRequest("Problem registering user");
        }
        [HttpGet]
        public async Task<ActionResult<UserDto>>GetCurrentUser(){
            var user = await this.userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            return CreateUserObject(user);
        }
        private UserDto CreateUserObject(AppUser user){
                return new UserDto{
                    DisplayName = user.DisplayName,
                    Image = null,
                    Token = _tokenService.CreateToken(user),
                    Username = user.UserName
                };
        }
    }
}