using ApplicationPortal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationPortal.Services;
using ApplicationPortal.Models;
using ApplicationPortal.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace ApplicationPortal.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        //delete, edit creation of companies{

        public async Task<IActionResult> GetAvailableCompanies()
        {
            var companies = await _companyService.GetCompanies();
            return View(companies);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyViewModel company)
        {
            if(ModelState.IsValid)
            {
                await _companyService.AddNewCompany(company);
                return RedirectToAction("GetAvailableCompanies");
            }

            return View(company);
        }

        public async Task DeleteCompany(int companyId)
        {
                await _companyService.DeleteCompany(companyId);
        }

    }
}
