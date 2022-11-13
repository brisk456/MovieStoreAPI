namespace MovieStore.API.Dtos.Movie
{
    public class MovieResultDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
