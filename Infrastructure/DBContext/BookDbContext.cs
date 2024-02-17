using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Infrastructure
{
    public partial class BookDbContext : DbContext
    {
        public BookDbContext()
        {

        }
        public BookDbContext(DbContextOptions<BookDbContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Book> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=ElmTestDb;User ID=sa;Password=P@ssw0rd;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");
                entity.HasKey(a => a.BookId);
                //entity.Property(r=>r.BookTitle)
               
                //entity.Property(r => r.BookInfoDto)
                //.HasConversion(
                //    v=> JsonConvert.SerializeObject(v),
                //    v=> JsonConvert.DeserializeObject<BookInfoDto>(v)
                //    );

                
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
