using PatientRestApi.DAL.Repository.Interfaces.Common.Models;

namespace PatientRestApi.DAL.Repository.Interfaces.Patients.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public PatientName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
