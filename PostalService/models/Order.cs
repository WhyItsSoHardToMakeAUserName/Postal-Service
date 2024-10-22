using System.ComponentModel.DataAnnotations;

namespace PostalService.models
{
    public class Order
    {
        [Key]
        public int Id{get;set;}
        public required string IpAddress{get;set;}
        public required int DistrictId{get;set;}
        public District? District{get;set;}
        public required float Weight{get;set;}
        public required DateTime TimeOfDelivery{get;set;}
    }
}