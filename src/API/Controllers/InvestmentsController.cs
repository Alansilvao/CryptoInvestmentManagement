using Application.Dtos.Requests.Investments;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace API.Controllers;

[ExcludeFromCodeCoverage]
[Authorize]
[Route("api/v1/investments")]
public class InvestmentsController : ControllerBase
{
	private readonly IBuyAssetUseCase _buyAssetUseCase;
	private readonly ISellAssetUseCase _sellAssetUseCase;
	private readonly IGetPortfolioUseCase _getPortfolioUseCase;
    private readonly ILogger<InvestmentsController> _logger;

    public InvestmentsController(IBuyAssetUseCase buyAssetUse, ISellAssetUseCase sellAssetUse, 
								IGetPortfolioUseCase getPortfolioUseCase, ILogger<InvestmentsController> logger)
	{
		_buyAssetUseCase = buyAssetUse;
		_sellAssetUseCase = sellAssetUse;
		_getPortfolioUseCase = getPortfolioUseCase;
        _logger = logger;
    }

	[HttpPost]
	[Route("buy")]
	public async Task<IActionResult> BuyAssets([FromBody] BuyAssetRequest request)
	{
		try
		{
            _logger.LogInformation($"{DateTime.Now} | Executando o método BuyAssetRequest");

            if (ModelState.IsValid is false)
				return BadRequest(ModelState);

			var authorizationHeader = Request.Headers["Authorization"].ToString();
			var token = authorizationHeader["Bearer ".Length..].Trim();

			var output = await _buyAssetUseCase.ExecuteAsync(request, token);

            _logger.LogInformation($"{DateTime.Now} | Compra Realizada com Sucesso");
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

	[HttpPost]
	[Route("sell")]
	public async Task<IActionResult> SellAssets([FromBody] SellAssetRequest request)
	{
		try
		{
            _logger.LogInformation($"{DateTime.Now} | Executando o método SellAssetRequest");

            if (ModelState.IsValid is false)
                return BadRequest(ModelState);

            var authorizationHeader = Request.Headers["Authorization"].ToString();
			var token = authorizationHeader["Bearer ".Length..].Trim();

			var output = await _sellAssetUseCase.ExecuteAsync(request, token);

            _logger.LogInformation($"{DateTime.Now} | Venda Realizada com Sucesso");
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

	[HttpGet]
	[Route("portfolio")]
	public async Task<IActionResult> GetPortfolio()
	{
		try
		{
            _logger.LogInformation($"{DateTime.Now} | Executando o método SignInRequest");

            var authorizationHeader = Request.Headers["Authorization"].ToString();
			var token = authorizationHeader["Bearer ".Length..].Trim();

			var output = await _getPortfolioUseCase.ExecuteAsync(new GetPortfolioRequest(), token);

            _logger.LogInformation($"{DateTime.Now} | Portfólio Consultado com Sucesso");
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
}
