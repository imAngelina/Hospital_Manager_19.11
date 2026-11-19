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
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasData(
    new Patient
    {
        Id = 1,
        FirstName = "Petar",
        LastName = "Georgiev",
        Email = "petar@gmail.com",
        PhoneNumber = "0888444444"
    },
    new Patient
    {
        Id = 2,
        FirstName = "Elena",
        LastName = "Petrova",
        Email = "elena@gmail.com",
        PhoneNumber = "0888555555"
    },
    new Patient
    {
        Id = 3,
        FirstName = "Nikolay",
        LastName = "Ivanov",
        Email = "nikolay@gmail.com",
        PhoneNumber = "0888666666"
    }
);
        }
    }
}
