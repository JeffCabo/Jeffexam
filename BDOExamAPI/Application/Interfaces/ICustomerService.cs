using BDOExamAPI.DTOs;

namespace BDOExamAPI.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomersDto?> GetCustomersAsync(long customerId);
        Task<long> CreateCustomerAsync(CreateCustomerDto dto);
        Task<bool> UpdateCustomerAsync(long id, UpdateCustomerDto dto);
    }
}
