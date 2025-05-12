using PatientRestApi.DAL.Repository.Interfaces.Common.Models;
using PatientRestApi.DAL.Repository.Interfaces.Patients.Models;

namespace PatientRestApi.DAL.Repository.Interfaces.Patients.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> CreatePatient(Patient patient);
        Task CreateRangePatient(IEnumerable<Patient> patients);
        Task<bool> Delete(Guid id);
        Task<Patient> GetPatientById(Guid id);
        Task<IEnumerable<Patient>> GetPatientCollection(DateFilter dateFilter);
        Task<Patient> UpdatePatient(Guid id, Patient patient);
    }
}
