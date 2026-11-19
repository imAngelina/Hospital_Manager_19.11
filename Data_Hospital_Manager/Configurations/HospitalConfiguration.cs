using Data_Hospital_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Hospital_Manager.Configurations
{
    public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
    {
        public void Configure(EntityTypeBuilder<Hospital> builder)
        {
            builder.Property(h=>h.Name).IsRequired().HasMaxLength(50);
            builder.Property(h=>h.Address).IsRequired();
            builder.Property(h=>h.Email).IsRequired();
            builder.Property(h=>h.PhoneNumber).IsRequired();
            builder.Property(h=>h.BedsCount).IsRequired();
            builder.HasData(
                new Hospital
                {
                    Id=1,
                    Name="mbal kazanlak",
                    Address="kazanlak",
                    Email="levkfm@fv.fv",
                    PhoneNumber="0123456789",
                    BedsCount=200,
                    CreatedOn = new DateTime(2026,1,1),   
                    IsDeleted=false
                },
                new Hospital
                {
                    Id = 2,
                    Name = "umbal st zagora",
                    Address = "stara zagora",
                    Email = "tgerv@fw.hv",
                    PhoneNumber = "0123456777",
                    BedsCount = 350,
                    CreatedOn = new DateTime(2026, 2, 1),
                    IsDeleted = false
                });
        }
    }
}
