using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Api.Data;
using DatingApp.Api.Dto;
using DatingApp.Api.Helpers;
using DatingApp.Api.Model;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/photos")]
    public class PhotosController: ControllerBase
    {
        private IDatingRespository _datingRespository;
        private IMapper _mapper;
        private  IOptions<CloudinarySetting> _cloudinarySetting;
        private Cloudinary _cloudinary;
        public PhotosController(IDatingRespository datingRespository, IMapper mapper,
            IOptions<CloudinarySetting> cloudinarySetting)
        {
            _datingRespository = datingRespository;
            _mapper = mapper;
            _cloudinarySetting = cloudinarySetting;
            Account account = new Account(
                                    cloudinarySetting.Value.CloudName,
                                    cloudinarySetting.Value.ApiKey,
                                    cloudinarySetting.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }
        [HttpGet("{id}",Name ="GetPhoto")]

        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _datingRespository.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }
            [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,[FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var userFromRepo = await _datingRespository.GetUser(userId);
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            if ( file!= null&&file.Length > 0)
            {
                using (var stream =file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name,stream),
                        Transformation = new Transformation().Width(500).Height(550).Crop("fill").Gravity("face")

                };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);

                }

            }
            else
            {
                return BadRequest("Bad Request");
            }
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;
            var photo = _mapper.Map<Photo>(photoForCreationDto);
            if (!userFromRepo.Photos.Any(x => x.IsMain))
                photo.IsMain = true;
            userFromRepo.Photos.Add(photo);
            if(await _datingRespository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo); 
                return CreatedAtAction(nameof(GetPhoto), new {userId=userId,id= photo.Id}, photoToReturn);
            }
            //GetPhoto
            return BadRequest("Photo Cloud not Added");
             
        }
        [HttpPost("{photoid}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId,int photoid)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var user = await _datingRespository.GetUser(userId);
            if (!user.Photos.Any(p => p.Id == photoid))
                Unauthorized();
            var photofromRepo = await _datingRespository.GetPhoto(photoid);
            if (photofromRepo.IsMain)
                return BadRequest("Photo Allready Main");

            var currentMainPhoto = (await _datingRespository.GetMainPhoto(userId));
            currentMainPhoto.IsMain = false;
            photofromRepo.IsMain = true;
            if (await _datingRespository.SaveAll())
            {
                return NoContent();    
            }
            //GetPhoto
            return BadRequest("Could not set Main Photo");
             
        }
     
    }
}
