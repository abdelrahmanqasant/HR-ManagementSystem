using AutoMapper;
using HR_ManagementSystem.Repositories;
using HR_ManagementSystem.Utilities;
using Perm = HR_ManagementSystem.Utilities.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HR_ManagementSystem.Models;
using System.Threading.Tasks;
using HR_ManagementSystem.DTOs;

namespace HR_ManagementSystem.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrganizationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
       [Authorize(Perm.Organization.View)]
        [HttpGet]
        public ActionResult Get()
        {
            CommissionSettings? commissionSettings = _unitOfWork.Commission.Get();
            DeductionSettings? deductionSettings = _unitOfWork.deduction.Get();
            WeeklyDaysOff? weeklyDaysOff = _unitOfWork.weeklyDaysOffRepo.Get();
            OrganizationSettings organizationSettings = new()
            {
                CommissionDTO = _mapper.Map<CommissionDTO>(commissionSettings),
                DeductionDTO = _mapper.Map<DeductionDTO>(deductionSettings),
                WeeklyDaysDTO = _mapper.Map<WeeklyDaysDTO>(weeklyDaysOff),
            };
            return Ok(organizationSettings);
        }
   [Authorize(Perm.Organization.Edit)]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] OrganizationSettings organization)
        {
            CommissionDTO commissionDTO = organization.CommissionDTO;
            DeductionDTO deductionDTO = organization.DeductionDTO;
            WeeklyDaysDTO weeklyDaysDTO = organization.WeeklyDaysDTO;

            if (commissionDTO.Amount < 0 || commissionDTO.Hours < 0)
                return BadRequest("Commission data has nagetive numbers");
            if (deductionDTO.Amount < 0 || deductionDTO.Hours < 0)
                return BadRequest("Deduction data has negative numbers");
            foreach (int day in weeklyDaysDTO.Days)
            {
                if (day < 0 || day > 6)
                    return BadRequest("Bad Day Number");
            }
            CommissionSettings oldCommission = _unitOfWork.Commission.Get();
            DeductionSettings oldDeduction = _unitOfWork.deduction.Get();
            WeeklyDaysOff oldDaysOff = _unitOfWork.weeklyDaysOffRepo.Get();
           
            CommissionSettings commission = _mapper.Map<CommissionSettings>(commissionDTO);
            DeductionSettings deduction = _mapper.Map<DeductionSettings>(deductionDTO);
            WeeklyDaysOff newWeeklyDaysOff = _mapper.Map<WeeklyDaysOff>(weeklyDaysDTO);
            if (oldCommission == null || oldDeduction == null || oldDaysOff == null)
            {
                _unitOfWork.Commission.Add(commission);
                _unitOfWork.deduction.Add(deduction);
                _unitOfWork.weeklyDaysOffRepo.Add(newWeeklyDaysOff);
            }
            else
            {
                if (oldCommission != null)
               
                {
                    oldCommission.type = (Unit)commissionDTO.type;
                    oldCommission.Hours = commissionDTO.Hours;
                    oldCommission.Amount = commissionDTO.Amount;
                    _unitOfWork.Commission.Update(oldCommission);
                }
                else
                {
                    _unitOfWork.Commission.Add(commission);
                }
                if (oldDeduction != null)
                {
                    oldDeduction.type = (Unit)deductionDTO.type;
                    oldDeduction.Hours = deductionDTO.Hours;
                    oldDeduction.Amount = deductionDTO.Amount;
                    _unitOfWork.deduction.Update(oldDeduction);
                }
                else
                    _unitOfWork.deduction.Add(deduction);
                if (oldDaysOff != null)
                {
                    oldDaysOff.Days.Clear();
                    foreach (var day in weeklyDaysDTO.Days)
                    {
                        oldDaysOff.Days.Add((DaysName)day);

                    }
                    _unitOfWork.weeklyDaysOffRepo.Update(oldDaysOff);
                }
                else
                {
                    _unitOfWork.weeklyDaysOffRepo.Add(newWeeklyDaysOff);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction("Get", organization);
        }
    }
}