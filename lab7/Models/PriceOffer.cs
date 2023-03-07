using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab7.Models
{
    public class PriceOffer
    {
        [Key]
        public int PriceOfferId { get; set; }
        public int NewPrice { get; set; }
        public string OfferTxt { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        
        public Book Book { get; set; }
    }
}
