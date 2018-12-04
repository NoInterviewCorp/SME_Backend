using System.Collections.Generic;
namespace SME.Models
{
    public class QuestionBatchResponse
    {
        public string Username { get; set; }
        public Dictionary<string, List<Question>> RequestDictionary { get; set; }
        public QuestionBatchResponse(string Username, Dictionary<string, List<Question>> response)
        {
            this.Username = Username;
            RequestDictionary = new Dictionary<string, List<Question>>();
        }
    }
}