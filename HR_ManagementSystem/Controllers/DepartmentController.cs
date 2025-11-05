using AutoMapper;
using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perm = HR_ManagementSystem.Utilities.Permission;
namespace HR_ManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [Authorize(Perm.Department.View)]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Department> departments = _unitOfWork.departmentRepo.GetAll();
            if(departments.Count == 0)
                return NotFound();
            List<DepartmentDTO> departmentDTOs = new();
            foreach(var dept in departments)
            {
                DepartmentDTO deptDTO = _mapper.Map<DepartmentDTO>(dept);
                departmentDTOs.Add(deptDTO);
            }
            return Ok(departmentDTOs);
        }
        [Authorize(Perm.Department.View)]
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
                Department department = _unitOfWork.departmentRepo.GetByName(name);
            if(department == null)
                return NotFound();
            DepartmentDTO departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return Ok(departmentDTO);
        }
    }
}
