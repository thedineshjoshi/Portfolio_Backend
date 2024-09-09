using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio_Backend.Data;
using Portfolio_Backend.Model;
using static Azure.Core.HttpHeader;

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
            if (!string.IsNullOrEmpty(blog.Label))
            {
                var label = await _context.Labels.FirstOrDefaultAsync(l => l.Name == blog.Label);

                if (label == null)
                {

                    label = new Label
                    {
                        Id = Guid.NewGuid(),
                        Name = blog.Label
                    };
                    _context.Labels.Add(label);
                    await _context.SaveChangesAsync();
                }
                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync();

                var BlogLabel = new BlogLabel
                {
                    BlogId = blog.Id,
                    LabelId = label.Id
                };
                _context.Blogs.Add(blog);
            }
            else
            {
                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(blog.Label))
            {
                await _context.SaveChangesAsync();
            }

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

        //Add Comment
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(Guid blogId, [FromBody] Comment comment)
        {
            if (comment == null || string.IsNullOrEmpty(comment.Name) || string.IsNullOrEmpty(comment.Content))
            {
                return BadRequest("Invalid comment data.");
            }

            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog == null)
            {
                return NotFound("Blog not found.");
            }
            comment.Id = Guid.NewGuid();
            comment.BlogId = blogId;
            comment.PostedAt = DateTime.Now;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        [HttpGet("GetCommentsByBlogId/{blogId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByBlogId(Guid blogId)
        {
            // Check if the blog exists
            var blogExists = await _context.Blogs.AnyAsync(b => b.Id == blogId);
            if (!blogExists)
            {
                return NotFound(new { Message = "Blog not found." });
            }

            // Fetch comments for the specified blog
            var comments = await _context.Comments
                                         .Where(c => c.BlogId == blogId)
                                         .OrderBy(c => c.PostedAt) // Optionally, order by creation date
                                         .ToListAsync();

            if (comments == null || comments.Count == 0)
            {
                return NotFound(new { Message = "No comments found for this blog." });
            }

            return Ok(comments);
        }

    }
}
