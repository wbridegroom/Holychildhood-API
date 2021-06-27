﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Database;

namespace api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("api.Models.Calendar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Calendars");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Parish"
                        });
                });

            modelBuilder.Entity("api.Models.CalendarContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CalendarId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId");

                    b.ToTable("CalendarContents");
                });

            modelBuilder.Entity("api.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AllDay")
                        .HasColumnType("bit");

                    b.Property<DateTime>("BeginDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CalendarId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EventTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRecurring")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RecurrenceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId");

                    b.HasIndex("EventTypeId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("api.Models.EventType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EventTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Color = "Blue",
                            Name = "Standard"
                        },
                        new
                        {
                            Id = 2,
                            Color = "Red",
                            Name = "Meeting"
                        },
                        new
                        {
                            Id = 3,
                            Color = "Green",
                            Name = "Mass"
                        },
                        new
                        {
                            Id = 4,
                            Color = "Gray",
                            Name = "Holiday"
                        });
                });

            modelBuilder.Entity("api.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BlobId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FileContentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FileContentId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("api.Models.FileContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FileContents");
                });

            modelBuilder.Entity("api.Models.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MenuItems");
                });

            modelBuilder.Entity("api.Models.Page", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int?>("MenuItemId")
                        .HasColumnType("int");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MenuItemId");

                    b.HasIndex("ParentId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("api.Models.PageContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CalendarContentId")
                        .HasColumnType("int");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FileContentId")
                        .HasColumnType("int");

                    b.Property<bool>("HasTitle")
                        .HasColumnType("bit");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("PageId")
                        .HasColumnType("int");

                    b.Property<int?>("TabContentId")
                        .HasColumnType("int");

                    b.Property<int?>("TextContentId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CalendarContentId");

                    b.HasIndex("FileContentId");

                    b.HasIndex("PageId");

                    b.HasIndex("TabContentId");

                    b.HasIndex("TextContentId");

                    b.ToTable("PageContents");
                });

            modelBuilder.Entity("api.Models.Tab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("TabContentId")
                        .HasColumnType("int");

                    b.Property<int?>("TextContentId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TabContentId");

                    b.HasIndex("TextContentId");

                    b.ToTable("Tabs");
                });

            modelBuilder.Entity("api.Models.TabContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("TabContents");
                });

            modelBuilder.Entity("api.Models.TextContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TextContents");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "Home Content 1"
                        },
                        new
                        {
                            Id = 2,
                            Content = "Home Content 2"
                        },
                        new
                        {
                            Id = 3,
                            Content = "Home Content 3"
                        },
                        new
                        {
                            Id = 4,
                            Content = "Home Content 4"
                        },
                        new
                        {
                            Id = 5,
                            Content = "Home Content 5"
                        },
                        new
                        {
                            Id = 6,
                            Content = "Home Content 6"
                        },
                        new
                        {
                            Id = 7,
                            Content = "Home Content 7"
                        },
                        new
                        {
                            Id = 8,
                            Content = "Home Content 8"
                        },
                        new
                        {
                            Id = 9,
                            Content = "Home Content 9"
                        },
                        new
                        {
                            Id = 10,
                            Content = "Home Content 10"
                        });
                });

            modelBuilder.Entity("api.Models.TextContentBackup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TextContentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TextContentId");

                    b.ToTable("TextContentsBackup");
                });

            modelBuilder.Entity("api.Models.CalendarContent", b =>
                {
                    b.HasOne("api.Models.Calendar", "Calendar")
                        .WithMany()
                        .HasForeignKey("CalendarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Calendar");
                });

            modelBuilder.Entity("api.Models.Event", b =>
                {
                    b.HasOne("api.Models.Calendar", "Calendar")
                        .WithMany("Events")
                        .HasForeignKey("CalendarId");

                    b.HasOne("api.Models.EventType", "EventType")
                        .WithMany()
                        .HasForeignKey("EventTypeId");

                    b.Navigation("Calendar");

                    b.Navigation("EventType");
                });

            modelBuilder.Entity("api.Models.File", b =>
                {
                    b.HasOne("api.Models.FileContent", null)
                        .WithMany("Files")
                        .HasForeignKey("FileContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api.Models.Page", b =>
                {
                    b.HasOne("api.Models.MenuItem", "MenuItem")
                        .WithMany("Pages")
                        .HasForeignKey("MenuItemId");

                    b.HasOne("api.Models.Page", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("MenuItem");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("api.Models.PageContent", b =>
                {
                    b.HasOne("api.Models.CalendarContent", "CalendarContent")
                        .WithMany()
                        .HasForeignKey("CalendarContentId");

                    b.HasOne("api.Models.FileContent", "FileContent")
                        .WithMany()
                        .HasForeignKey("FileContentId");

                    b.HasOne("api.Models.Page", "Page")
                        .WithMany("PageContents")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.TabContent", "TabContent")
                        .WithMany()
                        .HasForeignKey("TabContentId");

                    b.HasOne("api.Models.TextContent", "TextContent")
                        .WithMany()
                        .HasForeignKey("TextContentId");

                    b.Navigation("CalendarContent");

                    b.Navigation("FileContent");

                    b.Navigation("Page");

                    b.Navigation("TabContent");

                    b.Navigation("TextContent");
                });

            modelBuilder.Entity("api.Models.Tab", b =>
                {
                    b.HasOne("api.Models.TabContent", "TabContent")
                        .WithMany("Tabs")
                        .HasForeignKey("TabContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.TextContent", "TextContent")
                        .WithMany()
                        .HasForeignKey("TextContentId");

                    b.Navigation("TabContent");

                    b.Navigation("TextContent");
                });

            modelBuilder.Entity("api.Models.TextContentBackup", b =>
                {
                    b.HasOne("api.Models.TextContent", "TextContent")
                        .WithMany()
                        .HasForeignKey("TextContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TextContent");
                });

            modelBuilder.Entity("api.Models.Calendar", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("api.Models.FileContent", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("api.Models.MenuItem", b =>
                {
                    b.Navigation("Pages");
                });

            modelBuilder.Entity("api.Models.Page", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("PageContents");
                });

            modelBuilder.Entity("api.Models.TabContent", b =>
                {
                    b.Navigation("Tabs");
                });
#pragma warning restore 612, 618
        }
    }
}
