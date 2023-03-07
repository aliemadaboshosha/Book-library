using System.ComponentModel.DataAnnotations.Schema;

namespace lab7.Models
{
    public class Book_Authors
    {
        [ForeignKey("Book")]
       public int Book_id { get; set; }
        [ForeignKey("Author")]
       public int Author_id { get; set; }
        public virtual Book Book { get; set; }
        public virtual Author Author { get; set; }
        public int? Order { get; set; }

    }
}
