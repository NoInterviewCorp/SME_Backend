// using SME.Models;
// using SME.Services;
// using Neo4jClient;
// using System;
// using System.Threading.Tasks;
// using System.Collections.Generic;
// using System.Dynamic;

// namespace SME.Persistence
// {
//     public class ResourceRepository : IResourceRepository
//     {
//         private GraphClient graph;
//         public ResourceRepository(GraphDbConnection graph)
//         {
//             this.graph = graph.Client;
//         }
//         // Resource
//         public async Task<Resource> AddResourceAsync(Resource resource)
//         {
//             // Add an unique id to resource
//             resource.ResourceId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
//             // Linking technologies between resource and each of its concepts
//             List<Concept> concepts = new List<Concept>();
//             List<Technology> technologies = new List<Technology>();
//             if (resource.Concepts != null)
//             {
//                 concepts = new List<Concept>(resource.Concepts);
//                 resource.Concepts = null;
//             }
//             else
//             {
//                 return null;
//             }
//             if (resource.Technologies != null)
//             {
//                 technologies = new List<Technology>(resource.Technologies);
//                 resource.Technologies = null;
//             }
//             else
//             {
//                 return null;
//             }
//             // queries
//             // query to create a resource
//             var matchResource = graph.Cypher
//                 .Match("(r:Resource)")
//                 .Where((Resource r) => r.ResourceLink == resource.ResourceLink)
//                 .Return(r => r.As<Resource>())
//                 .Results;
//             var list = new List<Resource>(matchResource);
//             if (list.Count > 0)
//             {
//                 return null;
//             }
//             else
//             {
//                 var resQuery = new List<Resource>(
//                     await graph.Cypher
//                         .Create("(res:Resource {resource})")
//                         .WithParams(new
//                         {
//                             resource
//                         })
//                         .Return(res => res.As<Resource>())
//                         .ResultsAsync
//                     )[0];
//                 var conceptQuery = graph.Cypher;
//                 var techCypherQuery = graph.Cypher;
//                 var paramsObj = new ExpandoObject() as IDictionary<string, Object>;
//                 for (int h = 0; h < concepts.Count; h++)
//                 {
//                     Concept concept = concepts[h];
//                     // Converting resource/technology/concept names to upper case
//                     // for faster indexed searches
//                     concept.Name = concept.Name.ToUpper();
//                     // To avoid "Property already exists" error with neo4j
//                     // WithParams() call
//                     var conceptParamsObj = new ExpandoObject() as IDictionary<string, Object>;
//                     conceptParamsObj.Add("conceptName" + h, concept.Name);
//                     conceptParamsObj.Add("concept" + h, concept);
//                     conceptQuery = conceptQuery
//                           .Merge($"(con{h}:Concept {{ Name: {{conceptName{h}}}}})")
//                           .OnCreate()
//                           .Set($"con{h}={{concept{h}}}")
//                           .With($"con{h}")
//                           .Match("(r:Resource)")
//                           .Where((Resource r) => r.ResourceId == resource.ResourceId)
//                           .Merge($"(r)-[:EXPLAINS]->(con{h})")
//                           .WithParams(conceptParamsObj);
//                     for (int i = 0; i < technologies.Count; i++)
//                     {
//                         conceptParamsObj.Add("techParamsObj" + h + i, new ExpandoObject() as IDictionary<string, object>);
//                         // Converting resource/technology/concept names to upper case
//                         // for faster indexed searches
//                         var technology = technologies[i];
//                         technology.Name = technology.Name.ToUpper();
//                         (conceptParamsObj["techParamsObj" + h + i] as IDictionary<string, Object>).Add("techName" + h + i, technology.Name);
//                         (conceptParamsObj["techParamsObj" + h + i] as IDictionary<string, Object>).Add("technology" + h + i, technology);
//                         techCypherQuery = techCypherQuery
//                                .Merge($"(t{h}{i}:Technology {{ Name: {{techName{h}{i}}} }})")
//                                .OnCreate()
//                                .Set($"t{h}{i}={{technology{h}{i}}}")
//                                .With($"t{h}{i}")
//                                .Match("(con:Concept)")
//                                .Where((Concept con) => con.Name == concept.Name)
//                                .Merge($"(con)-[:BELONGS_TO]->(t{h}{i})")
//                                .WithParams((conceptParamsObj["techParamsObj" + h + i] as IDictionary<string, Object>));
//                     }
//                 }
//                 await conceptQuery.ExecuteWithoutResultsAsync();
//                 await techCypherQuery.ExecuteWithoutResultsAsync();
//                 return resQuery;
//             }
//         }

