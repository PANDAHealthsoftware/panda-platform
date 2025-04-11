using AutoMapper;
using PANDA.Api.Models;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Appointment;
using PANDA.Shared.DTOs.Clinician;

namespace PANDA.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>();
            CreateMap<CreatePatientDto, Patient>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<PatientDto, Patient>();

            // Map DTO → Appointment (creation)
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MissedTimestamp, opt => opt.Ignore())
                .ForMember(dest => dest.Clinician, opt => opt.Ignore())
                .ForSourceMember(src => src.PatientName, opt => opt.DoNotValidate()); 

            // Map Appointment → DTO (for display)
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.Clinician, opt => opt.MapFrom(src => src.Clinician))
                .ForMember(dest => dest.ClinicianId, opt => opt.MapFrom(src => src.ClinicianId)).ReverseMap();

            CreateMap<Appointment, UpdateAppointmentDto>()
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate)).ReverseMap();

            // Map DTO → Appointment (update)
            CreateMap<UpdateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MissedTimestamp, opt => opt.Ignore())
                .ForMember(dest => dest.Clinician, opt => opt.Ignore())
                .ForSourceMember(src => src.PatientName, opt => opt.DoNotValidate());


            CreateMap<Clinician, ClinicianDto>();
            CreateMap<ClinicianDto, Clinician>();

            CreateMap<CreateClinicianDto, Clinician>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}