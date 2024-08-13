using Dapper;
using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Infrastructure.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class TarefaRepositoryTests : IAsyncLifetime
{
  private readonly string _connectionString;
  private readonly TarefaRepository _repository;

  public TarefaRepositoryTests()
  {
    // Substitua pela string de conexão do seu banco de testes
    _connectionString = "Host=localhost;Port=5432;Database=GestaoTarefasDB-teste;Username=postgres;Password=889521";
    _repository = new TarefaRepository(_connectionString);
  }

  private NpgsqlConnection CreateConnection()
  {
    return new NpgsqlConnection(_connectionString);
  }

  public async Task InitializeAsync()
  {
    using (var connection = CreateConnection())
    {
      await connection.OpenAsync();
      // Limpa a tabela antes de cada teste
      await connection.ExecuteAsync("DELETE FROM Tarefas");
    }
  }

  public Task DisposeAsync()
  {
    // Nada a limpar após os testes
    return Task.CompletedTask;
  }

  [Fact]
  public async Task AddAsync_ShouldInsertNewTarefa()
  {
    // Arrange
    var tarefa = new Tarefa
    {
      Titulo = "Teste Tarefa",
      Descricao = "Descrição da tarefa de teste",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };

    // Act
    var id = await _repository.AddAsync(tarefa);
    var tarefaInserida = await _repository.GetByIdAsync(id);

    // Assert
    Assert.NotNull(tarefaInserida);
    Assert.Equal(tarefa.Titulo, tarefaInserida.Titulo);
    Assert.Equal(tarefa.Descricao, tarefaInserida.Descricao);
    Assert.Equal(tarefa.DataVencimento.Value.Date, tarefaInserida.DataVencimento!.Value.Date);
    Assert.Equal(tarefa.Status, tarefaInserida.Status);
  }

  [Fact]
  public async Task GetAllAsync_ShouldReturnAllTarefas()
  {
    // Arrange
    var tarefa1 = new Tarefa
    {
      Titulo = "Teste Tarefa 1",
      Descricao = "Descrição da tarefa de teste 1",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };

    var tarefa2 = new Tarefa
    {
      Titulo = "Teste Tarefa 2",
      Descricao = "Descrição da tarefa de teste 2",
      DataVencimento = DateTime.UtcNow.AddDays(2),
      Status = StatusTarefa.EmProgresso
    };

    await _repository.AddAsync(tarefa1);
    await _repository.AddAsync(tarefa2);

    // Act
    var tarefas = await _repository.GetAllAsync();

    // Assert
    Assert.Equal(2, tarefas.Count());
  }

  [Fact]
  public async Task UpdateAsync_ShouldUpdateExistingTarefa()
  {
    // Arrange
    var tarefa = new Tarefa
    {
      Titulo = "Teste Tarefa",
      Descricao = "Descrição da tarefa de teste",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };

    var id = await _repository.AddAsync(tarefa);
    var tarefaInserida = await _repository.GetByIdAsync(id);

    // Act
    tarefaInserida.Titulo = "Tarefa Atualizada";
    tarefaInserida.Descricao = "Descrição Atualizada";
    tarefaInserida.Status = StatusTarefa.Concluida;
    await _repository.UpdateAsync(tarefaInserida);
    var tarefaAtualizada = await _repository.GetByIdAsync(id);

    // Assert
    Assert.NotNull(tarefaAtualizada);
    Assert.Equal("Tarefa Atualizada", tarefaAtualizada.Titulo);
    Assert.Equal("Descrição Atualizada", tarefaAtualizada.Descricao);
    Assert.Equal(StatusTarefa.Concluida, tarefaAtualizada.Status);
  }

  [Fact]
  public async Task DeleteAsync_ShouldRemoveTarefa()
  {
    // Arrange
    var tarefa = new Tarefa
    {
      Titulo = "Teste Tarefa",
      Descricao = "Descrição da tarefa de teste",
      DataVencimento = DateTime.UtcNow.AddDays(1),
      Status = StatusTarefa.Pendente
    };

    var id = await _repository.AddAsync(tarefa);

    // Act
    await _repository.DeleteAsync(id);
    var tarefaRemovida = await _repository.GetByIdAsync(id);

    // Assert
    Assert.Null(tarefaRemovida);
  }
}
