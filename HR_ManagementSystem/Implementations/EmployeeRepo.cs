using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;

namespace HR_ManagementSystem.Implementations
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private HRDbContext _context;
        public EmployeeRepo(HRDbContext context)
        {
            _context = context;
        }
        public void Add(Employee employee)
        {
          _context.Employees.Add(employee);
        }

        public void Delete(int id)
        {
            _context.Employees.Remove(GetById(id));
        }

        public List<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees.SingleOrDefault(e => e.Id == id);
        }

        public void Update(int id, Employee employee)
        {
           if(GetById(id) != null)
            {
                _context.Employees.Update(employee);
            }

        }
    }
}
