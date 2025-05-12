using System.ComponentModel.DataAnnotations;

namespace PatientRestApi.BLL.Service.Interfaces.Patients.Models
{
    public class PatientNameDto
    {
        public Guid? Id { get; set; }
        public string? Use { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна.")]
        public string Family { get; set; }
        public List<string>? Given { get; set; }
    }
}
