using PatientRestApi.BLL.Service.Interfaces.Patients.Interfaces;
using PatientRestApi.BLL.Service.Interfaces.Patients.Models;
using PatientRestApi.DAL.Repository.Interfaces.Common.Models;
using PatientRestApi.DAL.Repository.Interfaces.Patients.Interfaces;
using PatientRestApi.DAL.Repository.Interfaces.Patients.Models;

namespace PatientRestApi.BLL.Service.Patients
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository) 
        {
            _patientRepository = patientRepository;
        }

        public async Task<PatientDto> CreatePatient(PatientDto patientDto)
        {
            var created = await _patientRepository.CreatePatient(ConvertToPatient(patientDto));

            var createdDto = ConvertToPatientDto(created);

            return createdDto;
        }

        public async Task CreateRangePatient(IEnumerable<PatientDto> patientDtos)
        {
            var patients = patientDtos.Select(ConvertToPatient);
            await _patientRepository.CreateRangePatient(patients);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _patientRepository.Delete(id);
        }

        public async Task<PatientDto> GetPatient(Guid id)
        {
            var patient = await _patientRepository.GetPatientById(id);
            if (patient == null)
            {
                return null;
            }

            var patientDto = ConvertToPatientDto(patient);

            return patientDto;
        }

        public async Task<IEnumerable<PatientDto>> GetPatientCollection(List<string> birthdate)
        {
            var dateFilter = ParseDateFilter(birthdate);
            var patients = await _patientRepository.GetPatientCollection(dateFilter);
            var patientDtos = patients.Select(ConvertToPatientDto);

            return patientDtos;
        }

        public async Task<PatientDto> UpdatePatient(Guid id, PatientDto patientDto)
        {
            var existingPatient = await _patientRepository.GetPatientById(id);
            if (existingPatient == null)
            {
                return null;
            }

            var updatedPatient = await _patientRepository.UpdatePatient(id, ConvertToPatient(patientDto));

            var updatedDto = ConvertToPatientDto(updatedPatient);

            return updatedDto;

        }

        private static Patient ConvertToPatient(PatientDto source)
        {
            return new Patient
            {
                Name = new PatientName
                {
                    Family = source.Name.Family,
                    Given = source.Name.Given?.Select(g => new GivenName { Given = g }).ToList() ?? new List<GivenName>(),
                    Use = source.Name.Use,
                },
                Gender = source.Gender,
                BirthDate = source.BirthDate.GetValueOrDefault(),
                Active = source.Active,
            };
        }

        private static PatientDto ConvertToPatientDto(Patient source)
        {
            return new PatientDto
            {
                Name = new PatientNameDto
                {
                    Id = source.Name.Id,
                    Family = source.Name.Family,
                    Given = source.Name.Given?.Select(g => g.Given).ToList(),
                    Use = source.Name.Use,
                },
                Gender = source.Gender,
                BirthDate = source.BirthDate
            };
        }

        private static DateFilter ParseDateFilter(List<string> birthdates)
        {
            if (birthdates.Count > 2)
            {
                throw new ArgumentException("Поиск по датам не может содержать больше двух дат.");
            }

            var dateFilter = new DateFilter();

            foreach (var birthdate in birthdates)
            {
                var filterType = birthdate.Substring(0, 2);
                var dateStr = birthdate.Substring(2);

                DateTime date = DateTime.Parse(dateStr);

                if (dateFilter.StartDate is null)
                {
                    dateFilter.StartDate = date;
                    dateFilter.StartDateFilterType = GetDateFilterType(filterType);
                }
                else
                {
                    dateFilter.EndDate = date;
                    dateFilter.EndDateFilterType = GetDateFilterType(filterType);
                }
            }

            return dateFilter;
        }

        private static DateFilterType GetDateFilterType(string filterType)
        {
            return filterType switch
            {
                "ge" => DateFilterType.GreaterThanOrEqual,
                "le" => DateFilterType.LessThanOrEqual,
                "eq" => DateFilterType.Equal,
                "ne" => DateFilterType.NotEqual,
                "lt" => DateFilterType.LessThan,
                "gt" => DateFilterType.GreaterThan,
                "sa" => DateFilterType.StartsAfter,
                "eb" => DateFilterType.EndsBefore,
                "ap" => DateFilterType.Approximate,
                _ => throw new ArgumentException("Invalid filter type")
            };
        }
    }
}
