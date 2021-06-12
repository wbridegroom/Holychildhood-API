﻿using api.Database;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PageContentController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public PageContentController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/PageContent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PageContent>>> GetPageContents()
        {
            return await dbContext.PageContents.ToListAsync();
        }

        // GET: api/PageContent/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<object>> GetPageContent(int id)
        {
            var pageContent = await dbContext.PageContents
                .Select(pc => new
                {
                    pc.Id,
                    pc.Index,
                    pc.TextContent,
                    TabContent = pc.TabContent == null ? null : new { 
                        pc.TabContent.Id, 
                        Tabs = pc.TabContent.Tabs.Select(t => new { t.Id, t.Title, t.Index, t.TabContentId, t.TextContent })
                    },
                    CaldendarContnet = pc.CalendarContent == null ? null : new {
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
                }).OrderBy(pc => pc.Id)
                //.Include(pc => pc.TextContent)
                //.Include(pc => pc.TabContent).ThenInclude(tc => tc.Tabs).ThenInclude(t => t.TextContent)
                //.Include(pc => pc.CalendarContent).ThenInclude(cc => cc.Calendar).ThenInclude(c => c.Events)
                //.Include(pc => pc.FileContent).ThenInclude(fc => fc.Files)
                //.OrderBy(pc => pc.Index)
                .FirstOrDefaultAsync(pc => pc.Id == id);

            if (pageContent == null) return NotFound();

            return pageContent;
        }

        // PUT: api/PageContent/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutPageContent(int id, PageContent pageContent)
        {
            if (id != pageContent.Id) return BadRequest();

            dbContext.Entry(pageContent).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageContentExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/PageContent
        [HttpPost]
        [Authorize]
        [ProducesResponseType(201)]
        public async Task<ActionResult<PageContent>> PostPageContent(PageContent pageContent)
        {
            var page = await dbContext.Pages.Include(p => p.PageContents).FirstOrDefaultAsync(p => p.Id == pageContent.Page.Id);
            if (page == null) return NotFound();
            pageContent.Page = page;

            // Find max Index
            var index = 0;
            foreach (var content in page.PageContents)
            {
                if (content.Index >= index) index = content.Index + 1;
            }
            pageContent.Index = index;

            await dbContext.PageContents.AddAsync(pageContent);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetPageContent", new { id = pageContent.Id }, pageContent);
        }

        // DELETE: api/PageContent/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PageContent>> DeletePageContent(int id)
        {
            var pageContent = await dbContext.PageContents
                .Include(pc => pc.TextContent)
                .Include(pc => pc.TabContent).ThenInclude(tc => tc.Tabs).ThenInclude(t => t.TextContent)
                .Include(pc => pc.CalendarContent).ThenInclude(cc => cc.Calendar).ThenInclude(c => c.Events)
                .Include(pc => pc.FileContent).ThenInclude(fc => fc.Files)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (pageContent == null) return NotFound();

            if (pageContent.TextContent != null)
            {
                dbContext.TextContents.Remove(pageContent.TextContent);
            }

            if (pageContent.TabContent != null)
            {
                foreach (var tab in pageContent.TabContent.Tabs)
                {
                    dbContext.TextContents.Remove(tab.TextContent);
                }
                dbContext.TabContents.Remove(pageContent.TabContent);
            }

            if (pageContent.FileContent != null)
            {
                foreach (var file in pageContent.FileContent.Files)
                {
                    dbContext.Files.Remove(file);
                }

                dbContext.FileContents.Remove(pageContent.FileContent);
            }

            if (pageContent.CalendarContent != null)
            {
                dbContext.CalendarContents.Remove(pageContent.CalendarContent);
            }

            dbContext.PageContents.Remove(pageContent);
            await dbContext.SaveChangesAsync();

            return pageContent;
        }

        [HttpPost("moveup/{id}")]
        [Authorize]
        public async Task<IActionResult> MoveContentUp(int id)
        {
            var content = await dbContext.PageContents.FindAsync(id);
            if (content.Index == 0) return NoContent();

            var prevContent = await dbContext.PageContents.Where(pc => pc.PageId == content.PageId && pc.Index == (content.Index - 1)).FirstAsync();
            if (prevContent == null) return NoContent();

            content.Index = content.Index - 1;
            prevContent.Index = prevContent.Index + 1;

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("movedown/{id}")]
        [Authorize]
        public async Task<IActionResult> MoveContentDown(int id)
        {
            var content = await dbContext.PageContents.FindAsync(id);
            var total = await dbContext.PageContents.CountAsync(pc => pc.PageId == content.PageId);
            if (content.Index == total - 1) return NoContent();

            var nextContent = await dbContext.PageContents.Where(pc => pc.PageId == content.PageId && pc.Index == (content.Index + 1)).FirstAsync();
            if (nextContent == null) return NoContent();

            content.Index = content.Index + 1;
            nextContent.Index = nextContent.Index - 1;

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool PageContentExists(int id)
        {
            return dbContext.PageContents.Any(e => e.Id == id);
        }
    }
}
