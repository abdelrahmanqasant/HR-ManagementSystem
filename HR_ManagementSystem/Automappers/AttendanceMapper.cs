using AutoMapper;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Automappers
{
    public class AttendanceMapper :Profile
    {
        public AttendanceMapper()
        {
            CreateMap<Attendence, AttendenceDTO>()
              .ForMember(dest => dest.EmpName,
                  opt => opt.MapFrom(src => src.Employee.FullName))
              .ForMember(dest => dest.DeptName,
                  opt => opt.MapFrom(src => src.Employee.Department.Name));


            CreateMap<AttendenceDTO, Attendence>()
                .ForMember(dest => dest.Employee, opt => opt.Ignore());
        }
    }
}
