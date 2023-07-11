using HealthCareAPI.DTOs;
using HealthCareAPI.Interfaces;
using HealthCareAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetPatients(int pageNumber, string searchQuery = null)
        {
            var paginationHelper = await _patientService.GetPatients(pageNumber, searchQuery);
            if (paginationHelper.Items.Any())
            {
                var morePagesAvailable = paginationHelper.PageNumber < paginationHelper.TotalPages;

                var result = new
                {
                    data = paginationHelper.Items,
                    more = morePagesAvailable
                };

                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
