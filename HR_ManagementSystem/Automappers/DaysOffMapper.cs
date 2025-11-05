using AutoMapper;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Automappers
{
    public class DaysOffMapper:Profile
    {
        public DaysOffMapper()
        {
            CreateMap<DaysOff, DaysOffDTO>().ReverseMap();
        }
    }
}
