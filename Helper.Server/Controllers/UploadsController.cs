using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Helper.Server.Controllers
{

    [ApiController]
    //[Route("[controller]")]
    [Route("api/Files/Upload")]
    public class UploadsController: ControllerBase
    {

        private readonly IWebHostEnvironment Environment;
        public UploadsController(IWebHostEnvironment env)
        {
            Environment = env;
        }

        [HttpPost]
        [Route("api/Files/Upload")]
        public async Task<ActionResult> Post(IFormFile file)
        {
            var filePath = Path.Combine(Environment.ContentRootPath,"Uploads", file.FileName);
            using var stream = System.IO.File.OpenWrite(filePath);
            await file.CopyToAsync(stream);
            try
            {
                //HttpContext context = HttpContext.Current;
                //var httpRequest = HttpContext.Current.Request;
                //if (httpRequest.Files.Count > 0) {
                //    foreach (string file in httpRequest.Files)
                //    {
                //        var postedFile = httpRequest.Files[file];
                //        var fileName = postedFile.FileName.Split('\\').LastOrDefaut().Split('/').LastOrDefault();
                //        var filePath = HttpContext.Curreny.Server.MapPath("~/Uploads/" + fileName);
                //        postedFile.SaveAs(filePath);
                //        return "/Uploads/" + fileName;
                //    }
                //}
            
            }
            catch (Exception exeption) {
            }

            return Ok();
        }

        
    }
}
