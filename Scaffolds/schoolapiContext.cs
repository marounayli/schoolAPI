using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AppDist.Scaffolds
{
    public partial class schoolapiContext : DbContext
    {
        public schoolapiContext()
        {
        }

        public schoolapiContext(DbContextOptions<schoolapiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Coursemembership> Coursemembership { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=root;database=schoolAPI");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PRIMARY");

                entity.ToTable("admin");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("course");

                entity.HasIndex(e => e.TeacherId)
                    .HasName("fk_course_teacher");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CourseCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CourseName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TeacherId)
                    .HasColumnName("TeacherID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("fk_course_teacher");
            });

            modelBuilder.Entity<Coursemembership>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.CourseId })
                    .HasName("PRIMARY");

                entity.ToTable("coursemembership");

                entity.HasIndex(e => e.CourseId)
                    .HasName("fk_course");

                entity.Property(e => e.StudentId)
                    .HasColumnName("StudentID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CourseId)
                    .HasColumnName("CourseID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Coursemembership)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("fk_course");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Coursemembership)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("fk_student");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("student");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("teacher");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
