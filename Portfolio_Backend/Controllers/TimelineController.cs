using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio_Backend.Data;
using Portfolio_Backend.Model;
using System.Reflection.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Portfolio_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly ApplicationDbContext DbContext;
        public TimelineController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        // GET: api/<TimelineController>
        [HttpGet("GetTimeline")]
        public async Task<ActionResult<IEnumerable<Timeline>>> GetTimelines()
        {
            return await DbContext.Timelines.ToListAsync();
        }

        // GET api/<TimelineController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TimelineController>
        [Route("AddTimeline")]
        [HttpPost]
        public async Task<ActionResult<Timeline>> PostTimeline(Timeline timeline)
        {
            if (timeline == null)
            {
                return BadRequest("Empty timeline is null.");
            }
            if (string.IsNullOrEmpty(timeline.Label) || string.IsNullOrEmpty(timeline.Description))
            {
                return BadRequest("Title and Description is required");
            }
            DbContext.Timelines.Add(timeline);
            await DbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTimelines), new { id = timeline.Id }, timeline);
        }
        // PUT api/<TimelineController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TimelineController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
