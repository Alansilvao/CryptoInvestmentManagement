using Application.Dtos.Requests.Clients;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace API.Controllers;

[ExcludeFromCodeCoverage]
[Route("api/v1")]
public class ClientsController : ControllerBase
{
	private readonly ISignInUseCase _signInUseCase;
	private readonly ISignUpUseCase _signUpUseCase;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(ISignInUseCase signInUseCase, 
							ISignUpUseCase signUpUseCase,
                            ILogger<ClientsController> logger)
	{
		_signInUseCase = signInUseCase;
		_signUpUseCase = signUpUseCase;
        _logger = logger;
    }

	[HttpPost]
	[Route("token")]
	public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
	{
		try
		{
			_logger.LogInformation($"{DateTime.Now} | Executando o método SignInRequest");

			if (ModelState.IsValid is false)
				return BadRequest(ModelState);

            
            var output = await _signInUseCase.ExecuteAsync(request);
            _logger.LogInformation($"{DateTime.Now} | Token Obtido com Sucesso");
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
	[Route("register")]
	public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
	{
		try
		{
            _logger.LogInformation($"{DateTime.Now} | Executando o método SignUpRequest");

            if (ModelState.IsValid is false)
				return BadRequest(ModelState);

			await _signUpUseCase.ExecuteAsync(request);
            _logger.LogInformation($"{DateTime.Now} | Conta Cadastrada com Sucesso");
            return Created("Account created", null);
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