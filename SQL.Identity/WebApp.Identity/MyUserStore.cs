using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Identity.Models;

namespace WebApp.Identity
{
    public class MyUserStore : IUserStore<MyUser>
    {
        // BASIC'S
        public async Task<IdentityResult> CreateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "INSERT INTO Users([Id]," +
                    "[UserName]," +
                    "[NormalizedUserName]," +
                    "[PasswordHash]) " +
                    "Values(@id, @userName, @normalizedUserName, @passwordHash)",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "UPDATE Users([Id]," +
                    "[UserName]," +
                    "[NormalizedUserName]," +
                    "[PasswordHash]) " +
                    "Values(@id, @userName, @normalizedUserName, @passwordHash)",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "DELETE FROM Users WHERE Id = @id",
                    new
                    {
                        id = user.Id
                    });
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            
        }

        // FIND'S
        public async Task<MyUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefault(
                    "SELECT * FROM Users WHERE Id = @id",
                    new { id = userId });
            }
        }

        public async Task<MyUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefault(
                    "SELECT * FROM Users WHERE NormalizedUserName = @nome",
                    new { nome = normalizedUserName });
            }
        }

        // GET DATABASE
        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection("Provider=SQLOLEDB.1;" +
                                                "Integrated Security=SSPI;" +
                                                "Persist Security Info=False;" +
                                                "Initial Catalog=IdentityTest;" +
                                                @"Data Source=SHU_01\SQLEXPRESS");

            connection.Open();

            return connection;
        }

        // GET'S
        public Task<string> GetNormalizedUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        // SET'S
        public Task SetNormalizedUserNameAsync(MyUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(MyUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;

            return Task.CompletedTask;
        }
    }
}