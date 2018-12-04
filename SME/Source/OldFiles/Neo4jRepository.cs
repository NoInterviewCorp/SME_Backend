// using System;
// using System.IO;
// using System.Linq;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;
// using SME.Models;
// using Neo4j.Driver.V1;
// namespace SME.Persistence
// {

//     public class Neo4jRepository : IDisposable, IDatabaseRepository
//     {
//         private readonly IDriver _driver;

//         public Neo4jRepository(string uri, string user, string password)
//         {
//             _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
//         }

//         public void PrintGreeting(string message)
//         {
//             using (var session = _driver.Session())
//             {
//                 var greeting = session.WriteTransaction(tx =>
//                 {
//                     var result = tx.Run("CREATE (a:Greeting) " +
//                                         "SET a.message = $message " +
//                                         "RETURN a.message + ', from node ' + id(a)",
//                         new { message });
//                     return result.Single()[0].As<string>();
//                 });
//                 Console.WriteLine(greeting);
//             }
//         }

//         public void Dispose()
//         {
//             _driver?.Dispose();
//         }        
//     }
// }