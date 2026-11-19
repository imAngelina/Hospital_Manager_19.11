using Data_Hospital_Manager;
using Data_Hospital_Manager.Entities;
using Hospital_Manager.ViewModels.Patient;
using HospitalManagerServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Manager.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService ps;
        private readonly IDoctorService ds;

        public PatientController(IPatientService ps, IDoctorService ds)
        {
            this.ps = ps;
            this.ds = ds;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await ps.GetAllPatientsAsync();
            var model = new List<PatientIndexViewModel>();
            foreach (var patient in patients)
            {
                model.Add(new PatientIndexViewModel
                {
                    Id = patient.Id,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Email = patient.Email,
                    PhoneNumber = patient.PhoneNumber,
                    DoctorName = string.Join(" ",
                    patient.DoctorsPatients
                    .Select(dp => dp.Doctor.FirstName + " " + dp.Doctor.LastName))
                });
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PatientCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                foreach (var item in model.DosctorIds)
                {
                    patient.DoctorsPatients.Add(new DoctorPatient
                    {
                        DoctorId = item
                    });
                }
                ps.AddPatientAsync(patient);
                return RedirectToAction(nameof(Index));
            }
            await LoadDoctors(model.DosctorIds);
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await ps.GetPatientWithDoctorsAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            var model = new PatientEditViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DosctorIds = patient.DoctorsPatients.Select(x => x.DoctorId).ToList()
            };
            await LoadDoctors(model.DosctorIds);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PatientEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var p = await ps.GetPatientWithDoctorsAsync(id);
                if (p == null)
                {
                    return NotFound();
                }
                p.FirstName = model.FirstName;
                p.LastName = model.LastName;
                p.Email = model.Email;
                p.PhoneNumber = model.PhoneNumber;
                p.DoctorsPatients.Clear();
                foreach (var item in model.DosctorIds)
                {
                    p.DoctorsPatients.Add(new DoctorPatient
                    {
                        DoctorId = item,
                        PatientId = p.Id
                    });
                }
                await ps.UpdatePatientAsync(p, model.DosctorIds);
                return RedirectToAction(nameof(Index));
            }
            await LoadDoctors();
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var patient = await ps.GetPatientWithDoctorsAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            var model = new PatientDetailsViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DoctorName = string.Join(" ",
                    patient.DoctorsPatients
                    .Select(dp => dp.Doctor.FirstName + " " + dp.Doctor.LastName))
            };
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            await LoadDoctors();
            return View();
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ps.DeletePatientAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var p = await ps.GetPatientByIdAsync(id);
            if (p == null)
            {
                return NotFound();
            }

            var model = new PatientDeleteViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,

            };
            return View(model);
        }

        private async Task LoadDoctors(List<int> selectedD = null)
        {
            var doctors = await ds.GetAllDoctorsAsync();
            var doctori = doctors
             .Select(d => new
             {
                 d.Id,
                 FullName = d.FirstName + " " + d.LastName
             }).ToList();
            ViewBag.Docotors = new MultiSelectList(
            doctori,
            "Id",
            "FullName",
            selectedD
            );
        }
    }
}
