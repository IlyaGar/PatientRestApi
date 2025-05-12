namespace PatientRestApi.DAL.Repository.Interfaces.Common.Models
{
    public enum DateFilterType
    {
        Equal,
        NotEqual,
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual,
        StartsAfter,
        EndsBefore,
        Approximate
    }
}
