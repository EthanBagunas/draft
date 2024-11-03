using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data
{
    public partial class BookingManagementContext : DbContext
    {
        public BookingManagementContext()
        {
        }

        public BookingManagementContext(DbContextOptions<BookingManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<RoomInformation> RoomInformations { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Addr=(localdb)\\MSSqlLocalDb; database=BookingManagement; Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustId);

                entity.ToTable("Customer");

                entity.Property(e => e.CustId)
                    .ValueGeneratedNever()
                    .HasColumnName("cust_ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(10)
                    .HasColumnName("address")
                    .IsFixedLength();

                entity.Property(e => e.Custfname)
                    .HasMaxLength(10)
                    .HasColumnName("custfname")
                    .IsFixedLength();

                entity.Property(e => e.Custlname)
                    .HasMaxLength(10)
                    .HasColumnName("custlname")
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .ValueGeneratedNever()
                    .HasColumnName("employee_ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(10)
                    .HasColumnName("address")
                    .IsFixedLength();

                entity.Property(e => e.ContactNum)
                    .HasMaxLength(10)
                    .HasColumnName("contact_num")
                    .IsFixedLength();

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Fname)
                    .HasMaxLength(10)
                    .HasColumnName("fname")
                    .IsFixedLength();

                entity.Property(e => e.JobDepartment)
                    .HasMaxLength(10)
                    .HasColumnName("job_department")
                    .IsFixedLength();

                entity.Property(e => e.Lname)
                    .HasMaxLength(10)
                    .HasColumnName("lname")
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("password")
                    .IsFixedLength();

                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("username")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservation");

                entity.Property(e => e.ReservationId)
                    .ValueGeneratedNever()
                    .HasColumnName("reservation_ID");

                entity.Property(e => e.CustomerIdFk).HasColumnName("customer_ID_FK");

                entity.Property(e => e.DateIn)
                    .HasColumnType("date")
                    .HasColumnName("date_in");

                entity.Property(e => e.DateOut)
                    .HasColumnType("date")
                    .HasColumnName("date_out");

                entity.Property(e => e.DateRange)
                    .HasColumnType("date")
                    .HasColumnName("date_range");

                entity.Property(e => e.ReservationDate)
                    .HasColumnType("date")
                    .HasColumnName("reservation_date");

                entity.Property(e => e.RoomId).HasColumnName("room_ID");
            });

            modelBuilder.Entity<RoomInformation>(entity =>
            {
                entity.HasKey(e => e.RoomId);

                entity.ToTable("Room Information");

                entity.Property(e => e.RoomId)
                    .ValueGeneratedNever()
                    .HasColumnName("room_ID");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Price)
                    .HasMaxLength(10)
                    .HasColumnName("price")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.TransactionId)
                    .ValueGeneratedNever()
                    .HasColumnName("transaction_ID");

                entity.Property(e => e.CustomerIdFk).HasColumnName("customer_ID_FK");

                entity.Property(e => e.EmployeeIdFk).HasColumnName("employee_ID_FK");

                entity.Property(e => e.ReservationIdFk).HasColumnName("reservation_ID_FK");

                entity.Property(e => e.TransactionDate)
                    .HasColumnType("date")
                    .HasColumnName("transaction_date");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
