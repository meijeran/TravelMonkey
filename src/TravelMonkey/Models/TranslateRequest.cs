using System.Collections;
using System.Collections.Generic;

namespace TravelMonkey.Models
{
    public class TranslateRequest
    {
        public string From { get; set; }
        public IEnumerable<string> To { get; set; }
        public string Text { get; set; }
    }
}