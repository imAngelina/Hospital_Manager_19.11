using Data_Hospital_Manager;
using Data_Hospital_Manager.Entities;
using HospitalManagerServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagerServices.Services
{
    public class HospitalService:IHospitalService
    {
        private readonly HospitalDbContext new_context;

        public HospitalService(HospitalDbContext context)
        {
            new_context=context;
        }

        public async Task AddHospitalAsync(Hospital hospital)
        {
            new_context.Hospitals.Add(hospital);
            await new_context.SaveChangesAsync();
        }

        public async Task DeleteHospitalAsync(int id)
        {
            var hospital = await new_context.Hospitals.FindAsync(id);
            if(hospital == null)
            {
                return;
            }
            bool hasdoctors = await new_context.Doctors.AnyAsync(x=>x.HospitalId==id);

            if (hasdoctors)
            {
                throw new InvalidOperationException("cannot be deleted becaouse of existing doctor");
            }
            hospital.IsDeleted = true;
            await new_context.SaveChangesAsync();
        }

        public async Task<List<Hospital>> GetAllHospitalsAsync(string? search, int? minBeds, string? sortOreder)
        {
            var query = new_context.Hospitals.Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(h => h.Name.Contains(search));
            if(minBeds.HasValue)
                query=query.Where(x=>x.BedsCount>=minBeds.Value);
            switch (sortOreder)
            {
                case "name":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "beds_asc":
                    query = query.OrderBy(x => x.BedsCount);
                    break;
                case "beds_desc":
                    query = query.OrderByDescending(x => x.BedsCount);
                    break;
                default:
                    query = query.OrderBy(x => x.Name);
                    break;
            }
            return await query.ToListAsync();
        } 

        public async Task<Hospital?> GetHospitalByIdAsync(int id)
        {
            return await new_context.Hospitals.FindAsync(id);
        }

        public async Task UpdateHospitalAsync(Hospital hospital)
        {
            await new_context.SaveChangesAsync();
        }
    }
}
