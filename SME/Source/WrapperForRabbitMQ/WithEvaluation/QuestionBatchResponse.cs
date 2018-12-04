using System.Collections.Generic;
namespace SME.Models
{
    public class QuestionBatchResponse
    {
        public string Username { get; set; }
        public Dictionary<string, List<Question>> ResponseDictionary { get; set; }
        public QuestionBatchResponse(string Username, Dictionary<string, List<Question>> responseDictionary)
        {
            this.Username = Username;
            ResponseDictionary = responseDictionary;
        }
    }
}