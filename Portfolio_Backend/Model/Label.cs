namespace Portfolio_Backend.Model
{
    public class Label
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<BlogLabel> BlogLabels { get; set; }
    }
}
