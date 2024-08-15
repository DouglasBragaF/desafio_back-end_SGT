using GestaoTarefas.Api.Controllers;
using GestaoTarefas.Application;
using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class TarefaControllerTests
{
  private readonly Mock<ITarefaService> _mockService;
  private readonly TarefaController _controller;

  public TarefaControllerTests()
  {
    _mockService = new Mock<ITarefaService>();
    _controller = new TarefaController(_mockService.Object);
  }

  [Fact]
  public async Task CreateTarefa_ShouldReturnCreatedAtAction_WhenTarefaIsCreated()
  {
    // Arrange
    var createTarefaDto = new CreateTarefaDto
    {
      Titulo = "Nova Tarefa",
      Descricao = "Descrição de teste",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };
    _mockService.Setup(s => s.CreateTarefaAsync(createTarefaDto)).ReturnsAsync(1);

    // Act
    var result = await _controller.CreateTarefa(createTarefaDto);

    // Assert
    var actionResult = Assert.IsType<CreatedAtActionResult>(result);
    Assert.Equal("GetTarefa", actionResult.ActionName);
    Assert.Equal(1, actionResult.RouteValues?["id"]);
  }

  [Fact]
  public async Task GetTarefaById_ShouldReturnOk_WhenTarefaExists()
  {
    // Arrange
    var tarefa = new Tarefa
    {
      Id = 1,
      Titulo = "Tarefa de Teste",
      Descricao = "Descrição de Teste",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };
    _mockService.Setup(s => s.GetTarefaByIdAsync(1)).ReturnsAsync(tarefa);

    // Act
    var result = await _controller.GetTarefa(1);

    // Assert
    var actionResult = Assert.IsType<OkObjectResult>(result);
    var model = Assert.IsType<Tarefa>(actionResult.Value);
    Assert.Equal(tarefa.Id, model.Id);
  }

  [Fact]
  public async Task GetTarefaById_ShouldReturnNotFound_WhenTarefaDoesNotExist()
  {
    // Arrange
    _mockService.Setup(s => s.GetTarefaByIdAsync(1)).ReturnsAsync((Tarefa?)null);

    // Act
    var result = await _controller.GetTarefa(1);

    // Assert
    Assert.IsType<NotFoundResult>(result);
  }

  [Fact]
  public async Task GetAllTarefas_ShouldReturnOk_WithListOfTarefas()
  {
    // Arrange
    var tarefas = new List<Tarefa>
        {
            new Tarefa { Id = 1, Titulo = "Tarefa 1", Descricao = "Descrição 1", DataVencimento = DateTime.UtcNow.AddDays(1), Status = StatusTarefa.Pendente },
            new Tarefa { Id = 2, Titulo = "Tarefa 2", Descricao = "Descrição 2", DataVencimento = DateTime.UtcNow.AddDays(2), Status = StatusTarefa.EmProgresso }
        };
    _mockService.Setup(s => s.GetAllTarefasAsync()).ReturnsAsync(tarefas);

    // Act
    var result = await _controller.GetTarefas();

    // Assert
    var actionResult = Assert.IsType<OkObjectResult>(result);
    var model = Assert.IsType<List<Tarefa>>(actionResult.Value);
    Assert.Equal(2, model.Count);
  }

  [Fact]
  public async Task UpdateTarefa_ShouldReturnNoContent_WhenUpdateIsSuccessful()
  {
    // Arrange
    var updateTarefaDto = new UpdateTarefaDto
    {
      Id = 1,
      Titulo = "Tarefa Atualizada",
      Descricao = "Descrição Atualizada",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };

    _mockService.Setup(s => s.UpdateTarefaAsync(updateTarefaDto)).Returns(Task.CompletedTask);

    // Act
    var result = await _controller.UpdateTarefa(1, updateTarefaDto);

    // Assert
    Assert.IsType<NoContentResult>(result);
  }

  [Fact]
  public async Task DeleteTarefa_ShouldReturnNoContent_WhenDeleteIsSuccessful()
  {
    // Arrange
    _mockService.Setup(s => s.DeleteTarefaAsync(1)).Returns(Task.CompletedTask);

    // Act
    var result = await _controller.DeleteTarefa(1);

    // Assert
    Assert.IsType<NoContentResult>(result);
  }
}
