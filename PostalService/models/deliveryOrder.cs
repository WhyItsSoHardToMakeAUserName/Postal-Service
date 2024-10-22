using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostalService.models
{
    public class deliveryOrder
    {
        [Key]
        public int Id{get;set;}
        public required string IpAddress{get;set;}
        public required int Count{get;set;}
    }
}