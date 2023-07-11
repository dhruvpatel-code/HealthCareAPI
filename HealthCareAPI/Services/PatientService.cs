using HealthCareApi.Utils;
using HealthCareAPI.Data;
using HealthCareAPI.DTOs;
using HealthCareAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthCareAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PatientService> _logger;

        public PatientService(ApplicationDbContext context, ILogger<PatientService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PaginationHelper<PatientDto>> GetPatients(int pageNumber, string searchQuery = null)
        {
            var pageSize = 9;
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(m => m.FirstName.Contains(searchQuery) || m.LastName.Contains(searchQuery));
            }

            var totalItems = await query.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            pageNumber = Math.Clamp(pageNumber, 1, totalPages);

            var skipAmount = (pageNumber - 1) * pageSize;

            var patients = await query
                .Skip(skipAmount)
                .Take(pageSize)
                .Select(m => new PatientDto
                {
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    DateOfBirth = m.DateOfBirth,
                    Address = m.Address,
                    Gender = m.Gender,
                    PhoneNumber = m.PhoneNumber,
                    Email = m.Email,
                    Note = m.Note,
                })
                .ToListAsync();

            return new PaginationHelper<PatientDto>(patients, pageNumber, pageSize, totalItems);
        }


    }
}
