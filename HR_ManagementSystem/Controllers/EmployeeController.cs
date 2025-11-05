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
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        
        public EmployeeController(IUnitOfWork unitOfWork, IAuthorizationService authorizationService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }
       [Authorize(Perm.Employee.View)]
        [HttpGet]
        public  IActionResult GetAll ()
        {
        List<Employee> employees = _unitOfWork.employeeRepo.GetAll();
            if (employees == null || employees.Count == 0)
                return NotFound();
            List<EmployeeDTO> employeeDTOs = new();
            foreach (Employee employee in employees)
            {
                EmployeeDTO  employeeDTO = _mapper.Map<EmployeeDTO>(employee);
                employeeDTOs.Add(employeeDTO);
            }
            return Ok(employeeDTOs);
        }
      [Authorize(Perm.Employee.View)]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Employee employee = _unitOfWork.employeeRepo.GetById(id);
            if(employee == null)
                return NotFound();
            EmployeeDTO employeeDTO = _mapper.Map<EmployeeDTO>(employee);
            return Ok(employeeDTO);
        }
       [Authorize(Perm.Employee.Create)]
        [HttpPost]
        public async Task<IActionResult> Add(EmployeeDTO employeeDTO)
        {
            if(employeeDTO == null)
                return BadRequest("Invalid Employee Data");
            var department = _unitOfWork.departmentRepo.GetByName(employeeDTO.DepartmentName);
            if (department == null)
                return NotFound("Department Not Found");
                Employee employee = _mapper.Map<Employee>(employeeDTO);
            employee.DeptId = department.Id;
            _unitOfWork.employeeRepo.Add(employee);
           await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employeeDTO);

        }
       [Authorize(Perm.Employee.Edit)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id ,  EmployeeDTO employeeDTO)
        {
            Employee existingEmployee = _unitOfWork.employeeRepo.GetById(id);
            if (existingEmployee == null)
                return NotFound($"Employee with ID {id} not found");
            Department department = _unitOfWork.departmentRepo.GetByName(employeeDTO.DepartmentName);
            if (department == null)
                return NotFound("Department Not Found");
            existingEmployee.FullName = employeeDTO.FullName;
            existingEmployee.Address = employeeDTO.Address;
            existingEmployee.Arrival = employeeDTO.Arrival;
            existingEmployee.Departure = employeeDTO.Departure;
            existingEmployee.Gender = employeeDTO.Gender;
            existingEmployee.PhoneNumber = employeeDTO.PhoneNumber;
            existingEmployee.BaseSalary = employeeDTO.BaseSalary;
            existingEmployee.BirthDate = employeeDTO.BirthDate;
            existingEmployee.ContractDate = employeeDTO.ContractDate;
            existingEmployee.Nationality = employeeDTO.Nationality;
            existingEmployee.DeptId = department.Id  ;
            existingEmployee.SSN = employeeDTO.SSN;
            _unitOfWork.employeeRepo.Update(id, existingEmployee);
          await  _unitOfWork.SaveChangesAsync();
            return Ok();

        }
       [Authorize(Perm.Employee.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            Employee employee =_unitOfWork.employeeRepo.GetById(id);
            if (employee == null) return NotFound();
            _unitOfWork.employeeRepo.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
