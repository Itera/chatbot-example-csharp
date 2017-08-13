using System;

namespace Alfred.Models
{
    [Serializable]
    public class UserData
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public long BirthYear { get; set; }
    }
}