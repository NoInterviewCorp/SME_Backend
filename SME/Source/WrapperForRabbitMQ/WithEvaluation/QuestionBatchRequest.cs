using System.Collections.Generic;
namespace SME.Models
{
    public class QuestionBatchRequest
    {
        public string Username { get; set; }
        public Dictionary<string, List<string>> RequestDictionary;
        public QuestionBatchRequest(string Username)
        {
            this.Username = Username;
            RequestDictionary = new Dictionary<string, List<string>>();
        }
    }
}