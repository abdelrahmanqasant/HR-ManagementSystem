using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;
using HR_ManagementSystem.Helpers;

namespace HR_ManagementSystem.Repositories
{
    public interface IAttendence
    {
        public Task<PagedList<AttendenceDTO>> GetAll(UserParams userParams);
        public List<Attendence>? GetByPeriod(DateOnly startDate, DateOnly endDate);
        public Attendence? GetDayByEmpId(int empId, DateOnly day);
        public void Add(Attendence attendence);
        public void Update(int empId, DateOnly date, Attendence attendence);
        public void Delete(int empId, DateOnly date);

        public List<Attendence> GetAttendenceByEmpId(int empId);
    }
}
