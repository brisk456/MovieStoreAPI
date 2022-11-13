namespace MoviesStore.Domain.Models
{
    public class Movie : Entity
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public String Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
