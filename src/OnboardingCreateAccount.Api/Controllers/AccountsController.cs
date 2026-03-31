using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnboardingCreateAccount.Application.Commands;
using OnboardingCreateAccount.Application.DTOs;
using OnboardingCreateAccount.Application.Queries;

namespace OnboardingCreateAccount.Controllers;

[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<AccountResponse>> Create([FromBody] CreateAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountResponse>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetAccountByIdQuery(id));
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AccountResponse>> Update(Guid id, [FromBody] UpdateAccountCommand command)
    {
        if (id != command.Id) return BadRequest("ID divergente.");
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAccountCommand(id));
        return NoContent();
    }
}