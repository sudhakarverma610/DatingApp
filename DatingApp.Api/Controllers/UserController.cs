using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private IDatingRespository _datingRespository;
        private IMapper _mapper;

        public UserController(IDatingRespository datingRespository,IMapper mapper)
        {
            _datingRespository = datingRespository;
            _mapper = mapper;
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

    }
}
