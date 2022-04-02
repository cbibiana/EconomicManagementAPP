using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieAccounts
    {
        Task Create(Accounts accounts); 
        Task<bool> Exist(string Name, int AccountTypeId);
        Task<IEnumerable<Accounts>> getAccounts(int AccountTypeId);
        Task Modify(AccountsCreateViewModel account);
        Task<Accounts> getAccountById(int id, int userId); 
        Task Delete(int id);
        Task<IEnumerable<Accounts>> Search(int userId);
        
    }

    public class RepositorieAccounts : IRepositorieAccounts
    {
        private readonly string connectionString;

        public RepositorieAccounts(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public async Task Create(Accounts accounts)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Accounts 
                                                (Name, AccountTypeId, Balance, Description) 
                                                VALUES (@Name, @AccountTypeId, @Balance, @Description); SELECT SCOPE_IDENTITY();", accounts);
            accounts.Id = id;
        }


        public async Task<IEnumerable<Accounts>> Search(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Accounts>(@"SELECT Accounts.Id, Accounts.Name, Balance, at.Name AS AccountTypes
                                                          FROM Accounts
                                                          INNER JOIN AccountTypes at
                                                          ON at.Id = Accounts.AccountTypeId
                                                          WHERE at.UserId = @UserId
                                                          ORDER BY at.OrderAccount", new { userId });
        }

        //Cuando retorna un tipo de dato se debe poner en el Task Task<bool>
        public async Task<bool> Exist(string Name, int AccountTypeId)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM Accounts
                                    WHERE Name = @Name AND AccountTypeId = @AccountTypeId;",
                                    new { Name, AccountTypeId });
            return exist == 1;
        }

        // Obtenemos las cuentas del usuario
        public async Task<IEnumerable<Accounts>> getAccounts(int UserId )
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Accounts>(@"SELECT Accounts.Id, Accounts.Name, Balance, at.Name AS AccountType
                                                         FROM Accounts
                                                         INNER JOIN AccountTypes at
                                                         ON at.Id = Accounts.AccountTypeId
                                                         WHERE at.UserId = @UserId", new { UserId});
        }

        // Actualizar
        //Obtiene el Objeto
        public async Task Modify(AccountsCreateViewModel account)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Accounts
                                            SET Name = @Name, Balance = @Balance, Description = @Description, AccountTypeId = @AccountTypeId
                                            WHERE Id = @Id", account);
        }

        //Para actualizar se necesita obtener el tipo de cuenta por el id
        //Este metodo de getAccountById nos va a regresar un AccountTypes osea todo el schema
        public async Task<Accounts> getAccountById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Accounts>(@"SELECT Accounts.Id, Accounts.Name, Balance, Description, at.Id
                                                                      FROM Accounts
                                                                      INNER JOIN AccountTypes at
                                                                      ON at.Id = Accounts.AccountTypeId
                                                                      WHERE at.UserId = @UserId AND Accounts.Id = @Id", new {id, userId });
                                                                      
        }

        //Eliminar
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE FROM Accounts WHERE Id = @Id", new { id });
        }
    }
}
