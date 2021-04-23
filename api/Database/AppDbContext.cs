using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageContent> PageContents { get; set; }
        public DbSet<TextContent> TextContents { get; set; }
        public DbSet<FileContent> FileContents { get; set; }
        public DbSet<TabContent> TabContents { get; set; }
        public DbSet<Tab> Tabs { get; set; }
        public DbSet<CalendarContent> CalendarContents { get; set; }
        public DbSet<TextContentBackup> TextContentsBackup { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PageContent>().Property(pc => pc.ContentType);
            builder.Entity<PageContent>().HasOne(pc => pc.Page).WithMany(p => p.PageContents)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TextContent>().HasMany<TextContentBackup>().WithOne(tcb => tcb.TextContent).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Tab>().HasOne(t => t.TabContent).WithMany(tc => tc.Tabs).OnDelete(DeleteBehavior.Cascade);

            // Seed Data
            builder.Entity<EventType>().HasData(new EventType { Id = 1, Name = "Standard", Color = "Blue" });
            builder.Entity<EventType>().HasData(new EventType { Id = 2, Name = "Meeting", Color = "Red" });
            builder.Entity<EventType>().HasData(new EventType { Id = 3, Name = "Mass", Color = "Green" });
            builder.Entity<EventType>().HasData(new EventType { Id = 4, Name = "Holiday", Color = "Gray" });

            builder.Entity<Calendar>().HasData(new Calendar { Id = 1, Name = "Parish" });

            builder.Entity<TextContent>().HasData(new TextContent { Id = 1, Content = "Home Content 1" });
            builder.Entity<TextContent>().HasData(new TextContent { Id = 2, Content = "Home Content 2" });
            builder.Entity<TextContent>().HasData(new TextContent { Id = 3, Content = "Home Content 3" });
            builder.Entity<TextContent>().HasData(new TextContent { Id = 4, Content = "Home Content 4" });
            builder.Entity<TextContent>().HasData(new TextContent { Id = 5, Content = "Home Content 5" });
            builder.Entity<TextContent>().HasData(new TextContent { Id = 6, Content = "Home Content 6" });
            builder.Entity<TextContent>().HasData(new TextContent { Id = 7, Content = "Home Content 7" });
            builder.Entity<TextContent>().HasData(new TextContent { Id = 8, Content = "Home Content 8" });
            builder.Entity<TextContent>().HasData(new TextContent { Id = 9, Content = "Home Content 9" });
            builder.Entity<TextContent>().HasData(new TextContent { Id = 10, Content = "Home Content 10" });

            base.OnModelCreating(builder);
        }
    }
}
