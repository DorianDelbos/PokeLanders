using Microsoft.AspNetCore.Mvc;

namespace LandAPI.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        [HttpGet("{fileName}")]
        public IActionResult GetModel(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assets/Models", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Model file not found.");
            }

            string mimeType = fileName.ToLower().EndsWith(".glb") ? "model/gltf-binary" : "application/octet-stream";

            return PhysicalFile(filePath, mimeType);
        }
    }
}
