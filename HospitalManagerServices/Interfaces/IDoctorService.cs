using Data_Hospital_Manager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagerServices.Interfaces
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetAllDoctorsAsync(string? search = null, string? speciality = null, string? sortOreder = null);
        Task<Doctor?> GetDoctorByIdAsync(int id);
        Task<Doctor?> GetDoctorDetailsAsync(int id);
        Task AddDoctorAsync(Doctor d);
        Task UpdateDoctorAsync(Doctor d);
        Task DeleteDoctorAsync(int id);
    }
}
