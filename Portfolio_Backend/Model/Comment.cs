namespace Portfolio_Backend.Model
{
    public class Comment
    {
        public Guid Id { get; set; }                     
        public string Name { get; set; }  
        public string Email { get; set; } 
        public string Content { get; set; } 
        public DateTime PostedAt { get; set; }   
        public Guid BlogId { get; set; }                   
    }
}
