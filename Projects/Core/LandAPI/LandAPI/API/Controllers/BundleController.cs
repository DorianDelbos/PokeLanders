using Microsoft.AspNetCore.Mvc;

namespace LandAPI.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BundleController : ControllerBase
    {
        [HttpGet("{fileName}")]
        public IActionResult GetModel(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assets/Bundles", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Model file not found.");
            }

            string mimeType = fileName.ToLower().EndsWith(".bundle") ? "application/vnd.bundle" : "application/octet-stream";

            return PhysicalFile(filePath, mimeType);
        }
    }
}
