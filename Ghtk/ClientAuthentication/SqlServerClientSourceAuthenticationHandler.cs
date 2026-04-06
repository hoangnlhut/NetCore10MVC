using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAuthentication
{
    public class SqlServerClientSourceAuthenticationHandler : IClientSourceAuthenticationHandler, IDisposable
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        private bool disposedValue = false;

        public SqlServerClientSourceAuthenticationHandler(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
        }

        public bool Validate(string clientSource)
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }

            var query = "SELECT TOP 1 1 FROM ClientSoures WHERE ClientId = @ClientResource AND GETDATE() >= ValidFrom AND GETDATE() <= ValidTo AND IsEnable = 1";

            using var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@ClientResource", clientSource);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return true;
            }

            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_connection.State == System.Data.ConnectionState.Open)
                    {
                        _connection.Close();
                    }
                    _connection.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
