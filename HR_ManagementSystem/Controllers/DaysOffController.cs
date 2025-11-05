using AutoMapper;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;
using HR_ManagementSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Perm = HR_ManagementSystem.Utilities.Permission;

namespace HR_ManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DaysOffController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DaysOffController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


       [Authorize(Perm.DaysOff.View)]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<DaysOff> daysOffs = _unitOfWork.daysOffRepo.GetAll();
            if(daysOffs.Count == 0)
            return NotFound();
            List<DaysOffDTO> daysOffDTOs = new();
            foreach(var day in daysOffs)
            {
                DaysOffDTO daysOffDTO = _mapper.Map<DaysOffDTO>(day);
                daysOffDTOs.Add(daysOffDTO);
            }
            return Ok(daysOffDTOs);
        }
      [Authorize(Perm.DaysOff.View)]
        [HttpGet("{day}")]
        public IActionResult GetByDay(DateOnly day)
        {
            DaysOff daysOff = _unitOfWork.daysOffRepo.GetByDay(day);
            if(daysOff == null)
                return NotFound();
            DaysOffDTO daysOffDTO = _mapper.Map<DaysOffDTO>(daysOff);
            return Ok(daysOffDTO);
        }
   [Authorize(Perm.DaysOff.Create)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DaysOffDTO daysOffDto) 
        {
        if(_unitOfWork.daysOffRepo.GetByDay(daysOffDto.Date) != null)
                return BadRequest("This Day Is Already Exist!");
            DaysOff newDayOff = _mapper.Map<DaysOff>(daysOffDto);
            _unitOfWork.daysOffRepo.Add(newDayOff);
            await _unitOfWork.SaveChangesAsync();
            return Ok(newDayOff);
        }
    [Authorize(Perm.DaysOff.Edit)]
        [HttpPost("{day}")]
        public async Task<IActionResult> Update(DateOnly day , [FromBody] DaysOffDTO daysOff)
        {
            DaysOff existingDayOff = _unitOfWork.daysOffRepo.GetByDay(day);
            if (existingDayOff == null)
            {
                return NotFound();
            }
            existingDayOff.Name = daysOff.Name;
            _unitOfWork.daysOffRepo.Update(day, existingDayOff);
               await _unitOfWork.SaveChangesAsync();
            return Ok(daysOff);
        }
    [Authorize(Perm.DaysOff.Delete)]
        [HttpDelete("{day}")]
        public async Task<IActionResult> Delete (DateOnly day)
        {
            DaysOff existingDayOff = _unitOfWork.daysOffRepo.GetByDay(day);
            if (existingDayOff == null)
            {
                return NotFound();
            }
            _unitOfWork.daysOffRepo.Delete(day);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
