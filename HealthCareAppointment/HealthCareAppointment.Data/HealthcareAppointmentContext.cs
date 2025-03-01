using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCareAppointment.Models;

namespace HealthCareAppointment.Data
{
    public class HealthcareAppointmentContext : DbContext
    {
        public HealthcareAppointmentContext(DbContextOptions<HealthcareAppointmentContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(u => u!.AppointmentsAsPatient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(u => u!.AppointmentsAsDoctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = new Guid("c93d1328-8109-48a9-9ecb-77402a64d933"),
                    Name = "Duong Ba Thanh",
                    Email = "quuq@gmail.com",
                    DateOfBirth = new DateTime(1999, 12, 12),
                    Password = "User@123",
                    Role = Models.Users.RoleEnum.Patient,
                    Specialization = "None"
                },
                new Users
                {
                    Id = new Guid("71d6e368-04cc-4766-b621-9d9f1c9e8f6e"),
                    Name = "Quanh Thanh Cong",
                    Email = "qtc@gmail.com",
                    DateOfBirth = new DateTime(1992, 11, 12),
                    Password = "User@123",
                    Role = Models.Users.RoleEnum.Doctor,
                    Specialization = "Dentist"
                },
                 new Users
                 {
                     Id = new Guid("09fabf72-c8a7-4776-ad9e-69f99449ae77"),
                     Name = "Duong Ba Trinh",
                     Email = "quu@gmail.com",
                     DateOfBirth = new DateTime(1977, 12, 12),
                     Password = "User@123",
                     Role = Models.Users.RoleEnum.Patient,
                     Specialization = "None"
                 },
                new Users
                {
                    Id = new Guid("46ab5b8a-8880-46e6-8b39-5f201e2a27e9"),
                    Name = "Hoang Quy",
                    Email = "quyquy909@gmail.com",
                    DateOfBirth = new DateTime(1992, 11, 12),
                    Password = "User@123",
                    Role = Models.Users.RoleEnum.Doctor,
                    Specialization = "Dr"
                },
                new Users
                {
                    Id = new Guid("362cfb46-a865-48c1-bc03-a01101a0075f"),
                    Name = "Hoang Anh",
                    Email = "qtc@gmail.com",
                    DateOfBirth = new DateTime(1992, 11, 12),
                    Password = "User@123",
                    Role = Models.Users.RoleEnum.Doctor,
                    Specialization = "Dr"
                }
                );

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    Id = new Guid("da0f0d1f-c75f-4a53-a5b0-d3e1b868952a"),
                    PatientId = new Guid("c93d1328-8109-48a9-9ecb-77402a64d933"),
                    DoctorId = new Guid("71d6e368-04cc-4766-b621-9d9f1c9e8f6e"),
                    Date = new DateTime(2021, 12, 12),
                    Status = Models.Appointment.StatusEnum.Scheduled
                },
                new Appointment
                {
                    Id = new Guid("12193e30-ae95-4792-ba5c-0d2a507cd940"),
                    PatientId = new Guid("362cfb46-a865-48c1-bc03-a01101a0075f"),
                    DoctorId = new Guid("09fabf72-c8a7-4776-ad9e-69f99449ae77"),
                    Date = new DateTime(2021, 12, 12),
                    Status = Models.Appointment.StatusEnum.Scheduled
                }
                );
        }
    }
}