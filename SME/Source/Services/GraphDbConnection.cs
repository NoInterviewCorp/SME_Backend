using Neo4jClient;
using System;
namespace SME.Services
{
    public class GraphDbConnection : IDisposable
    {
        private GraphClient _client;
        public GraphClient Client { get { return _client; } }
        public GraphDbConnection()
        {
            _client = new GraphClient(new Uri("http://172.23.238.173:17474/db/data"), "neo4j", "qwertyuiop");
            _client.Connect();
        }
        public void Dispose(){
            _client.Dispose();
        }
    }
}