using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CarsApp.Models.Database
{
    public partial class CarsAppContext : DbContext
    {
        public CarsAppContext()
        {
        }

        public CarsAppContext(DbContextOptions<CarsAppContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Model> Models { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=гыук;password=password;database=CarsApp", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brand");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");
                List <Brand> brands = new();
                for (int i = 0; i < 10; i++)
                {
                    brands.Add(new()
                    {
                        Id = i + 1,
                        Name = $"Марка {i + 1}",
                        IsActive = (i + 1) % 2 == 0
                    });
                }
                entity.HasData(brands);
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.ToTable("model");

                entity.HasIndex(e => e.BrandId, "fk_brand_id_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("fk_brand_id");


                List<Model> models = new();
                for (int i = 0; i < 50; i++)
                {
                    models.Add(new()
                    {
                        Id = i + 1,
                        Name = $"Модель {i + 1}",
                        BrandId = new Random().Next(1, 10),
                        IsActive = (i + 1) % 2 == 0
                    });
                }
                entity.HasData(models);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
