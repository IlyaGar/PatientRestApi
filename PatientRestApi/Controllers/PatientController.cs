using Microsoft.AspNetCore.Mvc;
using PatientRestApi.BLL.Service.Interfaces.Patients.Interfaces;
using PatientRestApi.BLL.Service.Interfaces.Patients.Models;

namespace PatientRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Get Patient Collection
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPatientCollection([FromQuery] List<string> birthdate)
        {
            return Ok(await _patientService.GetPatientCollection(birthdate));
        }

        /// <summary>
        /// Get Patient by Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(Guid id)
        {
            var patient = await _patientService.GetPatient(id);

            return patient is null ? NotFound() : Ok(patient);
        }

        /// <summary>
        /// Create Patient
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePatient(PatientDto patient)
        {
            return Ok(await _patientService.CreatePatient(patient));
        }

        /// <summary>
        /// Create Patient
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, PatientDto patient)
        {
            var updated = await _patientService.UpdatePatient(id, patient);

            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>
        /// Delete Patient
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var deleted = await _patientService.Delete(id);

            return deleted ? NoContent() : NotFound();
        }

        /// <summary>
        /// Create Range Patient
        /// </summary>
        [HttpPost("create-range")]
        public async Task<IActionResult> CreateRangePatient(IEnumerable<PatientDto> patients)
        {
            await _patientService.CreateRangePatient(patients);
            return Ok(new { message = "Patients added successfully." });
        }
    }
}
