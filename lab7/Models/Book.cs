using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace lab7.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Publish_on { get; set; }
        public string Publisher { get; set; }
        [Required][Range(5, 300)]
        public int Price { get; set; }
        public string ImageURL { get; set; }
        [Required]
        public ICollection<Book_Authors> Authors { get; set; }
        public ICollection<Tag> Tags { get; set; }
        
        
        public ICollection<Review> Reviews{get;set;}
       
        public PriceOffer Offer { get; set; }




    }
}
