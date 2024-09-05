namespace Portfolio_Backend.Model
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string Content { get; set; }
        public string YoutubeVideoLink { get; set; }
        public string FeaturedImageUrl { get; set; }
        public ICollection<BlogImage> BlogImages { get; set; }
    }

    public class BlogImage
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
    }

}
