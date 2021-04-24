using api.Database;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TabController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public TabController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/Tab/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tab>> GetTab(int id)
        {
            var tab = await dbContext.Tabs.FindAsync(id);

            if (tab == null) return NotFound();

            return tab;
        }

        // PUT: api/Tab/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutTab(int id, Tab tab)
        {
            if (id != tab.Id) return BadRequest();

            dbContext.Entry(tab).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TabExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/Tab
        [HttpPost]
        [Authorize]
        [ProducesResponseType(201)]
        public async Task<ActionResult<Tab>> PostTab(Tab tab)
        {
            await dbContext.Tabs.AddAsync(tab);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetTab", new { id = tab.Id }, tab);
        }

        // DELETE: api/Tab/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Tab>> DeleteTab(int id)
        {
            var tab = await dbContext.Tabs.FindAsync(id);
            if (tab == null) return NotFound();

            dbContext.Tabs.Remove(tab);
            await dbContext.SaveChangesAsync();

            return tab;
        }

        private bool TabExists(int id)
        {
            return dbContext.Tabs.Any(e => e.Id == id);
        }
    }
}
