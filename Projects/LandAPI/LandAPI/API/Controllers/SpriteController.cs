using Microsoft.AspNetCore.Mvc;

namespace LandAPI.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SpriteController : ControllerBase
    {
        [HttpGet("{fileName}")]
        public IActionResult GetModel(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assets/Sprites", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Model file not found.");
            }

            string mimeType = fileName.ToLower().EndsWith(".png") ? "image/png" : "application/octet-stream";

            return PhysicalFile(filePath, mimeType);
        }
    }
}
