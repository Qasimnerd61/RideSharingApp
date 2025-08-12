using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Types;
using RideSharingApp.Models;
using System.Data;
using System.Data.SqlTypes;

namespace RideSharingApp.Repository.Rides
{
    public class RidesRepository : IRidesRepository
    {
        private readonly string _connectionString;

        public RidesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> CreateRideAsync(RidesRequest rideDto, SqlGeography? routeGeography)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.CreateRide", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserId", rideDto.UserId);
                command.Parameters.AddWithValue("@UserType", rideDto.UserType);
                command.Parameters.AddWithValue("@StartLat", rideDto.StartLat);
                command.Parameters.AddWithValue("@StartLng", rideDto.StartLng);
                command.Parameters.AddWithValue("@EndLat", rideDto.EndLat);
                command.Parameters.AddWithValue("@EndLng", rideDto.EndLng);
                command.Parameters.AddWithValue("@StartLocationName", rideDto.PickupLocationName);
                command.Parameters.AddWithValue("@EndLocationName", rideDto.DropoffLocationName);

                if (string.IsNullOrEmpty(rideDto.EncodedPolyline))
                    command.Parameters.AddWithValue("@EncodedRoutePolyline", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@EncodedRoutePolyline", rideDto.EncodedPolyline);

                if (routeGeography == null)
                    command.Parameters.AddWithValue("@RouteLineString", DBNull.Value);
                else
                    command.Parameters.Add(new SqlParameter("@RouteLineString", SqlDbType.Udt)
                    {
                        UdtTypeName = "geography",
                        Value = routeGeography
                    });

                command.Parameters.AddWithValue("@DepartureTime", rideDto.DepartureTime);

                if (rideDto.AvailableSeats > 0)
                    command.Parameters.AddWithValue("@AvailableSeats", rideDto.AvailableSeats);
                else
                    command.Parameters.AddWithValue("@AvailableSeats", DBNull.Value);

                command.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(rideDto.Status) ? "Active" : rideDto.Status);

                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
        }
        public async Task<IEnumerable<RideDto>> GetAvailableRidesForPassengerAsync(MatchingRidesRequest request)
        {
            var rides = new List<RideDto>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.GetAvailableRidesForPassenger", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StartLat", request.StartLat);
                command.Parameters.AddWithValue("@StartLng", request.StartLng);
                command.Parameters.AddWithValue("@EndLat", request.EndLat);
                command.Parameters.AddWithValue("@EndLng", request.EndLng);
                command.Parameters.AddWithValue("@BufferDistance", request.BufferDistance);
                command.Parameters.AddWithValue("@PassengerDepartureTime", request.DepartureTime);
                command.Parameters.AddWithValue("@PassengerRideId", request.PassengerRideId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        rides.Add(new RideDto
                        {
                            RideId = reader.GetInt32(reader.GetOrdinal("RideId")),
                            UserType = reader.GetString(reader.GetOrdinal("UserType")),
                            DepartureTime = reader.GetDateTime(reader.GetOrdinal("DepartureTime")),
                            AvailableSeats = reader.IsDBNull(reader.GetOrdinal("AvailableSeats"))
                                             ? (int?)null
                                             : reader.GetInt32(reader.GetOrdinal("AvailableSeats")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            PickupLocationName = reader.IsDBNull(reader.GetOrdinal("StartLocationName"))
                                                ? null
                                                : reader.GetString(reader.GetOrdinal("StartLocationName")),
                            DropoffLocationName = reader.IsDBNull(reader.GetOrdinal("EndLocationName"))
                                                ? null
                                                : reader.GetString(reader.GetOrdinal("EndLocationName")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                            // Map other fields if needed
                        });
                    }
                }
            }

            return rides;
        }
        public async Task<int> AddRideMatch(RideMatchRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_AddRideMatch", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@RideId", request.DriverRideId);
            cmd.Parameters.AddWithValue("@PassengerId", request.PassengerRideId);
            cmd.Parameters.AddWithValue("@Fare", (object?)request.Fare ?? DBNull.Value);

            conn.Open();
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public async Task<IEnumerable<RideMatchDto>> GetPendingRequestsForDriver(int driverId)
        {
            var result = new List<RideMatchDto>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetPendingRideMatchesForDriver", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@DriverId", driverId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new RideMatchDto
                {
                    MatchId = Convert.ToInt32(reader["MatchId"]),
                    PassengerRideId = Convert.ToInt32(reader["PassengerRideId"]),
                    DriverRideId = Convert.ToInt32(reader["DriverRideId"]),
                    Fare = reader["Fare"] == DBNull.Value ? null : (decimal?)reader["Fare"],
                    Status = reader["Status"].ToString(),
                    RequestedAt = Convert.ToDateTime(reader["RequestedAt"]),
                    PassengerName = reader["PassengerName"].ToString(),
                    PassengerPickup = reader["PassengerPickup"].ToString(),
                    PassengerDropoff = reader["PassengerDropoff"].ToString(),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                });
            }

            return result; // return as IEnumerable<RideMatchDto>
        }
        public async Task UpdateRideMatchStatusAsync(int matchId , string newStatus)
        {
            var result = new List<RideMatchDto>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_UpdateRideMatchStatus", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@MatchId", matchId);
            cmd.Parameters.AddWithValue("@NewStatus", newStatus);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

    }




}
