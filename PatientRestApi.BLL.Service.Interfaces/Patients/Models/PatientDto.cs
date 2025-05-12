using PatientRestApi.DAL.Repository.Interfaces.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PatientRestApi.BLL.Service.Interfaces.Patients.Models
{
    public class PatientDto
    {
        public PatientNameDto Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна.")]
        public DateTime? BirthDate { get; set; }
        public bool Active { get; set; }
    }
}
