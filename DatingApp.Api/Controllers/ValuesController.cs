using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ValuesController:ControllerBase
    {
        public ValuesController()
        {
            
        }
        public ActionResult<IEnumerable<string>> GetValues(){
            ///throw new System.Exception("some excepitoon Occured");
            return new string[]{"values1","value2"};
        }
    }
}