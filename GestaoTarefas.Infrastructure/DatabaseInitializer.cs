using Dapper;
using Npgsql;
using System.Data;

namespace GestaoTarefas.Infrastructure
{
  public class DatabaseInitializer
  {
    private readonly string _connectionString;
    private readonly string _databaseName;

    public DatabaseInitializer(string connectionString)
    {
      _connectionString = connectionString;
      _databaseName = GetDatabaseNameFromConnectionString(connectionString);
    }

    private string GetDatabaseNameFromConnectionString(string connectionString)
    {
      var builder = new NpgsqlConnectionStringBuilder(connectionString);
      return builder.Database;
    }

    private string GetMasterConnectionString()
    {
      var builder = new NpgsqlConnectionStringBuilder(_connectionString)
      {
        Database = "postgres" // Usamos o banco de dados padrão do PostgreSQL para as operações administrativas
      };
      return builder.ConnectionString;
    }

    private IDbConnection CreateConnection(string connectionString)
    {
      return new NpgsqlConnection(connectionString);
    }

    public async Task InitializeAsync()
    {
      // Verifica se o banco de dados existe e o cria se necessário
      await CreateDatabaseIfNotExistsAsync();

      using (var connection = CreateConnection(_connectionString))
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
              CREATE TABLE IF NOT EXISTS public.Tarefas
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

    private async Task CreateDatabaseIfNotExistsAsync()
    {
      var masterConnectionString = GetMasterConnectionString();

      using (var connection = CreateConnection(masterConnectionString))
      {
        connection.Open();

        var databaseExistsQuery = $"SELECT 1 FROM pg_database WHERE datname = '{_databaseName}'";
        var databaseExists = await connection.ExecuteScalarAsync<bool>(databaseExistsQuery);

        if (!databaseExists)
        {
          var createDatabaseQuery = $"CREATE DATABASE \"{_databaseName}\"";
          await connection.ExecuteAsync(createDatabaseQuery);
        }
      }
    }
  }
}
