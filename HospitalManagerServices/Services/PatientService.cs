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
    public class PatientService:IPatientService
    {
        private readonly HospitalDbContext new_context;

        public PatientService(HospitalDbContext context)
        {
            new_context = context;
        }
        public async Task AddPatientAsync(Patient patient)
        {
            new_context.Patients.Add(patient);
            await new_context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int id)
        {
            var patient = await new_context.Patients.Include(x => x.DoctorsPatients).FirstOrDefaultAsync(x => x.Id == id);
            if (patient != null)
            {
                new_context.DoctorsPatients.RemoveRange(patient.DoctorsPatients);
                new_context.Patients.Remove(patient);
                await new_context.SaveChangesAsync();
            }
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await new_context.Patients.Include(x=>x.DoctorsPatients).ThenInclude(x=>x.Doctor).ToListAsync();
        }

        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
            return await new_context.Patients.FindAsync(id);
        }

        public async Task UpdatePatientAsync(Patient patient, List<int> doctorIds)
        {
            patient.DoctorsPatients.Clear();
            foreach (var doctor in doctorIds.Distinct())
            {
                patient.DoctorsPatients.Add(new DoctorPatient
                {
                    DoctorId = doctor,
                    PatientId = patient.Id
                });
            }
            await new_context.SaveChangesAsync();
        }
        public async Task<Patient?> GetPatientWithDoctorsAsync(int id)
        {
            return await new_context.Patients.Include(x => x.DoctorsPatients).ThenInclude(x => x.Doctor).FirstOrDefaultAsync(x => x.Id == id);
        }

        
    }
}
