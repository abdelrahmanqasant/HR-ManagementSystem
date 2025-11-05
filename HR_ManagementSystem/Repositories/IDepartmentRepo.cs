using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Repositories
{
    public interface IDepartmentRepo
    {
        public List<Department> GetAll();
        public Department GetByName(string name);
    }
}
