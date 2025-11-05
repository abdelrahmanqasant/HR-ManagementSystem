using AutoMapper;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Automappers
{
    public class WeeklyDaysOffMapper:Profile
    {
        public WeeklyDaysOffMapper()
        {
            CreateMap<WeeklyDaysOff,WeeklyDaysDTO>().ReverseMap();
        }
    }
}
