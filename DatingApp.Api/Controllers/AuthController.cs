using DatingApp.Api.Data;
using DatingApp.Api.Dto;
using DatingApp.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
      private readonly  IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
          _authRepository=authRepository;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegistrationDto userRegistrationDto){
            if(!ModelState.IsValid){
                return new BadRequestObjectResult("Model is InValid");
            }
            userRegistrationDto.Username=userRegistrationDto.Username.ToLower();
            if(await _authRepository.UserExists(userRegistrationDto.Username))
            {
                return new BadRequestObjectResult("User All Ready Exists");
            }
            var userToCreate=new User(){
                UserName=userRegistrationDto.Username
            };
            var createUser= await  _authRepository.Register(userToCreate,userRegistrationDto.Password);
            return new StatusCodeResult(201);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto  userLoginDto){
            if(!ModelState.IsValid){
                return new BadRequestObjectResult("Model is InValid");
            }
            var LoginedUser=await _authRepository.Login(userLoginDto.UserName.ToString(),userLoginDto.Password);
            if(LoginedUser==null)
            return new UnauthorizedObjectResult("UserName and Password Incorrect");
            var claim=new []{
                new Claim(ClaimTypes.NameIdentifier,LoginedUser.Id.ToString()),
                new Claim(ClaimTypes.Name,LoginedUser.UserName)
            };
            var PrivateKey ="this is a key which is used by athentication to convert a Token";
            var  key=new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(PrivateKey));
            var signingCread=new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDecriptor=new SecurityTokenDescriptor(){
                Subject=new ClaimsIdentity(claim),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials=signingCread
            };
            var tokenHandler=new JwtSecurityTokenHandler();
            var token=tokenHandler.CreateToken(tokenDecriptor);
            return new OkObjectResult(new {token=tokenHandler.WriteToken(token)});

        }
    }
}