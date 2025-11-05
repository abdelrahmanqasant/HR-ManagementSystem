using AutoMapper;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Automappers
{
    public class DeductionSettingsMapper :Profile
    {
        public DeductionSettingsMapper()
        {
            CreateMap<DeductionSettings, DeductionDTO>().ReverseMap();
        }
    }
}
