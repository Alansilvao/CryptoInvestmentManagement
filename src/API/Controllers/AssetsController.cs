using Application.Dtos.Requests.Assets;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace API.Controllers;

[ExcludeFromCodeCoverage]
[Route("api/v1/assets")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AssetsController : ControllerBase
{
    private readonly IGetAllAssetsUseCase _getAllAssetsUseCase;
    private readonly IGetAssetBySymbolUseCase _getAssetBySymbolUseCase;
    private readonly IPostAssetsUseCase _postAssetsUseCase;
    private readonly IPutAssetsUseCase _putAssetsUseCase;
    private readonly IDeleteAssetsUseCase _deleteAssetsUseCase;
    private readonly ILogger<AssetsController> _logger;

    public AssetsController(
        IGetAllAssetsUseCase getAllAssetsUseCase, 
        IGetAssetBySymbolUseCase getAssetBySymbolUseCase, 
        IPostAssetsUseCase postAssetsUseCase,
        IPutAssetsUseCase putAssetsRequest,
        IDeleteAssetsUseCase deleteAssetsRequest,
        ILogger<AssetsController> logger)
    {
        _getAllAssetsUseCase = getAllAssetsUseCase;
        _getAssetBySymbolUseCase = getAssetBySymbolUseCase;
        _postAssetsUseCase = postAssetsUseCase;
        _putAssetsUseCase = putAssetsRequest;
        _deleteAssetsUseCase = deleteAssetsRequest;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAssets()
    {
        try
        {
            _logger.LogInformation($"{DateTime.Now} | Executando o método GetAllAssets");
            var output = await _getAllAssetsUseCase.ExecuteAsync(new GetAllAssetsRequest());
            _logger.LogInformation($"{DateTime.Now} | Consulta Realizada com Sucesso");
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

    [HttpGet("{symbol}")]
    public async Task<IActionResult> GetAssetBySymbol(string symbol)
    {
        try
        {
            _logger.LogInformation($"{DateTime.Now} | Executando o método GetAssetBySymbol");
            var output = await _getAssetBySymbolUseCase.ExecuteAsync(symbol);
            _logger.LogInformation($"{DateTime.Now} | Consulta por Symbol Realizada com Sucesso");
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

    [HttpPost()]
    public async Task<IActionResult> PostAssets([FromBody] PostAssetsRequest request)
    {
        try
        {
            _logger.LogInformation($"{DateTime.Now} | Executando o método PostAssetsRequest");

            if (ModelState.IsValid is false)
                return BadRequest(ModelState);

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader["Bearer ".Length..].Trim();

            var output = await _postAssetsUseCase.ExecuteAsync(request, token);
            _logger.LogInformation($"{DateTime.Now} | Assets Criado com Sucesso");
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

    [HttpPut()]
    public async Task<IActionResult> PutAssets([FromBody] PutAssetsRequest request)
    {
        try
        {
            _logger.LogInformation($"{DateTime.Now} | Executando o método PutAssetsRequest");

            if (ModelState.IsValid is false)
                return BadRequest(ModelState);

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader["Bearer ".Length..].Trim();

            var output = await _putAssetsUseCase.ExecuteAsync(request, token);
            _logger.LogInformation($"{DateTime.Now} | Assets Atualizado com Sucesso");
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssets([FromBody] DeleteAssetsRequest request)
    {
        try
        {
            _logger.LogInformation($"{DateTime.Now} | Executando o método DeleteAssetsRequest");
            if (ModelState.IsValid is false)
                return BadRequest(ModelState);

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader["Bearer ".Length..].Trim();

            var output = await _deleteAssetsUseCase.ExecuteAsync(request, token);
            _logger.LogInformation($"{DateTime.Now} | Assets Removido com Sucesso");
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