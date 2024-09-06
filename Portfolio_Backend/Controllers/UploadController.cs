using Microsoft.AspNetCore.Mvc;

namespace Portfolio_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        // GET: api/Upload
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Upload/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Upload/UploadImage
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Ensure that the file is an image by checking MIME types
            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Invalid file type. Only .jpg, .jpeg, .png, .gif are allowed.");

            try
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadPath); // Ensure the folder exists

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName); // Generate a unique file name
                var filePath = Path.Combine(uploadPath, fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Construct the URL for the uploaded image
                var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

                // Return the URL to the uploaded file
                return Ok(new { Url = fileUrl });
            }
            catch (Exception ex)
            {
                // Log error if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // PUT: api/Upload/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Upload/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
