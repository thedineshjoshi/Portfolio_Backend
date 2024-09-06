using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio_Backend.Data;
using Portfolio_Backend.Model;

namespace Portfolio_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Blogs/GetBlog
        [HttpGet("GetBlog")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            return await _context.Blogs.Include(b => b.BlogImages).ToListAsync();
        }

        // GET: api/Blogs/GetBlogById/5
        [HttpGet("GetBlogById/{id}")]
        public async Task<ActionResult<Blog>> GetBlog(Guid id)
        {
            var blog = await _context.Blogs.Include(b => b.BlogImages).FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }

        // POST: api/Blogs/AddBlog
        [HttpPost("AddBlog")]
        public async Task<ActionResult<Blog>> PostBlog([FromBody] Blog blog)
        {
            if (blog == null)
            {
                return BadRequest("Blog data is null.");
            }

            // Validate required fields
            if (string.IsNullOrEmpty(blog.Title) || string.IsNullOrEmpty(blog.MetaDescription))
            {
                return BadRequest("Title and MetaDescription are required.");
            }

            // Ensure blogImages has valid data
            if (blog.BlogImages != null)
            {
                foreach (var image in blog.BlogImages)
                {
                    if (string.IsNullOrEmpty(image.ImageUrl))
                    {
                        return BadRequest("Each blog image must have a valid URL.");
                    }
                }
            }

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlog), new { id = blog.Id }, blog);
        }

        // PUT: api/Blogs/UpdateBlog/5
        [HttpPut("UpdateBlog/{id}")]
        public async Task<IActionResult> PutBlog(Guid id, Blog blog)
        {
            if (id != blog.Id)
            {
                return BadRequest();
            }

            _context.Entry(blog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Blogs/DeleteBlog/5
        [HttpDelete("DeleteBlog/{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogExists(Guid id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
