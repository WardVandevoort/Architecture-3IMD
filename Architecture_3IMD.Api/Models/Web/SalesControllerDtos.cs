using System.ComponentModel.DataAnnotations;
using Architecture_3IMD.Models;
using Architecture_3IMD.Models.Domain;

namespace Architecture_3IMD.Models.Web
{
    // You can also group classes inside of a file. Not really best practice but if you have lots of small
    // DTOs this is actually more readable. Do not do this for complex objects and try to group them by domain. 
    public class CreateSaleDto
    {
        [Required]
         int Id { get; set; }
         int Store_id { get; set; }
         int Bouquet_id { get; set; }
         int Amount { get; set; }
         string FirstName { get; set; }
         string LastName { get; set; }
    }

     public class SaleLinks
     {
        public SaleLinks(string self)
        {
            Self = self;
        }
        public string Self { get; set; }
     }

    public class ViewSaleDto
    {
        public ViewSaleDto()
        {

        }

        public ViewSaleDto(Sale sale, SaleLinks meta)
        {
            Id = sale.Id;
            Store_id = sale.Store_id;
            Bouquet_id = sale.Bouquet_id;
            Amount = sale.Amount;
            FirstName = sale.FirstName;
            LastName = sale.LastName;
            Meta = meta;
        }

        public SaleLinks Meta { get; set; }
        public int Id { get; set; }
        public int Store_id { get; set; }
        public int Bouquet_id { get; set; }
        public int Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}