using System.Collections.Generic;

namespace Alfred.Services.Models
{
    public class Prize
    {
        public string Year { get; set; }
        public List<Laureate> Laureates { get; set; }
    }
}