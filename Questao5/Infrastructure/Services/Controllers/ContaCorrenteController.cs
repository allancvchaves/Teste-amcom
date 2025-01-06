using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Exceptions;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> MovimentarContaCorrente([FromBody] MovimentarContaCorrenteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _mediator.Send(request);

                return Ok(new { result.Mensagem, result.IdMovimentacao });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { ex.Message, ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("saldo/{idContaCorrente}")]
        public async Task<IActionResult> ConsultarSaldo(Guid idContaCorrente)
        {
            try
            {
                var query = new ConsultarSaldoContaCorrenteQuery { IdContaCorrente = idContaCorrente.ToString() };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { ex.Message, ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
