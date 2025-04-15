using AutoMapper;
using PANDA.Api.Models;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Appointment;
using PANDA.Shared.DTOs.Clinician;
using PANDA.Shared.DTOs.Patient;

namespace PANDA.Api.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Patient ↔ DTO
        CreateMap<Patient, PatientDto>();
        CreateMap<PatientDto, Patient>()
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

        CreateMap<CreatePatientDto, Patient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

        // Appointment ↔ DTO
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.Clinician, opt => opt.MapFrom(src => src.Clinician))
            .ForMember(dest => dest.ClinicianId, opt => opt.MapFrom(src => src.ClinicianId))
            .ReverseMap()
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

        CreateMap<CreateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MissedTimestamp, opt => opt.Ignore())
            .ForMember(dest => dest.Clinician, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore())
            .ForSourceMember(src => src.PatientName, opt => opt.DoNotValidate());

        CreateMap<UpdateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MissedTimestamp, opt => opt.Ignore())
            .ForMember(dest => dest.Clinician, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore())
            .ForSourceMember(src => src.PatientName, opt => opt.DoNotValidate());

        // Clinician ↔ DTO
        CreateMap<Clinician, ClinicianDto>().ReverseMap()
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

        CreateMap<CreateClinicianDto, Clinician>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

        CreateMap<UpdateClinicianDto, Clinician>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());
    }
}
