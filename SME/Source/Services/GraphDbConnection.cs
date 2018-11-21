using Microsoft.Extensions.Options;
using Neo4jClient;
using System;
namespace SME.Services
{
    public class GraphDbConnection : IDisposable
    {
        private GraphClient _client;
        public GraphClient Client { get { return _client; } }
        public GraphDbConnection(IOptions<Neo4jSettings> options)
        {
            _client = new GraphClient(
                new Uri(options.Value.ConnectionString),
                options.Value.UserId,
                options.Value.Password
            );
            _client.Connect();
        }
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}