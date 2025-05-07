using Microsoft.Data.SqlClient;
using s30543_kolokwium1.Models;

namespace s30543_kolokwium1.Services;

public class VisitService : iVisitService
{
    private readonly string _connectionString =
        "Data Source=localhost,1433;Initial Catalog=kolokwium_db;User ID=sa;Password=2025LocalDBWW1@;TrustServerCertificate=True";

    public async Task<bool> DoesVisitExist(int visitId)
    {
        var query = "SELECT 1 FROM Visit WHERE visit_id = @visitId";

        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@visitId", visitId);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<VisitGetDTO> GetVisitByIdAsync(int visitId)
    {
        var visit = new VisitGetDTO();
        
        var query = "SELECT * FROM Visit WHERE visit_id = @visitId;";
        
        await using SqlConnection conn = new SqlConnection(_connectionString);
        await using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.Add(new SqlParameter("@visitId", visitId));
            await conn.OpenAsync();
            
            var reader = await cmd.ExecuteReaderAsync();
            
            var visitIdOrdinal = reader.GetOrdinal("visit_id");
            var clientIdOrdinal = reader.GetOrdinal("client_id");
            var mechanicIdOrdinal = reader.GetOrdinal("mechanic_id");
            var dateOrdinal = reader.GetOrdinal("date");
            
            while (await reader.ReadAsync())
            {
                visit = new VisitGetDTO()
                {
                    visit_id = reader.GetInt32(visitIdOrdinal),
                    client_id = await GetClientByIdAsync(reader.GetInt32(clientIdOrdinal)),
                    mechanic_id = await GetMechanicByIdAsync(reader.GetInt32(mechanicIdOrdinal)),
                    date = reader.GetDateTime(dateOrdinal)
                };
            }
        }
        return visit;
    }

    public async Task<ClientGetDTO> GetClientByIdAsync(int clientId)
    {
        var client = new ClientGetDTO();
        
        var query = "SELECT * FROM Client WHERE client_id = @clientId;";
        
        await using SqlConnection conn = new SqlConnection(_connectionString);
        await using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@clientId", clientId);
            await conn.OpenAsync();
            
            var reader = await cmd.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                client = new ClientGetDTO()
                {
                    client_id = reader.GetInt32(0),
                    first_name = reader.GetString(1),
                    last_name = reader.GetString(2),
                    date_of_birth = reader.GetDateTime(3)
                };
            }
        }
        return client;
    }

    public async Task<MechanicGetDTO> GetMechanicByIdAsync(int mechanicId)
    {
        var mechanic = new MechanicGetDTO();
        
        var query = "SELECT * FROM Mechanic WHERE mechanic_id = @mechanicId;";
        
        await using SqlConnection conn = new SqlConnection(_connectionString);
        await using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@mechanicId", mechanicId);
            await conn.OpenAsync();
            
            var reader = await cmd.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                mechanic = new MechanicGetDTO()
                {
                    mechanic_id = reader.GetInt32(0),
                    first_name = reader.GetString(1),
                    last_name = reader.GetString(2),
                    license_number = reader.GetString(3)
                };
            }
        }
        return mechanic;
    }

    public async Task<List<ServiceGetDTO>> GetVisitServicesAsync(int visitId)
    {
        var services = new List<ServiceGetDTO>();
        
        var query = @"SELECT s.Name, s.base_fee 
                        FROM Service s
                        INNER JOIN Visit_Service vs ON s.service_id = vs.service_id
                        INNER JOIN Visit v ON v.visit_id = vs.visit_id
                        WHERE v.visit_id = @visitId;";
        
        await using SqlConnection conn = new SqlConnection(_connectionString);
        await using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@visitId", visitId);
            await conn.OpenAsync();
            
            var reader = await cmd.ExecuteReaderAsync();
            
            var nameOrdinal = reader.GetOrdinal("name");
            var baseFeeOrdinal = reader.GetOrdinal("base_fee");

            while (await reader.ReadAsync())
            {
                services.Add(new ServiceGetDTO()
                {
                    name = reader.GetString(nameOrdinal),
                    base_fee = reader.GetDecimal(baseFeeOrdinal)
                });
            }
        }

        return services;
    }

    public async Task<MechanicGetDTO> GetMechanicByLicenseAsync(string lNumber)
    {
        var mechanic = new MechanicGetDTO();
        
        var query = "SELECT * FROM Mechanic WHERE licence_number = @lNumber;";
        
        await using SqlConnection conn = new SqlConnection(_connectionString);
        await using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@lNumber", lNumber);
            await conn.OpenAsync();
            
            var reader = await cmd.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                mechanic = new MechanicGetDTO()
                {
                    mechanic_id = reader.GetInt32(0),
                    first_name = reader.GetString(1),
                    last_name = reader.GetString(2),
                    license_number = reader.GetString(3)
                };
            }
        }
        return mechanic;
    }

    public async Task<VisitGetDTO> AddVisitAsync(VisitCreateDTO visit)
    {
        var query = @"INSERT INTO Visit(visit_id, client_id)
                       VALUES (@visit_id, @client_id);";

        var licence_number = visit.mechanic_license_number;
        
        await using SqlConnection conn = new SqlConnection(_connectionString);
        await using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@visit_id", visit.visit_id);
            cmd.Parameters.AddWithValue("@client_id", visit.client_id);

            return new VisitGetDTO()
            {
                visit_id = visit.visit_id,
                client_id = await GetClientByIdAsync(visit.client_id),
                mechanic_id = await GetMechanicByLicenseAsync(licence_number),
                date = DateTime.Now
            };
        }
    }
}