using BDOExamAPI.Application.Exceptions;
using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.Application.Services;
using BDOExamAPI.Domain.Entities;
using BDOExamAPI.Domain.Helpers;
using BDOExamAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BDOExamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Assuming it has JWT Auth 
    //Assuming has error logging
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
        {//last work here. debug error
            try
            {
                var validationResult = AccountValidator.ValidateFields(dto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }
                dto.ProccessBy = "admintest";//assuming from claims
                var accountNum = await _accountService.CreateAccountAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Account successfully created.",
                    CustomerId = dto.CustomerId,
                    AccountNumber = accountNum
                });
            }
            catch (AccountException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }
        }

        [HttpGet("{accountNumber}")]
        public async Task<IActionResult> GetAccountDetails(long accountNumber)
        {
            try
            {

                var validationResult = AccountValidator.ValidateAccountNumber(accountNumber);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }


                var result = await _accountService.GetAccountDetailsAsync(accountNumber);

                if (result == null)
                    return NotFound("Account not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }
        }

        [HttpGet("{accountNumber}/balance")]
        public async Task<IActionResult> GetAccountBalance(long accountNumber)
        {
            try
            {
                var validationResult = AccountValidator.ValidateAccountNumber(accountNumber);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }

                var result = await _accountService.GetAccountBalanceAsync(accountNumber);

                if (result.Balance == null)
                    return NotFound("Account not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }
        }

        [HttpPut("{accountNumber}/status")]
        public async Task<IActionResult> UpdateStatus(long accountNumber, [FromBody] UpdateAccountStatusDto dto)
        {
            try
            {
                var validationResult = AccountValidator.ValidateAccountNumber(accountNumber);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }

                var success = await _accountService.UpdateAccountStatusAsync(accountNumber, dto);
                if (!success)
                    return NotFound(new { Message = "Account not found." });

                return Ok(new { Message = "Account status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }
        }

    }
}
