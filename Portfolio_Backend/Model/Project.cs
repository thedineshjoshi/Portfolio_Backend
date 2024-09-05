namespace Portfolio_Backend.Model
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string Link { get; set; }
    }

}
