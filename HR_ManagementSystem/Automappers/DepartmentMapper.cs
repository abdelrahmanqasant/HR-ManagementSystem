using AutoMapper;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Automappers
{
    public class DepartmentMapper:Profile
    {
        public DepartmentMapper()
        {
            CreateMap<Department, DepartmentDTO>().ReverseMap();
        }
    }
}
