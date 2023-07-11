using HealthCareApi.Utils;
using HealthCareAPI.DTOs;

namespace HealthCareAPI.Interfaces
{
    public interface IPatientService
    {
        Task<PaginationHelper<PatientDto>> GetPatients(int pageNumber, string searchQuery = null);
    }
}
