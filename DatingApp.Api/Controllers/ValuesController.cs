using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize]
    public class ValuesController:ControllerBase
    {
        public ValuesController()
        {
            
        }
        [HttpGet("Values")]
        public ActionResult<IEnumerable<string>> GetValues(){
            ///throw new System.Exception("some excepitoon Occured");
            return new string[]{"values1","value2"};
        }
        [AllowAnonymous]
        [HttpGet("values/{id}")]
         public ActionResult<IEnumerable<string>> GetValue(int id){
            ///throw new System.Exception("some excepitoon Occured");
            return new string[]{"value2"};
        }
    }
}