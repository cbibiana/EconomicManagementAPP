using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieUsers
    {
        Task<int> CreateUsers(Users users);
        Task<Users> getUserByEmail(string standarEmail);
    }

    public class RepositorieUsers : IRepositorieUsers
    {
        private readonly string connectionString;

        public RepositorieUsers(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
     
        public async Task<int> CreateUsers(Users users)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Users 
                                                (Email, StandarEmail, Password) 
                                                VALUES (@Email, @StandarEmail, @Password);
                                                 SELECT SCOPE_IDENTITY();", users);

            return id;
        }

        public async Task<Users> getUserByEmail(string standarEmail)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(@"
                                                                SELECT *
                                                                FROM Users
                                                                WHERE StandarEmail = @StandarEmail ",
                                                                new { standarEmail });
        }

    }
}
