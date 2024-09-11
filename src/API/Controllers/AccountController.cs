using Application.Dtos.Requests.Accounts;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace API.Controllers;

[ExcludeFromCodeCoverage]
[Route("api/v1/account")]
[Authorize]
public class AccountController : ControllerBase
{
	private readonly IDepositUseCase _depositUseCase;
	private readonly IWithdrawUseCase _withdrawUseCase;
	private readonly IGetAccountBalanceUseCase _getAccountBalanceUseCase;
	private readonly IGetTransactionsUseCase _getTransactionsUseCase;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
		IDepositUseCase depositUseCase, IWithdrawUseCase withdrawUseCase,
		IGetAccountBalanceUseCase getAccountBalanceUseCase,
		IGetTransactionsUseCase getTransactionsUseCase,
        ILogger<AccountController> logger
	)
	{
		_depositUseCase = depositUseCase;
		_withdrawUseCase = withdrawUseCase;
		_getAccountBalanceUseCase = getAccountBalanceUseCase;
		_getTransactionsUseCase = getTransactionsUseCase;
        _logger = logger;
    }

	[HttpGet("balance")]
	public async Task<IActionResult> GetAccountBalance()
	{
		try
		{
            _logger.LogInformation($"{DateTime.Now} | Executando o método GetAccountBalance");

            if (ModelState.IsValid is false)
				return BadRequest(ModelState);

			var authorizationHeader = Request.Headers["Authorization"].ToString();
			var token = authorizationHeader["Bearer ".Length..].Trim();

			var output = await _getAccountBalanceUseCase.ExecuteAsync(new GetBalanceRequest(), token);
            _logger.LogInformation($"{DateTime.Now} | Realizado consulta de saldo na conta com sucesso");
            return Ok(output);
		}
		catch (HttpStatusException ex)
		{
            _logger.LogError($"{DateTime.Now} | {ex.StatusCode}: {ex.Message}");
            return StatusCode(ex.StatusCode, new { ex.Message });
		}
		catch (Exception ex)
		{
            _logger.LogError($"{DateTime.Now} | {ex.Message} - {ex.StackTrace}");
            return StatusCode
			(
				StatusCodes.Status500InternalServerError, new { Message = "Internal Server Error" }
			);
		}
	}

	[HttpPost("deposit")]
	public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
	{
		try
		{
            _logger.LogInformation($"{DateTime.Now} | Executando o método DepositRequest");

            if (ModelState.IsValid is false)
				return BadRequest(ModelState);

			var authorizationHeader = Request.Headers["Authorization"].ToString();
			var token = authorizationHeader["Bearer ".Length..].Trim();

			var output = await _depositUseCase.ExecuteAsync(request, token);
            _logger.LogInformation($"{DateTime.Now} | Depósito Realizado com Sucesso");
            return Ok(output);
		}
		catch (HttpStatusException ex)
		{
            _logger.LogError($"{DateTime.Now} | {ex.StatusCode}: {ex.Message}");
            return StatusCode(ex.StatusCode, new { ex.Message });
		}
		catch (Exception ex)
		{
            _logger.LogError($"{DateTime.Now} | {ex.Message} - {ex.StackTrace}");
            return StatusCode
			(
				StatusCodes.Status500InternalServerError, new { Message = "Internal Server Error" }
			);
		}
	}

	[HttpPost("withdraw")]
	public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
	{
		try
		{
            _logger.LogInformation($"{DateTime.Now} | Executando o método WithdrawRequest");

            if (ModelState.IsValid is false)
                return BadRequest(ModelState);

            var authorizationHeader = Request.Headers["Authorization"].ToString();
			var token = authorizationHeader["Bearer ".Length..].Trim();

			var output = await _withdrawUseCase.ExecuteAsync(request, token);
            _logger.LogInformation($"{DateTime.Now} | Retirada Realizada com Sucesso");
            return Ok(output);
		}
		catch (HttpStatusException ex)
		{
            _logger.LogError($"{DateTime.Now} | {ex.StatusCode}: {ex.Message}");
            return StatusCode(ex.StatusCode, new { ex.Message });
		}
		catch (Exception ex)
		{
            _logger.LogError($"{DateTime.Now} | {ex.Message} - {ex.StackTrace}");
            return StatusCode
			(
				StatusCodes.Status500InternalServerError, new { Message = "Internal Server Error" }
			);
		}
	}

	[HttpGet("transactions")]
	public async Task<IActionResult> GetTransactionsExtract()
	{
		try
		{
            _logger.LogInformation($"{DateTime.Now} | Executando o método GetTransactionsExtract");
            var authorizationHeader = Request.Headers["Authorization"].ToString();
			var token = authorizationHeader["Bearer ".Length..].Trim();

			var output = await _getTransactionsUseCase.ExecuteAsync(new GetTransactionsExtractRequest(), token);
            _logger.LogInformation($"{DateTime.Now} | Extrato Bancário Retornado com Sucesso");
            return output is null ? BadRequest() : Ok(output);
		}
		catch (HttpStatusException ex)
		{
            _logger.LogError($"{DateTime.Now} | {ex.StatusCode}: {ex.Message}");
            return StatusCode(ex.StatusCode, new { ex.Message });
		}
		catch (Exception ex)
		{
            _logger.LogError($"{DateTime.Now} | {ex.Message} - {ex.StackTrace}");
            return StatusCode
			(
				StatusCodes.Status500InternalServerError, new { Message = "Internal Server Error" }
			);
		}
	}
}