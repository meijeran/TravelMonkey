using System.Collections.Generic;
using System.Linq;

namespace TravelMonkey.Models
{
    public class FaceResult
    {
        public int TotalHappyFaceCount => Details.Count(x => x.Emotion == "Happiness");
        
        public List<FaceDetails> Details { get; private set; } = new List<FaceDetails>();
    }

    public class FaceDetails
    {
        public string Gender { get; set; }
        public string Emotion { get; set; }
    }
}