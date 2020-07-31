using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using DatingApi.Data.Models;
using DatingApi.Data.Repositories;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        DatingDbContext dbContext;
        public ValuesController(DatingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // GET api/values
        [HttpGet]
		public async Task<ActionResult> Get()
        {
            var values = await dbContext.Values.ToListAsync();
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var value = await dbContext.Values.FindAsync(new {Id = id});
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Value value)
        {
            this.dbContext.Add<Value>(value);
            await this.dbContext.SaveChangesAsync();
            return Ok(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var value = await dbContext.Values.FindAsync(new {Id = id} );
            return Ok("Deleted");
        }
    }
}
