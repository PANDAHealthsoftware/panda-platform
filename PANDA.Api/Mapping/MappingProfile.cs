using AutoMapper;
using PANDA.Api.Dto;
using PANDA.Api.Models;
using PANDA.Shared.DTOs;

namespace PANDA.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Patient, PatientDto>();
        CreateMap<CreatePatientDto, Patient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); 
        CreateMap<PatientDto, Patient>(); 

        CreateMap<CreateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MissedTimestamp, opt => opt.Ignore());

        CreateMap<Appointment, UpdateAppointmentDto>()
            .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate)); 
        CreateMap<UpdateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MissedTimestamp, opt => opt.Ignore());
    }
}