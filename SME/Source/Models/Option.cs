using System.Collections.Generic;
using Newtonsoft.Json;
namespace SME.Models
{
    public class Option
    {
        public string OptionId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}