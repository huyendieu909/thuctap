using HoangXuanQuy.OnlinePainting.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoangXuanQuy.OnlinePainting.Data.Context
{
    public class OnlinePaintingContext(DbContextOptions<OnlinePaintingContext> options) : IdentityDbContext<Customer>(options)
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Painting> Paintings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Comment>()
            //.HasOne(c => c.Customer)
            //.WithMany(u => u.Comments)
            //.HasForeignKey(c => c.CustomerId) // Khóa ngoại
            //.HasPrincipalKey(u => u.Id); // Khóa chính

            modelBuilder.Entity<Order>().Property(c => c.TotalPrice).HasConversion(typeof(decimal));
            modelBuilder.Entity<Painting>().Property(x => x.Price).HasConversion(typeof(decimal));
            modelBuilder.Entity<Order>()
            .HasOne(o => o.Painting)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.PaintingId);

            //Cái này gọi là data seeding
            modelBuilder.Entity<Artist>()
                .HasData(
                new Artist { ArtistId = 1, Bio = "Male", Name = "John Ly Trý", BirthDate = new DateTime(2002, 11, 11), Nationality = "Đông Lào", Website = "abc.com.vn" },
                new Artist { ArtistId = 2, Bio = "Male", Name = "Hoàng Xuân Quý", BirthDate = new DateTime(1991, 10, 11), Nationality = "Việt Nam", Website = "abc.xyz.vn" },
                new Artist { ArtistId = 5, Name = "Leonardo da Vinci", Bio = "Renaissance artist", Website = "https://leonardo.com", BirthDate = new DateTime(1452, 4, 15), Nationality = "Italian" },
                new Artist { ArtistId = 6, Name = "Vincent van Gogh", Bio = "Post-Impressionist painter", Website = "https://vangogh.com", BirthDate = new DateTime(1853, 3, 30), Nationality = "Dutch" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 2, Name = "Thời kì phục hưng", Description = "Tranh thời phục hưng" },
                new Category { CategoryId = 3, Name = "Trường phái ấn tượng", Description = "Tranh trường phái ấn tượng" }
            );

            modelBuilder.Entity<Painting>().HasData(
                new Painting { PaintingId = 1, Title = "Mona Lisa", Description = "A portrait of Lisa Gherardini", Price = 1000000, ImageUrl = "monalisa.jpg", CreatedDate = new DateTime(1503, 1, 1), Dimensions = "77cm x 53cm", Medium = "Oil on poplar", ArtistId = 5, CategoryId = 2 },
                new Painting { PaintingId = 6, Title = "Starry Night", Description = "A depiction of the night sky", Price = 2000000, ImageUrl = "starrynight.jpg", CreatedDate = new DateTime(1889, 6, 1), Dimensions = "73.7cm x 92.1cm", Medium = "Oil on canvas", ArtistId = 6, CategoryId = 3 }
            );
            
            ////CustomerId = 1,
            //modelBuilder.Entity<Customer>().HasData(
            //    new Customer { Id = "b04a2b50-3d41-4b35-bdf2-6e5bfa245111", Name = "John Ly Duy", Email = "johnduy@example.com", PhoneNumber = "01234567889", Address = "123 Main St" }
            //);

            ////CustomerId = 1,
            //modelBuilder.Entity<Comment>().HasData(
            //    new Comment { CommentId = 1, Content = "Amazing artwork!", CommentDate = new DateTime(2022, 11, 11), Rating = 5, LikeCount = 10,  PaintingId = 1, CustomerId = "b04a2b50-3d41-4b35-bdf2-6e5bfa245111" }
            //);
            
            ////CustomerId = 1,
            //modelBuilder.Entity<Order>().HasData(
            //    new Order { OrderId = 1,  PaintingId = 1, OrderDate = new DateTime(2023, 12, 12), TotalPrice = 1000000, PaymentMethod = "Credit Card", OrderStatus = "Completed", ShippingAddress = "123 Main St", OrderNote = "Mang cẩn thận", CustomerId = "b04a2b50-3d41-4b35-bdf2-6e5bfa245111" }
            //);

        }
    }
}

