using api.Database;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public PageController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/Page
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Page>>> GetPages()
        {
            return await dbContext.Pages.Where(p => p.Parent == null).Include(p => p.Children).ToListAsync();
        }

        // GET: api/Page/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPage(int id)
        {
            var page = await dbContext.Pages
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.MenuItem,
                    Parent = p.Parent != null ? new { p.Parent.Id, p.Parent.Title, p.Parent.MenuItem } : null,
                    Children = p.Children.Select(c => new {c.Id, c.Title, c.Index}).OrderBy(c => c.Index).ToList(),
                    PageContents = p.PageContents
                        .Select(pc => new {
                            pc.Id,
                            pc.ContentType,
                            pc.Index,
                            pc.HasTitle,
                            pc.Title,
                            pc.TextContent,
                            TabContent = pc.TabContent == null ? null : new
                            {
                                pc.TabContent.Id,
                                Tabs = pc.TabContent.Tabs.Select(t => new { t.Id, t.Title, t.Index, t.TabContentId, t.TextContent })
                            },
                            CaldendarContnet = pc.CalendarContent == null ? null : new
                            {
                                pc.CalendarContent.Id,
                                Calendar = pc.CalendarContent.Calendar == null ? null : new
                                {
                                    pc.CalendarContent.Calendar.Id,
                                    pc.CalendarContent.Calendar.Events
                                }
                            },
                            FileContent = pc.FileContent == null ? null : new
                            {
                                pc.FileContent.Id,
                                pc.FileContent.Title,
                                pc.FileContent.FileType,
                                pc.FileContent.Files
                            }
                        }).OrderBy(pc => pc.Index).ToList(),
                })
//                .Include(p => p.Parent).ThenInclude(p => p.MenuItem)
//                .Include(p => p.Parent).ThenInclude(p => p.Children)
//                .Include(p => p.MenuItem)
//                .Include(p => p.Children)
//                .Include(p => p.PageContents).ThenInclude(pc => pc.TextContent)
//                .Include(p => p.PageContents).ThenInclude(pc => pc.TabContent).ThenInclude(tc => tc.Tabs).ThenInclude(t => t.TextContent)
//                .Include(p => p.PageContents).ThenInclude(pc => pc.CalendarContent).ThenInclude(cc => cc.Calendar).ThenInclude(c => c.Events)
//                .Include(p => p.PageContents).ThenInclude(pc => pc.FileContent).ThenInclude(fc => fc.Files)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (page == null) return NotFound();

            //page.Children = page.Children?.OrderBy(c => c.Index).ToList();
            //page.PageContents = page.PageContents?.OrderBy(pc => pc.Index).ToList();

            return page;
        }

        // PUT: api/Page/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutPage(int id, Page page)
        {
            if (id != page.Id) return BadRequest();

            dbContext.Entry(page).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/Page
        [HttpPost]
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Page>> PostPage(Page page)
        {
            try
            {
                if (page.Parent != null)
                {
                    var parent = await dbContext.Pages.Include(p => p.Children)
                        .FirstOrDefaultAsync(p => p.Id == page.Parent.Id);
                    parent?.Children.Add(page);
                }
                else
                {
                    await dbContext.Pages.AddAsync(page);
                }

                await dbContext.SaveChangesAsync();
                return CreatedAtAction("GetPage", new { id = page.Id }, page);
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        // DELETE: api/Page/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Page>> DeletePage(int id)
        {
            try
            {
                var page = await dbContext.Pages.FindAsync(id);
                if (page == null) return NotFound();

                dbContext.Pages.Remove(page);
                await dbContext.SaveChangesAsync();

                return page;
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        private bool PageExists(int id)
        {
            return dbContext.Pages.Any(e => e.Id == id);
        }
    }
}
