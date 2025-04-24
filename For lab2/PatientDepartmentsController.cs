using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HospitalApplication.Domain.DomainModels;
using HospitalApplication.Service.Interface;
using System.Security.Claims;

namespace HospitalApplication.Web.Controllers
{
    public class PatientDepartmentsController : Controller
    {
        private readonly IPatientDepartmentService _patientDepartmentService;
        private readonly IDepartmentService _departmentsService;
        private readonly IPatientService _patientService;

        public PatientDepartmentsController(
            IPatientDepartmentService patientDepartmentService,
            IDepartmentService departmentsService,
            IPatientService patientService)
        {
            _patientDepartmentService = patientDepartmentService;
            _departmentsService = departmentsService;
            _patientService = patientService;
        }

        public IActionResult Create(Guid patientId)
        {
            ViewBag.PatientId = patientId;
            ViewBag.DepartmentId = new SelectList(_departmentsService.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,PatientId,DepartmentId,DateAssigned")] PatientDepartment patientDepartment)
        {
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                patientDepartment.Id = Guid.NewGuid();
                _patientDepartmentService.AddPatientToDepartment(patientDepartment.PatientId, patientDepartment.DepartmentId, userId);
                return RedirectToAction("Index", "Patients");
            }
        }
    }
}
