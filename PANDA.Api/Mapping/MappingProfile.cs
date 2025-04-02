using AutoMapper;
using PANDA.Api.Dto;
using PANDA.Api.Models;

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

        CreateMap<Appointment, AppointmentDto>();  // Make sure this line is here
        CreateMap<AppointmentDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())  // Avoid changing the ID
            .ForMember(dest => dest.MissedTimestamp, opt => opt.Ignore());  // Ignore MissedTimestamp
    }
}