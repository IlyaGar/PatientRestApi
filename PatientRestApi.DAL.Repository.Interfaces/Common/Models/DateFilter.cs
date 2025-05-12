namespace PatientRestApi.DAL.Repository.Interfaces.Common.Models
{
    public class DateFilter
    {
        public DateFilterType StartDateFilterType { get; set; }
        public DateFilterType EndDateFilterType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
