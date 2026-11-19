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
    public class DoctorPatientConfiguration : IEntityTypeConfiguration<DoctorPatient>
    {
        public void Configure(EntityTypeBuilder<DoctorPatient> builder)
        {
            builder.HasKey(x => new { x.DoctorId, x.PatientId });
            builder.HasOne(x=>x.Doctor).WithMany(x=>x.DoctorsPatients).HasForeignKey(x=>x.DoctorId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=>x.Patient).WithMany(x=>x.DoctorsPatients).HasForeignKey(x=>x.PatientId).OnDelete(DeleteBehavior.Cascade);


            builder.HasData(
    new DoctorPatient
    {
        DoctorId = 1,
        PatientId = 1
    },
    new DoctorPatient
    {
        DoctorId = 1,
        PatientId = 2
    },
    new DoctorPatient
    {
        DoctorId = 2,
        PatientId = 3
    },
    new DoctorPatient
    {
        DoctorId = 3,
        PatientId = 1
    }
);
        }
    }
}
