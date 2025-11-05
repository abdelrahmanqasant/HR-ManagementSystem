using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Repositories
{
    public interface IEmployeeRepo
    {
        public List<Employee> GetAll();
        public Employee GetById(int id);
        public void Add(Employee employee);
        public void Update(int id, Employee employee);
        public void Delete(int id);
    }
}
