using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SME.Persistence;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SME
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // using (var greeter = new Neo4jRepository("bolt://localhost:7687", "neo4j", "qwertyuiop"))
            // {
            //     greeter.PrintGreeting("hello, world");
            // }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
