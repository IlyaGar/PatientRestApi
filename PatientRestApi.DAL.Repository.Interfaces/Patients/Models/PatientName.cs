namespace PatientRestApi.DAL.Repository.Interfaces.Patients.Models
{
    public class PatientName
    {
        public Guid Id { get; set; }
        public string Use { get; set; }
        public string Family { get; set; }

        public ICollection<GivenName> Given { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
