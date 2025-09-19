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
    public class CertificationController : ControllerBase
    {
        public readonly ApplicationDbContext DbContext;
        public CertificationController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }
        // GET: api/<CertificationController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Certifications>>> GetCertificates()
        {
            return await DbContext.Certifications.Include(b=>b.CertificateImageUrl).ToListAsync();
        }

        // GET api/<CertificationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CertificationController>
        [HttpPost("AddCertificate")]
        public async Task<ActionResult<Certifications>> PostCertificate([FromBody] Certifications certifications)
        {
            if (certifications == null)
            {
                return BadRequest("Certification is null.");
            }
            if (string.IsNullOrEmpty(certifications.Certification_Date))
            {
                return BadRequest("Certification Date and Certificate Image Url are required.");
            }
            if (certifications.CertificateImageUrl != null)
            {
                foreach (var image in certifications.CertificateImageUrl)
                {
                    if (string.IsNullOrEmpty(certifications.CertificateImageUrl))
                    {
                        return BadRequest("Each certifications image must have a valid URL.");
                    }
                }
            }
            DbContext.Certifications.Add(certifications);
            await DbContext.SaveChangesAsync();
            return Ok(certifications);
        }

        // PUT api/<CertificationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CertificationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
