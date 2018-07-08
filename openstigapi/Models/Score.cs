namespace openstigapi.Models
{
    /// <summary>
    /// This is the class that shows the score of the STIG for all categories
    /// <summary>
    public class Score
    {
        public int NotApplicable { get; set;}
        public int NotReviewed { get; set;}
        public int Open { get; set; }
        public int NotAFinding { get; set; }
    }
}