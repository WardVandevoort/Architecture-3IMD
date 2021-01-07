using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Architecture_3IMD.Models.Domain
{

public class Revenue
{
     
     [Required]
     public int Store_id{get; set;}

     [Required]
     public int Bouquet_id{get; set;}

     [Required]
     public int BouquetRevenue{get; set;}

}

}