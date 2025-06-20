using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.Domain.Helpers;
using BDOExamAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace BDOExamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositDto dto)
        {
            try
            {
                var validationResult = TransactionValidator.ValidateDeposit(dto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }

                dto.CreatedBy = "admintest";//assuming from claims
                var result = await _transactionService.DepositAsync(dto);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (TransactionException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured ");
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawDto dto)
        {
            try
            {
                var validationResult = TransactionValidator.ValidateWidrawal(dto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }

                dto.CreatedBy = "admintest";//assuming from claims
                var result = await _transactionService.WithdrawAsync(dto);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (TransactionException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred.");
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDto dto)
        {
            try
            {
                var validationResult = TransactionValidator.ValidateTransfer(dto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }

                dto.CreatedBy = "admintest";//assuming from claims
                var result = await _transactionService.TransferAsync(dto); 
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (TransactionException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred.");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var validationResult = TransactionValidator.ValidateId(id);
               
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }

                var result = await _transactionService.GetTransactionByIdAsync(id);
                return Ok(result);

            }
            catch (TransactionException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred.");
            }


        }

        [HttpGet("{accountNumber}/transactions")]
        public async Task<IActionResult> GetTransactionHistory(long accountNumber)
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
                var transactions = await _transactionService.GetTransactionsByAccountNumberAsync(accountNumber);

                if (transactions.Count == 0)
                    return NotFound("No data found.");


                return Ok(transactions);
            }
            catch (TransactionException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {//add error logging
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred."  );
            }
        }

        [HttpPost("accountstatement")]
        public async Task<IActionResult> GetAccountStatement([FromBody] AccountStatementDto dto)
        {
            try
            {
                var validationResult = TransactionValidator.ValidateAccountStatement(dto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }
                var transactions = await _transactionService.GetAccountStatementAsync(dto);
                if (transactions.Count == 0)
                    return NotFound("No data found.");
                
                
                return Ok(transactions);
            }
            catch (TransactionException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {//add error logging
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred." );
            }
        }

        [HttpPost("searchtransaction")]
        public async Task<IActionResult> SearchTransaction([FromBody] TransactionSearchDto dto)
        {
            try
            {
                var validationResult = TransactionValidator.ValidateSearchTransaction(dto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                    });
                }
                var transactions = await _transactionService.GetTransactionSearchResultAsync(dto);
                if (transactions.Count==0)
                    return NotFound("No data found.");


                return Ok(transactions);
            }
            catch (TransactionException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {//add error logging
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred." + ex.Message);
            }
        }
    }
}
