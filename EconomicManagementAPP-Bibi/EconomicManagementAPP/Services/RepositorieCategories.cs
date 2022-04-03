using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieCategories
    {
        Task Create(Categories categories); 
        Task<IEnumerable<Categories>> getCategories(int UserId);
        Task Modify(Categories categories);
        Task<Categories> getCategoriesById(int id, int UserId); 
        Task Delete(int id);
        Task<IEnumerable<Categories>> getCategories(int UserId, OperationTypes operationTypesId);
    }

    public class RepositorieCategories : IRepositorieCategories
    {
        private readonly string connectionString;

        public RepositorieCategories(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public async Task Create(Categories categories)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categories 
                                                (Name, OperationTypeId, UserId) 
                                                VALUES (@Name, @OperationTypeId, @UserId); 
                                                SELECT SCOPE_IDENTITY();", categories);
            categories.Id = id;
        }

       

  
        public async Task<IEnumerable<Categories>> getCategories(int UserId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categories>(@"SELECT *
                                                            FROM Categories
                                                            WHERE UserId = @UserId", new { UserId });
        }

        public async Task<IEnumerable<Categories>> getCategories(int UserId, OperationTypes operationTypesId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categories>(@"SELECT *
                                                            FROM Categories
                                                            WHERE UserId = @UserId AND OperationTypeId = @OperationTypesId", 
                                                            new { UserId, operationTypesId });
        }




        public async Task Modify(Categories categories)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Categories
                                            SET Name = @Name, OperationTypeId = @OperationTypeID
                                            WHERE Id = @Id", categories);
        }

        public async Task<Categories> getCategoriesById(int id, int UserId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categories>(@"
                                                                SELECT *
                                                                FROM Categories
                                                                WHERE Id = @Id AND UserId = @UserId",
                                                                new { id, UserId });
        }

        //Eliminar
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE FROM Categories WHERE Id = @Id", new { id });
        }
    }
}
