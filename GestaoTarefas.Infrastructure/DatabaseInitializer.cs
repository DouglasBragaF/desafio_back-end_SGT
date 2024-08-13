using Dapper;
using Npgsql;
using System.Data;

namespace GestaoTarefas.Infrastructure
{
  public class DatabaseInitializer
  {
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
      _connectionString = connectionString;
    }

    private IDbConnection CreateConnection()
    {
      return new NpgsqlConnection(_connectionString);
    }

    public async Task InitializeAsync()
    {
      using (var connection = CreateConnection())
      {
        connection.Open();

        // Verifica se a tabela existe
        var tableExistsQuery = @"
                    SELECT EXISTS (
                        SELECT FROM information_schema.tables 
                        WHERE table_schema = 'public' 
                        AND table_name = 'Tarefas'
                    )";

        var tableExists = await connection.ExecuteScalarAsync<bool>(tableExistsQuery);

        if (!tableExists)
        {
          // Cria a tabela se não existir
          var createTableQuery = @"
                        CREATE TABLE public.Tarefas
                        (
                            Id SERIAL PRIMARY KEY,
                            Titulo VARCHAR(255) NOT NULL,
                            Descricao TEXT NOT NULL,
                            DataCriacao TIMESTAMP WITHOUT TIME ZONE NOT NULL,
                            DataVencimento TIMESTAMP WITHOUT TIME ZONE,
                            Status INTEGER NOT NULL,
                            DataAlteracao TIMESTAMP WITHOUT TIME ZONE
                        );";

          await connection.ExecuteAsync(createTableQuery);
        }
      }
    }
  }
}