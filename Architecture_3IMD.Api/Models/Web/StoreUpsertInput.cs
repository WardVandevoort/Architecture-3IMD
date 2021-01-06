using System.ComponentModel.DataAnnotations;

namespace Architecture_3IMD.Models.Web
{
    public class StoreUpsertInput
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(255)]
        public string StreetNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string Region { get; set; }
    }
}