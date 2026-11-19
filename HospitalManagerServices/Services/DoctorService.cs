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
    public class DoctorService : IDoctorService
    {
        private readonly HospitalDbContext new_context;

        public DoctorService(HospitalDbContext context)
        {
            new_context = context;
        }
        public async Task AddDoctorAsync(Doctor doctor)
        {
            new_context.Doctors.Add(doctor);
            await new_context.SaveChangesAsync();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await new_context.Doctors.Include(x => x.DoctorsPatients).FirstOrDefaultAsync(x => x.Id == id);
            if (doctor != null)
            {
                new_context.DoctorsPatients.RemoveRange(doctor.DoctorsPatients);
                new_context.Doctors.Remove(doctor);
                await new_context.SaveChangesAsync();
            }
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync(string? search, string? speciality, string? sortOreder)
        {
            var query = new_context.Doctors.Include(x => x.Hospital).Where(d => true);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(h => h.FirstName.Contains(search) || h.LastName.Contains(search));
            if (!string.IsNullOrWhiteSpace(speciality))
                query = query.Where(x => x.Specialty.Contains(speciality));
            switch (sortOreder)
            {
                case "name_asc":
                    query = query.OrderBy(x => x.FirstName).ThenBy(x=>x.LastName);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName);
                    break;
                case "speciality":
                    query = query.OrderBy(x => x.Specialty).ThenBy(x => x.FirstName);
                    break;
                default:
                    query = query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
                    break;
            }
            return await query.ToListAsync();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await new_context.Doctors.FindAsync(id);
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            await new_context.SaveChangesAsync();
        }
        public async Task<Doctor?> GetDoctorDetailsAsync(int id)
        {
            return await new_context.Doctors.Include(x=>x.Hospital).Include(x=>x.DoctorsPatients).ThenInclude(x=>x.Patient).FirstOrDefaultAsync(x=>x.Id==id);
        }

       
    }
}
