using System.Collections.Generic;
namespace SME.Models
{
    public class QuestionBatchRequest
    {
        public string Username { get; set; }
        public Dictionary<string, List<string>> IdRequestDictionary;
        public QuestionBatchRequest(string Username)
        {
            this.Username = Username;
            IdRequestDictionary = new Dictionary<string, List<string>>();
        }
    }
}