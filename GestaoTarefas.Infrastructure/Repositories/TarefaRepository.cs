using Dapper;
using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Interfaces;
using Npgsql;
using System.Data;

namespace GestaoTarefas.Infrastructure.Repositories
{
  public class TarefaRepository : ITarefaRepository
  {
    private readonly string _connectionString;

    public TarefaRepository(string connectionString)
    {
      _connectionString = connectionString;
    }

    private IDbConnection CreateConnection()
    {
      return new NpgsqlConnection(_connectionString);
    }

    public async Task<Tarefa?> GetByIdAsync(int id)
    {
      using (var connection = CreateConnection())
      {
        var sql = "SELECT * FROM Tarefas WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<Tarefa>(sql, new { Id = id });
      }
    }

    public async Task<IEnumerable<Tarefa>> GetAllAsync()
    {
      using (var connection = CreateConnection())
      {
        var sql = "SELECT * FROM Tarefas";
        return await connection.QueryAsync<Tarefa>(sql);
      }
    }

    public async Task<int> AddAsync(Tarefa tarefa)
    {
      using (var connection = CreateConnection())
      {
        tarefa.DataCriacao = DateTime.UtcNow; // Define a data de criação

        var sql = @"
                    INSERT INTO Tarefas (Titulo, Descricao, DataCriacao, DataVencimento, Status)
                    VALUES (@Titulo, @Descricao, @DataCriacao, @DataVencimento, @Status)
                    RETURNING Id";

        var id = await connection.ExecuteScalarAsync<int>(sql, tarefa);
        return id;
      }
    }

    public async Task UpdateAsync(Tarefa tarefa)
    {
      using (var connection = CreateConnection())
      {
        tarefa.DataAlteracao = DateTime.UtcNow; // Define a data de alteração

        var sql = @"
                    UPDATE Tarefas
                    SET Titulo = @Titulo,
                        Descricao = @Descricao,
                        DataVencimento = @DataVencimento,
                        Status = @Status,
                        DataAlteracao = @DataAlteracao
                    WHERE Id = @Id";

        await connection.ExecuteAsync(sql, tarefa);
      }
    }

    public async Task DeleteAsync(int id)
    {
      using (var connection = CreateConnection())
      {
        var sql = "DELETE FROM Tarefas WHERE Id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
      }
    }
  }
}