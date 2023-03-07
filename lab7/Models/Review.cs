using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab7.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string VoterName { get; set; }
        [Range(0,5)]
       public int Stars { get; set; }
        public string Comment { get; set; }
         public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

    }
}