//         public async Task<List<Resource>> GetResourcesAsync()
//         {
//             var results = new List<Resource>(
//                 await graph.Cypher
//                     .Match("(r:Resource)")
//                     .Return(r => r.As<Resource>())
//                     .ResultsAsync
//             );
//             foreach (Resource resource in results)
//             {
//                 resource.Concepts = new List<Concept>(
//                         await graph.Cypher
//                     .Match("(r:Resource)-[:EXPLAINS]->(c:Concept)")
//                     .Where((Resource r) => r.ResourceId == resource.ResourceId)
//                     .ReturnDistinct(c => c.As<Concept>())
//                     .ResultsAsync
//                 );
//                 resource.Technologies = new List<Technology>(
//                         await graph.Cypher
//                     .Match("(r:Resource)-[:EXPLAINS]->(:Concept)-[:BELONGS_TO]->(t:Technology)")
//                     .Where((Resource r) => r.ResourceId == resource.ResourceId)
//                     .ReturnDistinct(t => t.As<Technology>())
//                     .ResultsAsync
//                 );
//             }
//             return results;
//         }
//         // TODO: Add search by link
//         public async Task<List<Resource>> GetResourceByStringAsync(string text)
//         {
//             text = text.ToUpper();
//             var query = graph.Cypher
//                     .Match("(r:Resource)-[:EXPLAINS]->(:Concept)-[:BELONGS_TO]->(t:Technology)")
//                     .Where("t.Name CONTAINS {text}")
//                     .WithParams(new
//                     {
//                         text
//                     })
//                     .ReturnDistinct(r => r.As<Resource>());
//             var results = new List<Resource>(
//                 await query
//                     .ResultsAsync
//             );
//             foreach (Resource resource in results)
//             {
//                 resource.Concepts = new List<Concept>(
//                         await graph.Cypher
//                     .Match("(r:Resource)-[:EXPLAINS]->(c:Concept)")
//                     .Where((Resource r) => r.ResourceId == resource.ResourceId)
//                     .ReturnDistinct(c => c.As<Concept>())
//                     .ResultsAsync
//                 );
//                 resource.Technologies = new List<Technology>(
//                         await graph.Cypher
//                     .Match("(r:Resource)-[:EXPLAINS]->(:Concept)-[:BELONGS_TO]->(t:Technology)")
//                     .Where((Resource r) => r.ResourceId == resource.ResourceId)
//                     .ReturnDistinct(t => t.As<Technology>())
//                     .ResultsAsync
//                 );
//             }
//             return results;
//         }
//         // public Task<List<Resource>> GetResourceByTechnologyAsync(string technology)
//         // {
//         //     return null;
//         // }

