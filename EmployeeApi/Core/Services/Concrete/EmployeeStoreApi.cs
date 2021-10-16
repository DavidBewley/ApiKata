using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Core.Services.Concrete
{
    public class EmployeeStoreApi : IEmployeeStore
    {
        private SqliteConnection _databaseConnection;
        private const string DatabaseName = "LocalDb";
        private const string TableName = "Employees";

        public EmployeeStoreApi()
        {
            OpenDatabaseConnection();

            var table = _databaseConnection.Query<string>($"SELECT name FROM sqlite_master WHERE type='table' AND name = '{TableName}';");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && tableName == TableName)
                return;

            _databaseConnection.Execute($"Create Table [{TableName}] (" +
                                       "[Id] uniqueidentifier NOT NULL," +
                                       "[Name] VARCHAR(100) NOT NULL," +
                                       "[StartDate] Date NOT NULL" +
                                       ");");
        }

        public async Task<Employee> FindEmployee(Guid id)
        {
            OpenDatabaseConnection();

            return (await _databaseConnection.QueryAsync<Employee>($"SELECT TOP 1 [Id],[Name],[StartDate] FROM [{TableName}] WHERE [Id] = '{id}'")).FirstOrDefault();
        }

        public async Task CreateEmployee(Employee employee)
        {
            OpenDatabaseConnection();
            await _databaseConnection.ExecuteAsync($"INSERT INTO [{TableName}] ([Id], [Name], [StartDate])" + "VALUES (@Id, @Name, @StartDate);", employee);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            OpenDatabaseConnection();
            await _databaseConnection.ExecuteAsync($"UPDATE [{TableName}] SET [Name] = @Name, [StartDate] = @StartDate WHERE [Id] = @Id", employee);
        }

        public async Task DeleteEmployee(Guid id)
        {
            OpenDatabaseConnection();
            await _databaseConnection.ExecuteAsync($"DELETE FROM [{TableName}] WHERE [Id] = @id", id);
        }

        private void OpenDatabaseConnection() 
            => _databaseConnection = new SqliteConnection(DatabaseName);
    }
}
