namespace Portfolio_Backend.Model
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string MetaDescription { get; set; }
        public string Content { get; set; }
        public string YoutubeVideoLink { get; set; }
        public string FeaturedImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<BlogImage> BlogImages { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }

    public class BlogImage
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public Guid BlogId { get; set; }
    }

}
