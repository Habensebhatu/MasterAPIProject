using Data_layer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data_layer.Context.Data
{
    public class MyDbcontextSofani : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Category> Category { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<LoginEnitiyModel> Login { get; set; }
        public DbSet<CustomerEntityModel> Customer { get; set; }
        public DbSet<CartEnityModel> Cart { get; set; }
        public DbSet<ProductImageEnityModel> ProductImage { get; set; }
        public DbSet<UserRegistrationEntityModel> UserRegistration { get; set; }
        public DbSet<WishlistEntityModel> WishlistEntityModel { get; set; }
        public DbSet<ProductWishlist> ProductWishlist { get; set; }


        public MyDbcontextSofani(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MyDbcontextSofani(DbContextOptions<MyDbcontextSofani> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("SofaniMarket");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRegistrationEntityModel>().OwnsOne(u => u.Address);

            modelBuilder.Entity<Product>()
                .Property(p => p.PiecePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
               .Property(p => p.CratePrice)
               .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartEnityModel>()
              .Property(p => p.Price)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CartEnityModel>()
             .Property(p => p.Kilo)
             .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
            .Property(p => p.Kilo)
            .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>()
               .Property(p => p.AmountTotal)
               .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>()
              .Property(p => p.Price)
              .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerEntityModel>()
        .HasKey(c => c.CustomerId);
        }
    }
}

