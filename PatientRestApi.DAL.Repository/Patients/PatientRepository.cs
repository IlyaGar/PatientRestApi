using Microsoft.EntityFrameworkCore;
using PatientRestApi.DAL.Repository.Interfaces.Common.Models;
using PatientRestApi.DAL.Repository.Interfaces.Context;
using PatientRestApi.DAL.Repository.Interfaces.Patients.Interfaces;
using PatientRestApi.DAL.Repository.Interfaces.Patients.Models;

namespace PatientRestApi.DAL.Repository.Patients
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _context;

        public PatientRepository(PatientDbContext context) 
        { 
            _context = context;
        }

        public async Task<Patient> CreatePatient(Patient patient)
        {
            patient.Name.Id = Guid.NewGuid();
            patient.CreatedDate = DateTime.Now;
            _context.Patients.Add(patient);

            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task CreateRangePatient(IEnumerable<Patient> patients)
        {
            foreach (var patient in patients)
            {
                patient.Name.Id = Guid.NewGuid();
                patient.CreatedDate = DateTime.Now;
            }

            _context.Patients.AddRange(patients);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            var patient = await _context.Patients.Include(x => x.Name).FirstOrDefaultAsync(x => x.Name.Id.Equals(id));
            if (patient == null)
            {
                return false;
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Patient?> GetPatientById(Guid id)
        {
            return await _context.Patients
                .Include(x => x.Name)
                .ThenInclude(x => x.Given)
                .FirstOrDefaultAsync(x => x.Name.Id.Equals(id));
        }

        public async Task<IEnumerable<Patient>> GetPatientCollection(DateFilter dateFilter)
        {
            var query = _context.Patients
                .Include(p => p.Name)
                    .ThenInclude(n => n.Given)
                .AsQueryable();

            if (dateFilter.StartDate != default)
            {
                query = dateFilter.StartDateFilterType switch
                {
                    DateFilterType.GreaterThan => query.Where(p => p.BirthDate > dateFilter.StartDate),
                    DateFilterType.GreaterThanOrEqual => query.Where(p => p.BirthDate >= dateFilter.StartDate),
                    DateFilterType.Equal => query.Where(p => p.BirthDate == dateFilter.StartDate),
                    DateFilterType.NotEqual => query.Where(p => p.BirthDate != dateFilter.StartDate),
                    DateFilterType.StartsAfter => query.Where(p => p.BirthDate > dateFilter.StartDate),
                    DateFilterType.EndsBefore => query.Where(p => p.BirthDate < dateFilter.StartDate),
                    DateFilterType.Approximate => query.Where(p =>
                        p.BirthDate >= dateFilter.StartDate.GetValueOrDefault().AddDays(-1) &&
                        p.BirthDate <= dateFilter.StartDate.GetValueOrDefault().AddDays(1)),
                    _ => query
                };
            }

            if (dateFilter.EndDate != default && dateFilter.EndDate != dateFilter.StartDate)
            {
                query = dateFilter.EndDateFilterType switch
                {
                    DateFilterType.LessThan => query.Where(p => p.BirthDate < dateFilter.EndDate),
                    DateFilterType.LessThanOrEqual => query.Where(p => p.BirthDate <= dateFilter.EndDate),
                    DateFilterType.Equal => query.Where(p => p.BirthDate == dateFilter.EndDate),
                    DateFilterType.NotEqual => query.Where(p => p.BirthDate != dateFilter.EndDate),
                    DateFilterType.StartsAfter => query.Where(p => p.BirthDate > dateFilter.EndDate),
                    DateFilterType.EndsBefore => query.Where(p => p.BirthDate < dateFilter.EndDate),
                    DateFilterType.Approximate => query.Where(p =>
                        p.BirthDate >= dateFilter.EndDate.GetValueOrDefault().AddDays(-1) &&
                        p.BirthDate <= dateFilter.EndDate.GetValueOrDefault().AddDays(1)),
                    _ => query
                };
            }

            return await query.ToListAsync();
        }

        public async Task<Patient> UpdatePatient(Guid id, Patient patient)
        {
            if (id != patient.Name.Id)
            {
                throw new ArgumentException("Идентификаторы не совпадают");
            }

            var existingPatient = await _context.Patients
                .Include(x => x.Name)
                    .ThenInclude(n => n.Given)
                .FirstOrDefaultAsync(x => x.Name.Id.Equals(id));

            if (existingPatient == null)
            {
                throw new KeyNotFoundException("Пациент не найден");
            }

            existingPatient.Gender = patient.Gender;
            existingPatient.BirthDate = patient.BirthDate;
            existingPatient.Active = patient.Active;
            existingPatient.UpdatedDate = DateTime.UtcNow;

            existingPatient.Name.Use = patient.Name.Use;
            existingPatient.Name.Family = patient.Name.Family;

            existingPatient.Name.Given.Clear();
            foreach (var given in patient.Name.Given)
            {
                existingPatient.Name.Given.Add(new GivenName
                {
                    Given = given.Given
                });
            }

            await _context.SaveChangesAsync();
            return existingPatient;
        }
    }
}
