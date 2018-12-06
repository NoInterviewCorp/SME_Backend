using System.Collections.Generic;
namespace SME.Models
{
    public class QuestionBatchRequest
    {
        public string Username { get; set; }
        public List<string> IdRequestList;
        public QuestionBatchRequest(){
            IdRequestList = new List<string>();
        }
        public QuestionBatchRequest(string Username)
        {
            this.Username = Username;
            IdRequestList = new List<string>();
        }
    }
}