namespace Portfolio_Backend.Model
{
    public class Timeline
    {
        public Guid Id { get; set; }
        public string Icon_Url { get; set; }
        public string Label { get; set; }
        public string Start_Date { get; set; }
        public string Completed_Date { get; set; }
        public string Description { get; set; }

    }
}
