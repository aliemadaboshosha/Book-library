using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace lab7.Models
{
    public class Tag
    {
        [Key]
        public int ID { get; set; } 
        [Required]
        [StringLength(100,MinimumLength =2)]
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
