using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;

namespace HR_ManagementSystem.Implementations
{
    public class DepartmentRepo : IDepartmentRepo
    {
        private HRDbContext _dbContext;
        public DepartmentRepo(HRDbContext context)
        {
            _dbContext = context;   
        }
        public List<Department> GetAll()
        {
            return _dbContext.Departments.ToList();
        }

        public Department GetByName(string name)
        {
            return _dbContext.Departments.SingleOrDefault(d => d.Name == name);
        }
    }
}
