﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace ef_core.Models
{
    public class Model
    {
        public class BloggingContext : DbContext
        {
            public BloggingContext(DbContextOptions<BloggingContext> options) : base(options) { }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Ignore<BlogMetaData>();
                modelBuilder.Entity<Blog>()
                    .Ignore(b => b.LoadedFromDatabase);
                modelBuilder.Entity<Blog>()
                    .Property(b => b.Url)
                    .IsRequired()
                    .HasMaxLength(500);
                modelBuilder.Entity<Blog>()
                    .Property(b => b.Url)
                    .HasField("_validatedUrl");
                modelBuilder.Entity<Member>()
                    .HasKey(m => m.MemberKey);
                // composite key
                //modelBuilder.Entity<Member>()
                //    .HasKey(m => new { m.MemberKey, m.Name });
                modelBuilder.Entity<Member>()
                    .Property(m => m.MustBeAssigned)
                    .ValueGeneratedNever();
                modelBuilder.Entity<Member>()
                    .Property(m => m.InsertedDate)
                    .ValueGeneratedOnAdd();
                modelBuilder.Entity<Member>()
                    .Property(m => m.UpdatedDate)
                    .ValueGeneratedOnAddOrUpdate();
                modelBuilder.Entity<Member>()
                    .Property(m => m.Name)
                    .IsConcurrencyToken();
                modelBuilder.Entity<Member>()
                    .Property(m => m.TimeStamp)
                    .ValueGeneratedOnAddOrUpdate()
                    .IsConcurrencyToken();
                modelBuilder.Entity<Member>()
                    .HasIndex()
                    .IsUnique();
                modelBuilder.Entity<Member>()
                    .HasAlternateKey(m => m.MemberCode);
                modelBuilder.Entity<Category>()
                    .Property<DateTime>("LastUpdated");
            }
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Post> Posts { get; set; }
            public DbSet<Member> Members { get; set; }
            public DbSet<Category> Categories { get; set; }
        }
        public class Blog
        {
            private string _validatedUrl;
            public int BlogId { get; set; }
            public string Url
            {
                get { return _validatedUrl; }
            }
            public void SetUrl(string url)
            {
                using (var client = new HttpClient())
                {   
                    var response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }   
                _validatedUrl = url;
            }
            public DateTime LoadedFromDatabase { get; set; }
            public byte? Rating { get; set; }

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
            public DateTime InsertedDate { get; set; }
            public DateTime UpdatedDate { get; set; }
            public string MustBeAssigned { get; set; }
            public byte TimeStamp { get; set; }
            public string Index { get; set; }
            public string MemberCode { get; set; }
        }
        public class Category
        {
            public int CategoryId { get; set; }
            public string Name { get; set; }
        }
    }
}
