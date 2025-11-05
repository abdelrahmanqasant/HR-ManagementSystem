using AutoMapper;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Extensions;
using HR_ManagementSystem.Helpers;
using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;
using HR_ManagementSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Perm = HR_ManagementSystem.Utilities.Permission;

namespace HR_ManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AttendenceController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Attendence
        [Authorize(Perm.Attendence.View)]
        [HttpGet]
        public async Task<ActionResult<PagedList<AttendenceDTO>>> GetAll([FromQuery] UserParams userParams)
        {
            var attendences = await _unitOfWork.Attendence.GetAll(userParams);
            if (attendences.Count == 0)
                return NotFound("Attendance list is empty");

            Response.AddPaginationHeader(new PaginationHeader(attendences.CurrentPage, attendences.TotalPages, attendences.PageSize, attendences.TotalCount));
            return Ok(attendences);
        }

        // POST: api/Attendence
        [Authorize(Perm.Attendence.Create)]
        [HttpPost]
        public async Task<ActionResult> Add(AttendenceDTO attendenceDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           
            var day = DateOnly.Parse(attendenceDTO.Day);
            var arrival = !string.IsNullOrEmpty(attendenceDTO.Arrival) ? TimeOnly.Parse(attendenceDTO.Arrival) : (TimeOnly?)null;
            var departure = !string.IsNullOrEmpty(attendenceDTO.Departure) ? TimeOnly.Parse(attendenceDTO.Departure) : (TimeOnly?)null;

            var employee = _unitOfWork.employeeRepo.GetById(attendenceDTO.EmpId);
            if (employee == null)
                return BadRequest("Employee not found");

            DaysOff daysOff = _unitOfWork.daysOffRepo.GetByDay(day);
            WeeklyDaysOff weeklyDays = _unitOfWork.weeklyDaysOffRepo.Get();
            if (daysOff != null)
                return BadRequest("This is an official day off");

            int dayIndex = (int)day.DayOfWeek;
            if (weeklyDays != null && weeklyDays.Days.Contains((DaysName)dayIndex))
                return BadRequest("This is a weekly day off");

            if (_unitOfWork.Attendence.GetDayByEmpId(attendenceDTO.EmpId, day) != null)
                return BadRequest("Attendance for this employee already added!");

            int employeeDepartureHour = employee.Departure.Hour;
            int employeeArrivalHour = employee.Arrival.Hour;

            Attendence attendence = new Attendence
            {
                EmpId = attendenceDTO.EmpId,
                Day = day,
                Arrival = arrival,
                Departure = departure,
                Status = (AttendenceStatus)attendenceDTO.Status
            };

            if (attendence.Status == AttendenceStatus.Present)
            {
                attendence.OvertimeInHours = 0;
                attendence.LatetimeInHours = 0;

                if (departure != null)
                {
                    var overTime = departure.Value.Hour - employeeDepartureHour;
                    attendence.OvertimeInHours = overTime > 0 ? overTime : 0;
                    attendence.LatetimeInHours += overTime < 0 ? -overTime : 0;
                }

                if (arrival != null)
                {
                    var lateTime = arrival.Value.Hour - employeeArrivalHour;
                    attendence.LatetimeInHours += lateTime > 0 ? lateTime : 0;
                    attendence.OvertimeInHours += lateTime < 0 ? -lateTime : 0;
                }
            }

            _unitOfWork.Attendence.Add(attendence);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetDayByEmpId", new { empId = attendence.EmpId, date = attendence.Day.ToString("yyyy-MM-dd") }, attendenceDTO);
        }



        [Authorize(Perm.Attendence.Delete)]
        [HttpDelete("{empId}")]
        public async Task<IActionResult> Delete([FromRoute] int empId, [FromQuery] string date)
        {
            var day = DateOnly.Parse(date);
            Attendence attendence = _unitOfWork.Attendence.GetDayByEmpId(empId, day);
            if (attendence == null)
                return NotFound();

            _unitOfWork.Attendence.Delete(empId, day);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Perm.Attendence.View)]
        [HttpGet("GetEmployeeDay/{empId}")]
        public IActionResult GetDayByEmpId([FromRoute] int empId, [FromQuery] string day)
        {
            var date = DateOnly.Parse(day);
            Attendence? attendence = _unitOfWork.Attendence.GetDayByEmpId(empId, date);
            if (attendence == null)
                return NotFound();

            AttendenceDTO attendenceDTO = _mapper.Map<AttendenceDTO>(attendence);
            return Ok(attendenceDTO);
        }

        [Authorize(Perm.Attendence.View)]
        [HttpGet("{empId}")]
        public IActionResult GetAttendenceByEmpId(int empId)
        {
            List<AttendenceDTO> attendenceDTOs = new();
            List<Attendence> attendences = _unitOfWork.Attendence.GetAttendenceByEmpId(empId);
            if (attendences == null || attendences.Count == 0)
                return NotFound("No attendance records found for the specified employee.");

            foreach (var attendence in attendences)
            {
                AttendenceDTO dto = _mapper.Map<AttendenceDTO>(attendence);
                attendenceDTOs.Add(dto);
            }
            return Ok(attendenceDTOs);
        }

        [Authorize(Perm.Attendence.View)]
        [HttpGet("GetByPeriod")]
        public ActionResult GetByPeriod([FromQuery] Period period)
        {
            var start = DateOnly.Parse(period.Start);
            var end = DateOnly.Parse(period.End);

            List<Attendence> attendences = _unitOfWork.Attendence.GetByPeriod(start, end);
            if (attendences.Count == 0)
                return NotFound("There is no attendances.");

            List<AttendenceDTO> attendenceDTOs = new();
            foreach (var attendence in attendences)
            {
                AttendenceDTO dto = _mapper.Map<AttendenceDTO>(attendence);
                attendenceDTOs.Add(dto);
            }
            return Ok(attendenceDTOs);
        }

        public class Period
        {
            public string Start { get; set; }
            public string End { get; set; }
        }
    }
}
