using Microsoft.Data.SqlClient;
using RideSharingApp.Models;

namespace RideSharingApp.Repository.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> SignupAsync(SignupRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("stp_api_SignUpUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FirstName", request.FName);
                    command.Parameters.AddWithValue("@LastName", request.LName);
                    command.Parameters.AddWithValue("@PhoneNumber", request.PhoneNo);
                    command.Parameters.AddWithValue("@Email", request.Email);
                    command.Parameters.AddWithValue("@PasswordHash", request.Password);

                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        public async Task<UserModel> LoginAsync(LoginRequest loginReq)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("stp_api_LoginUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", loginReq.Email);
                    command.Parameters.AddWithValue("@PasswordHash", loginReq.Password);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new UserModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNo = reader["PhoneNumber"].ToString(),
                                // Add other fields if needed
                            };
                        }
                    }
                }
            }

            return null;
        }


    }
}
