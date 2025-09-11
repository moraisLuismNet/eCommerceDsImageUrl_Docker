using Microsoft.EntityFrameworkCore;

namespace eCommerceDs.Models
{
    public class eCommerceDsContext : DbContext
    {
        public eCommerceDsContext(DbContextOptions<eCommerceDsContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<MusicGenre> MusicGenres { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.IdGroup).HasName("PK__Groups__32DFFDB3F74504DB");

                entity.Property(e => e.NameGroup).HasMaxLength(100);

                entity.HasOne(d => d.MusicGenre).WithMany(p => p.Groups)
                    .HasForeignKey(d => d.MusicGenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Groups_MusicalGenres");
            });

            modelBuilder.Entity<MusicGenre>(entity =>
            {
                entity.HasKey(e => e.IdMusicGenre).HasName("PK__MusicalG__C2A4358176EF3AF4");

                entity.Property(e => e.NameMusicGenre).HasMaxLength(100);
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.HasKey(e => e.IdRecord).HasName("PK__Records__356CCF9A247285E1");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.TitleRecord).HasMaxLength(100);

                entity.HasOne(d => d.Group).WithMany(p => p.Records)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Records_Groups");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email).HasName("PK__Users__A9D10535B2F51717");

                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Password).HasMaxLength(500);
                entity.Property(e => e.Role).HasMaxLength(50);
            });

            // Configure Cart's primary key
            modelBuilder.Entity<Cart>()
                .HasKey(c => c.IdCart); 

            modelBuilder.Entity<Cart>()
                .Property(c => c.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cart>() 
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserEmail)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the primary key for Order
            modelBuilder.Entity<Order>()
                .HasKey(o => o.IdOrder); 

            modelBuilder.Entity<Order>()
                .Property(o => o.Total)
                .HasPrecision(18, 2);

            // Configure the relationship between Orders and Cart
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Cart)
                .WithMany()
                .HasForeignKey(o => o.CartId)
                .OnDelete(DeleteBehavior.NoAction); // Switch to NoAction to avoid cycles

            // Configure the relationship between Orders and Users
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserEmail)
                .OnDelete(DeleteBehavior.NoAction); // Switch to NoAction to avoid cycles

            // Configure the CartDetail primary key
            modelBuilder.Entity<CartDetail>()
                .HasKey(cd => cd.IdCartDetail);

            // Setting up a one-to-many relationship: Cart - CartDetail
            modelBuilder.Entity<CartDetail>()
                .HasOne(cd => cd.Cart)
                .WithMany(c => c.CartDetails)
                .HasForeignKey(cd => cd.CartId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<CartDetail>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            // Setting up a one-to-many relationship: CartDetail - Record
            modelBuilder.Entity<CartDetail>()
                .HasOne(cd => cd.Record)
                .WithMany()
                .HasForeignKey(cd => cd.RecordId)
                .OnDelete(DeleteBehavior.NoAction); // Switch to NoAction to avoid cycles

            // Configure the primary key for OrderDetail
            modelBuilder.Entity<OrderDetail>()
                    .HasKey(cd => cd.IdOrderDetail); 

            // Setting up a one-to-many relationship: Order - OrderDetail
            modelBuilder.Entity<OrderDetail>()
                    .HasOne(cd => cd.Order)
                    .WithMany(c => c.OrderDetails)
                    .HasForeignKey(cd => cd.OrderId)
                    .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<OrderDetail>()
                    .Property(c => c.Price)
                    .HasPrecision(18, 2);

            // Setting up a one-to-many relationship: OrderDetail - Record
            modelBuilder.Entity<OrderDetail>()
                    .HasOne(cd => cd.Record)
                    .WithMany()
                    .HasForeignKey(cd => cd.RecordId)
                    .OnDelete(DeleteBehavior.NoAction); // Switch to NoAction to avoid cycles
        }
        
    }
    
}

