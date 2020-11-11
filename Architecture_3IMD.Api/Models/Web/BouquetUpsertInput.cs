using System.ComponentModel.DataAnnotations;

namespace Architecture_3IMD.Models.Web
{
    public class BouquetUpsertInput
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        [Required]
        public int Price { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }
    }
}