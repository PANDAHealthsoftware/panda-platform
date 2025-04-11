using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PANDA.Api.Infrastructure;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Clinician;

namespace PANDA.Api.Services.Clinician
{
    public class ClinicianService : IClinicianService
    {
        private readonly PandaDbContext _context;
        private readonly IMapper _mapper;

        public ClinicianService(PandaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClinicianDto>> GetAllAsync()
        {
            var clinicians = await _context.Clinicians.ToListAsync();
            return _mapper.Map<List<ClinicianDto>>(clinicians);
        }

        public async Task<ClinicianDto?> GetByIdAsync(int id)
        {
            var clinician = await _context.Clinicians.FindAsync(id);
            return clinician == null ? null : _mapper.Map<ClinicianDto>(clinician);
        }

        public async Task<ClinicianDto> CreateAsync(CreateClinicianDto dto)
        {
            var entity = _mapper.Map<Models.Clinician>(dto);
            _context.Clinicians.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ClinicianDto>(entity);
        }
    }
}