using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnboardingCreateAccount.Application.Commands;
using OnboardingCreateAccount.Application.DTOs;
using OnboardingCreateAccount.Application.Queries;

namespace OnboardingCreateAccount.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Cria uma nova conta bancária.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AccountResponse>> Create([FromBody] CreateAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Obtém os detalhes de uma conta pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountResponse>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetAccountByIdQuery(id));

        // O Middleware tratará se for null, mas aqui mantemos o padrão REST
        return result is not null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Atualiza os dados de uma conta existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<AccountResponse>> Update(Guid id, [FromBody] UpdateAccountCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Remove uma conta do sistema.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAccountCommand { Id = id });
        return NoContent();
    }
}