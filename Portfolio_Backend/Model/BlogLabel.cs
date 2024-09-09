namespace Portfolio_Backend.Model
{
    public class BlogLabel
    {
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
        public Guid LabelId { get; set; }
        public Label Label { get; set; }
    }
}
