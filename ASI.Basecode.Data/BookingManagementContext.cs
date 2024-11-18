using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data
{
    public partial class BookingManagementContext : DbContext
    {
        public BookingManagementContext(DbContextOptions<BookingManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.HasKey(e => e.Id)
                    .HasName("PK_Reservation");

                entity.Property(e => e.BookingDate)
                    .HasColumnType("date")
                    .HasColumnName("booking_date");

                entity.Property(e => e.TimeIn)
                    .HasColumnType("time(0)")
                    .HasColumnName("time_in");

                entity.Property(e => e.TimeOut)
                    .HasColumnType("time(0)")
                    .HasColumnName("time_out");

                entity.Property(e => e.Duration)
                    .HasColumnName("duration");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasDefaultValue("RESERVED");

                entity.Property(e => e.ReservationDate)
                    .HasColumnType("date")
                    .HasColumnName("reservation_date");

                entity.HasIndex(e => new { e.RoomId, e.BookingDate, e.TimeIn })
                    .HasName("UQ_RoomBooking")
                    .IsUnique();

                // Configure relationships
                entity.HasOne(d => d.Customer)
                    .WithMany()
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Book_Customer");

                entity.HasOne(d => d.Room)
                    .WithMany()
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Book_Room");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.Roomname).HasColumnName("roomname");
                entity.Property(e => e.MaxCapacity).HasColumnName("max_capacity");
                entity.Property(e => e.Status).HasColumnName("Status");
                entity.Property(e => e.RoomNumber).HasColumnName("RoomNumber");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.Password).HasColumnName("Password");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CreatedTime).HasColumnName("CreatedTime");
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
                entity.Property(e => e.UpdatedTime).HasColumnName("UpdatedTime");
                entity.Property(e => e.Fname).HasColumnName("Fname");
                entity.Property(e => e.Lname).HasColumnName("Lname");
                entity.Property(e => e.Contact).HasColumnName("Contact");
                entity.Property(e => e.Status).HasColumnName("Status");
            });
        }
    }
}
