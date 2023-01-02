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
            if (ModelState.IsValid)
            {
                await _companyService.AddNewCompany(company);
                return RedirectToAction("GetAvailableCompanies");
            }

            return View(company);
        }

        public async Task<IActionResult> DeleteCompany(int companyId)
        {
            await _companyService.DeleteCompany(companyId);
            return RedirectToAction("GetAvailableCompanies");
        }

        [HttpGet]
        public async Task<IActionResult> EditCompany(int companyId)
        {
            if (ModelState.IsValid)
            {
                var product = await _companyService.GetCompanyById(companyId);
                return View(product);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditCompany(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var company = await _companyService.EditCompany(model);
                return RedirectToAction("ViewCompanyPerId", new { companyId = company.CompanyId });
            }
            return View(model);
        }

        public async Task<IActionResult> ViewCompanyPerId(int companyId)
        {
            if (ModelState.IsValid)
            {
                var companyById = await _companyService.GetCompanyById(companyId);
                return View(companyById);
            }

            return View();
        }

    }
}
