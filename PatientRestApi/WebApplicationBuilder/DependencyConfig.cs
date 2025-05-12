using PatientRestApi.BLL.Service.Interfaces.Patients.Interfaces;
using PatientRestApi.BLL.Service.Patients;
using PatientRestApi.DAL.Repository.Interfaces.Patients.Interfaces;
using PatientRestApi.DAL.Repository.Patients;

namespace PatientRestApi.WebApplicationBuilder
{
    internal static class DependencyConfig
    {
        internal static void ConfigureServices(IServiceCollection services)
        {
            ConfigureBllServices(services);
            ConfigureDalRepository(services);
        }

        private static void ConfigureBllServices(IServiceCollection services)
        {
            services.AddScoped<IPatientService, PatientService>();

        }

        private static void ConfigureDalRepository(IServiceCollection services)
        {
            
            services.AddScoped<IPatientRepository, PatientRepository>();
        }
    }
}
