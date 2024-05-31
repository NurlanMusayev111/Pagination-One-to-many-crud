using FiorelloSlider_OnetoMany.Models;
using Microsoft.EntityFrameworkCore;

namespace FiorelloSlider_OnetoMany.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderInfo> SliderInfos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Setting> Settings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                        .HasData(
            new Blog
            {
                Id = 1,
                Title = "Blog title1",
                Description = "description",
                Date = DateTime.Now,
                Image = "blog-feature-img-1.jpg"
            },
            new Blog
            {
                Id = 2,
                Title = "Blog title2",
                Description = "description",
                Date = DateTime.Now,
                Image = "blog-feature-img-3.jpg"
            },
            new Blog
            {
                Id = 3,
                Title = "Blog title3",
                Description = "description",
                Date = DateTime.Now,
                Image = "blog-feature-img-4.jpg"
            }
        );
        }
    }
}
