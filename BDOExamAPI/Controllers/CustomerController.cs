using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.Domain.Helpers;
using BDOExamAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BDOExamAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        // Assuming it has JWT Auth 
        //Assuming has error loging middle ware

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
        {
            try
            {
                var validationResult = CustomerValidator.ValidateFields(dto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }

                //assuming getting the username from jwt token claims
                dto.ProccessBy = "admintest";

                var customerId = await _customerService.CreateCustomerAsync(dto);

                if (customerId == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create customer");

                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Customer successfully created.",
                    CustomerId = customerId, 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }

        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetCustomerWithAccounts(long id)
        {
            try
            {
                var customer = await _customerService.GetCustomersAsync(id);

                if (customer == null)
                    return NotFound();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }

        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateCustomer(long id, [FromBody] UpdateCustomerDto dto)
        {
            try {
                var validationResult = CustomerValidator.ValidateFields(dto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }

                //assuming getting the username from jwt token claims
                dto.ProccessBy = "admintest";

                var result = await _customerService.UpdateCustomerAsync(id, dto);
                if (!result)
                    return NotFound();
                else
                    return StatusCode(StatusCodes.Status200OK);
            
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }
            

        }

    }
}
