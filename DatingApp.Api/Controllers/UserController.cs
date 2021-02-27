using System.Data.Common;
using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using DatingApp.Api.Helpers;
using Microsoft.Extensions.Options;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private IDatingRespository _datingRespository;
        private IMapper _mapper;
        private IOptions<CloudinarySetting> _cloudinarySetting;
        public UserController(IDatingRespository datingRespository,IMapper mapper,
            IOptions<CloudinarySetting> cloudinarySetting)
        {
            _datingRespository = datingRespository;
            _mapper = mapper;
            _cloudinarySetting = cloudinarySetting;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(_mapper.Map<IEnumerable<UserListDto>>(await _datingRespository.GetUsers()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUser(int Id)
        {
            return Ok(_mapper.Map<UserDetailDto>(await _datingRespository.GetUser(Id)));
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser(int Id,UserForUpdateDto userForUpdateDto)
        {
            if (Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var userfromRepo=(await _datingRespository.GetUser(Id));
            _mapper.Map(userForUpdateDto, userfromRepo);
            if (await _datingRespository.SaveAll())
                return NoContent();
            throw new Exception($"Updating user {Id} failed on save");
        }

    }
}
