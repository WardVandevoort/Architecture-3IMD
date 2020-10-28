using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Architecture_3IMD.Models.Domain
{

public class Stores
{
     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public int Id{ get; set; }

     public string Name{get; set;}
     public string Address{get; set;}
     public string Region{get; set;}
}

}