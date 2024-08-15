using GestaoTarefas.Application;
using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Interfaces;
using MassTransit;
using Moq;

public class TarefaServiceTests
{
  private readonly TarefaService _tarefaService;
  private readonly Mock<ITarefaRepository> _mockRepository;
  private readonly Mock<IBus> _mockBus;

  public TarefaServiceTests()
  {
    _mockRepository = new Mock<ITarefaRepository>();
    _mockBus = new Mock<IBus>();
    _tarefaService = new TarefaService(_mockBus.Object, _mockRepository.Object);
  }

  [Fact]
  public async Task GetTarefaByIdAsync_ShouldReturnTarefa_WhenTarefaExists()
  {
    // Arrange
    var tarefa = new Tarefa
    {
      Id = 1,
      Titulo = "Teste Tarefa",
      Descricao = "Descrição de teste",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };

    _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(tarefa);

    // Act
    var result = await _tarefaService.GetTarefaByIdAsync(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(tarefa.Titulo, result?.Titulo);
    Assert.Equal(tarefa.Descricao, result?.Descricao);
    Assert.Equal(tarefa.DataVencimento, result?.DataVencimento);
    Assert.Equal(tarefa.Status, result?.Status);
  }

  [Fact]
  public async Task GetTarefaByIdAsync_ShouldReturnNull_WhenTarefaDoesNotExist()
  {
    // Arrange
    _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Tarefa?)null);

    // Act
    var result = await _tarefaService.GetTarefaByIdAsync(1);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task GetAllTarefasAsync_ShouldReturnAllTarefas()
  {
    // Arrange
    var tarefas = new List<Tarefa>
        {
            new Tarefa { Id = 1, Titulo = "Teste Tarefa 1", Descricao = "Descrição 1", DataVencimento = DateTime.UtcNow, Status = StatusTarefa.Pendente },
            new Tarefa { Id = 2, Titulo = "Teste Tarefa 2", Descricao = "Descrição 2", DataVencimento = DateTime.UtcNow.AddDays(1), Status = StatusTarefa.EmProgresso }
        };

    _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tarefas);

    // Act
    var result = await _tarefaService.GetAllTarefasAsync();

    // Assert
    Assert.NotNull(result);
    Assert.Equal(2, result.Count());
    Assert.Equal("Teste Tarefa 1", result.First().Titulo);
  }

  [Fact]
  public async Task CreateTarefaAsync_ShouldReturnNewTarefaId()
  {
    // Arrange
    var createTarefaDto = new CreateTarefaDto
    {
      Titulo = "Nova Tarefa",
      Descricao = "Descrição",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };

    _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Tarefa>())).ReturnsAsync(1);

    // Act
    var result = await _tarefaService.CreateTarefaAsync(createTarefaDto);

    // Assert
    Assert.Equal(1, result);
  }

  [Fact]
  public async Task UpdateTarefaAsync_ShouldCallRepositoryUpdate()
  {
    // Arrange
    var updateTarefaDto = new UpdateTarefaDto
    {
      Id = 1,
      Titulo = "Tarefa Atualizada",
      Descricao = "Descrição Atualizada",
      DataVencimento = DateTime.UtcNow.AddDays(2),
      Status = StatusTarefa.Concluida
    };

    var tarefaExistente = new Tarefa
    {
      Id = updateTarefaDto.Id,
      Titulo = "Tarefa Original",
      Descricao = "Descrição Original",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };

    _mockRepository.Setup(repo => repo.GetByIdAsync(updateTarefaDto.Id)).ReturnsAsync(tarefaExistente);

    // Act
    await _tarefaService.UpdateTarefaAsync(updateTarefaDto);

    // Assert
    _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Tarefa>()), Times.Once);
  }

  [Fact]
  public async Task DeleteTarefaAsync_ShouldCallRepositoryDelete()
  {
    // Arrange
    var tarefaId = 1;

    //mock
    var tarefa = new Tarefa
    {
      Id = tarefaId,
      Titulo = "Teste Tarefa",
      Descricao = "Descrição de teste",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };

    _mockRepository.Setup(repo => repo.GetByIdAsync(tarefaId)).ReturnsAsync(tarefa);


    // Act
    await _tarefaService.DeleteTarefaAsync(tarefaId);

    // Assert
    _mockRepository.Verify(repo => repo.DeleteAsync(tarefaId), Times.Once);
  }
}
