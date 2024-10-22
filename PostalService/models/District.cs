using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostalService.models
{
    public class District
    {
        [Key]
        public int Id{get;set;}
        public required string Name{get;set;}
        public List<Order> Orders{get;set;} = new();
    }
}