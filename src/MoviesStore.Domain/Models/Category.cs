namespace MoviesStore.Domain.Models
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public IEnumerable<Movie> Movies { get; set; }
    }
}
