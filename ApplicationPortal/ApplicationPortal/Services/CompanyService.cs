using ApplicationPortal.Data;
using ApplicationPortal.Data.Entities;
using ApplicationPortal.Enums;
using ApplicationPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationPortal.Services
{
    public class CompanyService
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CompanyViewModel>> GetCompanies()
        {
            var companies = await _context.Companies.Select(q => new CompanyViewModel()
            {
                CompanyId = q.CompanyId,
                Address = q.Address,
                CompanyName = q.CompanyName,
                BusinessId = q.BusinessId,
            }).ToListAsync();

            return companies;
        }

        public async Task<CompanyViewModel> AddNewCompany(CompanyViewModel newCompany)
        {
            var company = new Company()
            {
                CompanyId = newCompany.CompanyId,
                CompanyName = newCompany.CompanyName,
                Address = newCompany.Address,
                BusinessId = newCompany.BusinessId
            };

            await _context.AddAsync(company);
            await _context.SaveChangesAsync();
            return FullCompanyMap(company);
        }

        public CompanyViewModel FullCompanyMap(Company company)
        {
            return new CompanyViewModel()
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                Address = company.Address,
                BusinessId = company.BusinessId
            };
        }

        public async Task DeleteCompany(int companyId)
        {
            var companyForDelete = await _context.Companies.FindAsync(companyId);
            _context.Companies.Remove(companyForDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<CompanyViewModel> EditCompany(CompanyViewModel model)
        {
            var companyForEdit = await _context.Companies.FindAsync(model.CompanyId);

            companyForEdit.CompanyId = model.CompanyId;
            companyForEdit.CompanyName = model.CompanyName;
            companyForEdit.Address = model.Address;
            companyForEdit.BusinessId = model.BusinessId;

            await _context.SaveChangesAsync();
            return FullCompanyMap(companyForEdit);
        }

        public async Task<CompanyViewModel> GetCompanyById(int companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            return FullCompanyMap(company);
        }
    }
}
