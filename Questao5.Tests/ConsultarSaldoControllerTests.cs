using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Services.Controllers;

namespace Questao5.Tests
{
    public class ConsultarSaldoControllerTests
    {
        private readonly IMediator _mediator;
        private readonly ContaCorrenteController _controller;

        public ConsultarSaldoControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new ContaCorrenteController(_mediator);
        }

        [Fact]
        public async Task ConsultarSaldo_ShouldReturnOk_WhenQueryIsValid()
        {
            // Arrange
            var idContaCorrente = Guid.NewGuid();
            var query = new ConsultarSaldoContaCorrenteQuery { IdContaCorrente = idContaCorrente.ToString() };
            var response = new SaldoContaCorrenteResponse { NumeroContaCorrente = 001, NomeTitular = "Teste Silva", Saldo = 1000.50m, DataConsulta = DateTime.Now };
            _mediator.Send(Arg.Any<ConsultarSaldoContaCorrenteQuery>()).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.ConsultarSaldo(idContaCorrente);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<SaldoContaCorrenteResponse>(okResult.Value);
            Assert.Equal(001, resultValue.NumeroContaCorrente);
            Assert.Equal("Teste Silva", resultValue.NomeTitular);
        }

        [Fact]
        public async Task ConsultarSaldo_ShouldReturnBadRequest_WhenBusinessExceptionIsThrown()
        {
            // Arrange
            var idContaCorrente = Guid.NewGuid();
            _mediator.Send(Arg.Any<ConsultarSaldoContaCorrenteQuery>()).Throws(new BusinessException("Conta não encontrada", "INVALID_ACCOUNT"));

            // Act
            var result = await _controller.ConsultarSaldo(idContaCorrente);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Contains("Conta não encontrada", badRequestResult.Value.ToString());
            Assert.Contains("INVALID_ACCOUNT", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task ConsultarSaldo_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var idContaCorrente = Guid.NewGuid();
            _mediator.Send(Arg.Any<ConsultarSaldoContaCorrenteQuery>()).Throws(new Exception("Erro inesperado"));

            // Act
            var result = await _controller.ConsultarSaldo(idContaCorrente);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Erro inesperado", statusCodeResult.Value);
        }
    }
}
