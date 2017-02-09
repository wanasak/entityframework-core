using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ef_core.Models
{
    public class Model
    {
        public class BloggingContext : DbContext
        {
            public BloggingContext(DbContextOptions<BloggingContext> options) : base (options) { }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Ignore<BlogMetaData>();
                modelBuilder.Entity<Blog>()
                    .Ignore(b => b.LoadedFromDatabase);
                modelBuilder.Entity<Member>()
                    .HasKey(m => m.MemberKey);
                // composite key
                //modelBuilder.Entity<Member>()
                //    .HasKey(m => new { m.MemberKey, m.Name });
            }
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Post> Posts { get; set; }
            public DbSet<Member> Members { get; set; }
        }
        public class Blog
        {
            public int BlogId { get; set; }
            public string Url { get; set; }
            public DateTime LoadedFromDatabase { get; set; }

            public List<Post> Posts { get; set; }
        }
        public class Post
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public int BlogId { get; set; }
            public Blog Blog { get; set; }
        }
        public class BlogMetaData
        {
            public DateTime LoadedFromDatabase { get; set; }
        }
        public class Member
        {
            public int MemberKey { get; set; }
            public string Name { get; set; }
        }
    }
}
