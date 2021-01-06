using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Architecture_3IMD.Models.Domain
{

public class Store
{
     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public int Id{ get; set; }

     [Required]
     public string Name{get; set;}

     [Required]
     public string Address{get; set;}

     [Required]
     public string StreetNumber{get; set;}

     [Required]
     public string Region{get; set;}
}

}