//         // TODO: Add the ability to break relationships when a concept or technology
//         // is deleted from the resource
//         // TODO: Throw error if no concepts & technologies are passed 
//         // as they're required
//         public async Task<Resource> UpdateResourceAsync(Resource resource)
//         {
//             // Linking technologies between resource and each of its concepts
//             List<Concept> concepts = new List<Concept>();
//             List<Technology> technologies = new List<Technology>();
//             if (resource.Concepts != null)
//             {
//                 concepts = new List<Concept>(resource.Concepts);
//                 resource.Concepts = null;
//             }
//             if (resource.Technologies != null)
//             {
//                 technologies = new List<Technology>(resource.Technologies);
//                 resource.Technologies = null;
//             }
//             var result = new List<Resource>(
//                 await graph.Cypher
//                 .Match("(r:Resource)")
//                 .Where((Resource r) => r.ResourceId == resource.ResourceId)
//                 .Set("r = {resource}")
//                 .WithParam("resource", resource)
//                 .Return(r => r.As<Resource>())
//                 .ResultsAsync
//             )[0]; var conceptQuery = graph.Cypher;
//             var techCypherQuery = graph.Cypher;
//             var paramsObj = new ExpandoObject() as IDictionary<string, Object>;
//             for (int h = 0; h < concepts.Count; h++)
//             {
//                 Concept concept = concepts[h];
//                 // Converting resource/technology/concept names to upper case
//                 // for faster indexed searches
//                 concept.Name = concept.Name.ToUpper();
//                 // To avoid "Property already exists" error with neo4j
//                 // WithParams() call
//                 var conceptParamsObj = new ExpandoObject() as IDictionary<string, Object>;
//                 conceptParamsObj.Add("conceptName" + h, concept.Name);
//                 conceptParamsObj.Add("concept" + h, concept);
//                 conceptQuery = conceptQuery
//                       .Merge($"(con{h}:Concept {{ Name: {{conceptName{h}}}}})")
//                       .OnCreate()
//                       .Set($"con{h}={{concept{h}}}")
//                       .With($"con{h}")
//                       .Match("(r:Resource)")
//                       .Where((Resource r) => r.ResourceId == resource.ResourceId)
//                       .Merge($"(r)-[:EXPLAINS]->(con{h})")
//                       .WithParams(conceptParamsObj);
//                 for (int i = 0; i < technologies.Count; i++)
//                 {
//                     conceptParamsObj.Add("techParamsObj" + h + i, new ExpandoObject() as IDictionary<string, object>);
//                     // Converting resource/technology/concept names to upper case
//                     // for faster indexed searches
//                     var technology = technologies[i];
//                     technology.Name = technology.Name.ToUpper();
//                     (conceptParamsObj["techParamsObj" + h + i] as IDictionary<string, Object>).Add("techName" + h + i, technology.Name);
//                     (conceptParamsObj["techParamsObj" + h + i] as IDictionary<string, Object>).Add("technology" + h + i, technology);
//                     techCypherQuery = techCypherQuery
//                            .Merge($"(t{h}{i}:Technology {{ Name: {{techName{h}{i}}} }})")
//                            .OnCreate()
//                            .Set($"t{h}{i}={{technology{h}{i}}}")
//                            .With($"t{h}{i}")
//                            .Match("(con:Concept)")
//                            .Where((Concept con) => con.Name == concept.Name)
//                            .Merge($"(con)-[:BELONGS_TO]->(t{h}{i})")
//                            .WithParams((conceptParamsObj["techParamsObj" + h + i] as IDictionary<string, Object>));
//                 }
//             }
//             await conceptQuery.ExecuteWithoutResultsAsync();
//             await techCypherQuery.ExecuteWithoutResultsAsync();
//             return result;
//         }

//         public async Task<bool> DeleteResourceAsync(string resourceId)
//         {
//             var result = new List<Resource>(await graph.Cypher
//                 .Match("(r:Resource {ResourceId:{resourceId}})")
//                 .Return(r => r.As<Resource>())
//                 .ResultsAsync);
//             if (result.Count == 0)
//             {
//                 return false;
//             }
//             await graph.Cypher
//                 .OptionalMatch("(r:Resource)-[relation]->()")
//                 .Where((Resource r) => r.ResourceId == resourceId)
//                 .Delete("r, relation")
//                 .ExecuteWithoutResultsAsync();
//             return true;
//         }
//     }
// }