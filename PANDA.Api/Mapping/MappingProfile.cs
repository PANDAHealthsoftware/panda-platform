using AutoMapper;
using PANDA.Api.Models;
using PANDA.Domain.Entities;
using PANDA.Domain.ValueObjects;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Appointment;
using PANDA.Shared.DTOs.Clinician;
using PANDA.Shared.DTOs.Patient;

namespace PANDA.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ---------------------------
        // Patient ↔ DTO
        // ---------------------------

        CreateMap<Patient, PatientDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName));

        CreateMap<CreatePatientDto, Patient>()
            .ConstructUsing(dto => new Patient(
                new FullName(dto.FirstName, dto.LastName),
                dto.DateOfBirth,
                dto.Gender,
                dto.NHSNumber,
                dto.Postcode
            ))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

        CreateMap<UpdatePatientDto, Patient>()
            .ForAllMembers(opt => opt.Ignore());

        CreateMap<Patient, UpdatePatientDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName));

        // ---------------------------
        // Appointment ↔ DTO
        // ---------------------------

        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src => src.Patient.Name.ToString()))
            .ForMember(dest => dest.ClinicianFullName, opt => opt.MapFrom(src => src.Clinician.FullName))
            .ForMember(dest => dest.ClinicianId, opt => opt.MapFrom(src => src.ClinicianId));

        CreateMap<CreateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MissedTimestamp, opt => opt.Ignore())
            .ForMember(dest => dest.Clinician, opt => opt.Ignore())
            .ForMember(dest => dest.Patient, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

        CreateMap<UpdateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MissedTimestamp, opt => opt.Ignore())
            .ForMember(dest => dest.Clinician, opt => opt.Ignore())
            .ForMember(dest => dest.Patient, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

        CreateMap<Appointment, UpdateAppointmentDto>();

        // ---------------------------
        // Clinician ↔ DTO
        // ---------------------------

        CreateMap<Clinician, ClinicianDto>()
            .ReverseMap()
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