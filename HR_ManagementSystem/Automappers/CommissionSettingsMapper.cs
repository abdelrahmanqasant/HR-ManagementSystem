using AutoMapper;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Automappers
{
    public class CommissionSettingsMapper :Profile
    {
        public CommissionSettingsMapper()
        {
            CreateMap<CommissionSettings, CommissionDTO>().ReverseMap();
        }
    }
}
