using Npgsql;

namespace FistWeb.Data
{
    public class ConnectionTester
    {
        private readonly string _connectionString;

        public ConnectionTester(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> TestAsync()
        {
            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                Console.WriteLine("Kết nối thành công đến PostgreSQL!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kết nối thất bại: " + ex.Message);
                return false;
            }
        }
    }
}
