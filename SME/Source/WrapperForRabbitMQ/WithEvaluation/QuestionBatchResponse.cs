using System.Collections.Generic;
namespace SME.Models
{
    public class QuestionBatchResponse
    {
        public string Username { get; set; }
        public List<Question> ResponseList { get; set; }
        public QuestionBatchResponse()
        {
            ResponseList = new List<Question>();
        }
        public QuestionBatchResponse(string Username, List<Question> responseList)
        {
            this.Username = Username;
            ResponseList = responseList;
        }
    }
}