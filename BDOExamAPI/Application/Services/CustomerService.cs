using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.Domain.Entities;
using BDOExamAPI.DTOs;
using BDOExamAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BDOExamAPI.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomersDto?> GetCustomersAsync(long customerId)
        {
            //added rule: include the active account only
            var customer = await _context.Customers
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                return null;

            return new CustomersDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone= customer.Phone,
                Accounts = customer.Accounts
                    .Where(a => a.IsActive)
                    .Select(a => new AccountsDto
                    {
                        CustomerId = a.Id,
                        AccountNumber = a.AccountNumber,
                        AccountType = a.AccountType,
                        Balance = Math.Round(a.Balance, 2)
                    }).ToList()
            };
        }

        public async Task<long> CreateCustomerAsync(CreateCustomerDto dto)
        {
            long customerId = 0;
            var customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                CreatedBy = dto.ProccessBy,
                CreatedDate = DateTime.Now,
                ModifiedBy = dto.ProccessBy,
                ModifiedDate = DateTime.Now
            };

            _context.Customers.Add(customer);
            var result = await _context.SaveChangesAsync();
            customerId = customer.Id;
            return customerId;
        }

        public async Task<bool> UpdateCustomerAsync(long id, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return false;

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.ModifiedBy = dto.ProccessBy;
            customer.ModifiedDate = DateTime.Now;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

    }
}
