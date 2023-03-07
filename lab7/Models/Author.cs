using System.ComponentModel.DataAnnotations;

namespace lab7.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public ICollection<Book_Authors> Books { get; set; }
    }
}
