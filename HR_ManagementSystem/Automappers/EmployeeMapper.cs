using AutoMapper;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Automappers
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper()
        {
            
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) 
                .ForMember(dest => dest.SSN, opt => opt.MapFrom(src => src.SSN))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.ContractDate, opt => opt.MapFrom(src => src.ContractDate))
                .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.BaseSalary))
                .ForMember(dest => dest.Arrival, opt => opt.MapFrom(src => src.Arrival))
                .ForMember(dest => dest.Departure, opt => opt.MapFrom(src => src.Departure));

           
            CreateMap<EmployeeDTO, Employee>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.DeptId, opt => opt.Ignore())
                .ForMember(dest => dest.SSN, opt => opt.MapFrom(src => src.SSN))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.ContractDate, opt => opt.MapFrom(src => src.ContractDate))
                .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.BaseSalary))
                .ForMember(dest => dest.Arrival, opt => opt.MapFrom(src => src.Arrival))
                .ForMember(dest => dest.Departure, opt => opt.MapFrom(src => src.Departure));
        }
    }
}