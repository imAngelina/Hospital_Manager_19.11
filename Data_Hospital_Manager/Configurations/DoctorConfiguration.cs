using Data_Hospital_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Data_Hospital_Manager.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(h => h.FirstName).IsRequired().HasMaxLength(30);
            builder.Property(h => h.LastName).IsRequired().HasMaxLength(30);
            builder.Property(h => h.Specialty).IsRequired().HasMaxLength(50);
            builder.Property(h => h.PhoneNumber).IsRequired();
            builder.Property(h => h.Email).IsRequired();
            builder.HasOne(x=>x.Hospital).WithMany(x=>x.Doctors).HasForeignKey(x=>x.HospitalId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.User).WithOne(x => x.Doctor).HasForeignKey<Doctor>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
     new Doctor
     {
         Id = 1,
         FirstName = "Ivan",
         LastName = "Petrov",
         Specialty = "Cardiology",
         Email = "ivan@hospital.bg",
         PhoneNumber = "0888111111",
         HospitalId = 1
     },
     new Doctor
     {
         Id = 2,
         FirstName = "Maria",
         LastName = "Ivanova",
         Specialty = "Neurology",
         Email = "maria@hospital.bg",
         PhoneNumber = "0888222222",
         HospitalId = 1
     },
     new Doctor
     {
         Id = 3,
         FirstName = "Georgi",
         LastName = "Dimitrov",
         Specialty = "Surgery",
         Email = "georgi@hospital.bg",
         PhoneNumber = "0888333333",
         HospitalId = 2
     }
 );
        }
    }
}
