using System.ComponentModel.DataAnnotations;

namespace Architecture_3IMD.Models.Web
{
    public class SaleUpsertInput
    {
        public int Id { get; set; }

        [Required]
        public int Store_id { get; set; }
        
        [Required]
        public int Bouquet_id { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
    }
}