using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieTransactions
    {
        Task Create(Transactions transactions);
        Task Delete(int id);
        Task<IEnumerable<Transactions>> getTransactions(int userId);
        Task<Transactions> getTransactionsById(int id, int userId);
        Task Modify(Transactions transactions, decimal totalPrevious, int accountPreviousId);
    }

    public class RepositorieTransactions : IRepositorieTransactions
    {
        private readonly string connectionString;

        public RepositorieTransactions(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Transactions transactions)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("Transactions_Insertar",
                new
                {
                  transactions.UserId,
                  transactions.TransactionDate,
                  transactions.Total,
                  transactions.CategoryId,
                  transactions.AccountId,
                  transactions.Description
                },
                commandType: System.Data.CommandType.StoredProcedure);
                transactions.Id = id;
        }


        public async Task Modify(Transactions transactions, decimal totalPrevious, int accountPreviousId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transactions_Modify",
             new
             {
                 transactions.Id,
                 transactions.TransactionDate,
                 transactions.Total,
                 transactions.CategoryId,
                 transactions.AccountId,
                 transactions.Description,
                 totalPrevious,
                 accountPreviousId

             }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Transactions> getTransactionsById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transactions>(
                @"SELECT  Transactions.*, cat.OperationTypeId
                FROM Transactions
                INNER JOIN Categories cat
                ON cat.Id = Transactions.CategoryId
                WHERE Transactions.Id = @Id AND Transactions.UserId = @UserId",
                new { id, userId });
        }
        
        public  async Task<IEnumerable<Transactions>> getTransactions( int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transactions>(
                @"SELECT  Transactions.*, cat.OperationTypeId
                FROM Transactions
                INNER JOIN Categories cat
                ON cat.Id = Transactions.CategoryId
                WHERE Transactions.UserId = @UserId",
                new {  userId });
        }


        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transactions_Delete",
                new {id}, commandType: System.Data.CommandType.StoredProcedure);
        }

    }
}
