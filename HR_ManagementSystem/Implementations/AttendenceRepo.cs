using AutoMapper;
using AutoMapper.QueryableExtensions;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Helpers;
using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HR_ManagementSystem.Implementations
{
    public class AttendenceRepo : IAttendence
    {
        private readonly HRDbContext _context;
        private IMapper _mapper;
        public AttendenceRepo(HRDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void Add(Attendence attendence)
        {
            _context.Attendences.Add(attendence);
        }

        public void Delete(int empId, DateOnly date)
        {
            var attendence = _context.Attendences.FirstOrDefault(e=>e.EmpId == empId && e.Day == date);
            if (attendence == null)
            {
                return;
            }
            _context.Attendences.Remove(attendence);
        }

        public async Task<PagedList<AttendenceDTO>> GetAll(UserParams userParams)
        {
            var query = _context.Attendences.AsQueryable();

           
            query = query.Where(e => e.Day >= userParams.StartDate && e.Day <= userParams.EndDate);

            
            if (!string.IsNullOrEmpty(userParams.stringQuery))
            {
                query = query.Where(e =>
                    (e.Employee.FullName != null && e.Employee.FullName.Contains(userParams.stringQuery)) ||
                    (e.Employee.Department != null && e.Employee.Department.Name != null && e.Employee.Department.Name.Contains(userParams.stringQuery))
                );
            }

          
            return await PagedList<AttendenceDTO>.CreateAsync(
                query.AsNoTracking().ProjectTo<AttendenceDTO>(_mapper.ConfigurationProvider),
                userParams.PageNumber,
                userParams.PageSize
            );
        }


        public List<Attendence> GetAttendenceByEmpId(int empId)
        {
            return _context.Attendences.Where(e=>e.EmpId ==empId).ToList(); 
        }

        public List<Attendence>? GetByPeriod(DateOnly startDate, DateOnly endDate)
        {
            List<Attendence>? attendences = _context.Attendences.Where(e=>e.Day >=startDate && e.Day <= endDate).ToList();
            return attendences;
        }

        public Attendence? GetDayByEmpId(int empId, DateOnly day)
        {
            return _context.Attendences.FirstOrDefault(e => e.EmpId == empId && e.Day == day );
        }

        public void Update(int empId, DateOnly date, Attendence attendence)
        {
            if (GetDayByEmpId(empId, date) != null)
            {
                _context.Attendences.Update(attendence);
            }
        }
    }
}
