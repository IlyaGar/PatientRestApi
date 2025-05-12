
using PatientRestApi.BLL.Service.Interfaces.Patients.Models;

namespace PatientRestApi.BLL.Service.Interfaces.Patients.Interfaces
{
    public interface IPatientService
    {
        Task<PatientDto> CreatePatient(PatientDto patient);
        Task CreateRangePatient(IEnumerable<PatientDto> patients);
        Task<bool> Delete(Guid id);
        Task<PatientDto> GetPatient(Guid id);
        Task<IEnumerable<PatientDto>> GetPatientCollection(List<string> birthdate);
        Task<PatientDto> UpdatePatient(Guid id, PatientDto patient);
    }
}
