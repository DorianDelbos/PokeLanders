using Microsoft.AspNetCore.Mvc;

namespace LandAPI.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SpriteController : ControllerBase
    {
        [HttpGet("landers/{fileName}")]
        public IActionResult GetLander(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assets/Sprites/Landers", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Model file not found.");
            }

            string mimeType = fileName.ToLower().EndsWith(".png") ? "image/png" : "application/octet-stream";

            return PhysicalFile(filePath, mimeType);
        }

        [HttpGet("types/{fileName}")]
        public IActionResult GetSprite(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assets/Sprites/Types", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Sprite file not found.");
            }

            string mimeType = fileName.ToLower().EndsWith(".png") ? "image/png" : "application/octet-stream";

            return PhysicalFile(filePath, mimeType);
        }
    }
}
