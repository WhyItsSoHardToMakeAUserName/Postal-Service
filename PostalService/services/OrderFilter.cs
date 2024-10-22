using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PostalService.models;

namespace PostalService.services
{
    public class OrderFilter
    {
        private readonly DatabaseContext context;
        public OrderFilter(DatabaseContext _context){
            context = _context;
        }

    public async Task FilterDistrictOrders(string district,DateTime dateTime){
        try{
            var districtId = await context.Districts.Where(d => d.Name == district.ToLower())
                .Select(d => d.Id)
                .FirstOrDefaultAsync();

            var orders = context.Orders.Where(o => (o.DistrictId == districtId)& (o.TimeOfDelivery>=dateTime) &&(o.TimeOfDelivery<=dateTime.AddMinutes(30)))
                .OrderBy(o => o.TimeOfDelivery)
                .ToList();
                
            Logger.Log("список IP-адресов из файла журнала, входящих в указанный диапазон с количеством обращений с этого адреса в указанный интервал времени.");
            foreach(Order o in orders){
                Logger.Log($"Order Id:{o.Id} --- District:{o.District} Weight:{o.Weight} TimeOffDelivery:{o.TimeOfDelivery}");
            }

             var deliveryOrder = orders
                .GroupBy(o => o.IpAddress)
                .Select(g => new deliveryOrder
                {
                    IpAddress = g.Key,
                    Count = g.Count()
                })
                .ToList();
                
            

            await context.Database.ExecuteSqlRawAsync("DELETE FROM DeliveryOrder");
            
            await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='DeliveryOrder'");
            Logger.Log("previous dilevery order cleaned");

            await context.DeliveryOrder.AddRangeAsync(deliveryOrder);

            await context.SaveChangesAsync();
            Logger.Log("delivery order updated");
        }
        catch{
            Console.WriteLine("err");
        }
    }


    public async Task PopulateDataAsync(){
        // Add districts
        var districts = new List<District>
        {
            new District { Name = "manhattan" },
            new District { Name = "hollywood" },
            new District { Name = "chinatown" },
            new District { Name = "georgetown" },
            new District { Name = "texas" }
        };

        await context.Districts.AddRangeAsync(districts);
        await context.SaveChangesAsync();

        // Add orders
        var orders = new List<Order>
        {
        
            new Order { IpAddress = "192.166.234.1", DistrictId = 1, Weight = 15.5f, TimeOfDelivery = DateTime.Now.AddMinutes(12) },
            new Order { IpAddress = "192.166.234.12", DistrictId = 1, Weight = 20.3f, TimeOfDelivery = DateTime.Now.AddMinutes(5) },
            new Order { IpAddress = "192.166.234.12", DistrictId = 1, Weight = 20.3f, TimeOfDelivery = DateTime.Now.AddMinutes(5) },
            new Order { IpAddress = "192.166.234.1", DistrictId = 1, Weight = 22.8f, TimeOfDelivery = DateTime.Now.AddMinutes(45) },
            new Order { IpAddress = "192.166.234.2", DistrictId = 2, Weight = 12.7f, TimeOfDelivery = DateTime.Now.AddMinutes(30) },
            new Order { IpAddress = "192.166.234.34", DistrictId = 1, Weight = 14.6f, TimeOfDelivery = DateTime.Now.AddMinutes(50) },
            new Order { IpAddress = "192.166.234.2", DistrictId = 2, Weight = 18.5f, TimeOfDelivery = DateTime.Now.AddMinutes(15) },
            new Order { IpAddress = "192.166.234.4", DistrictId = 4, Weight = 10.2f, TimeOfDelivery = DateTime.Now.AddMinutes(1) },
            new Order { IpAddress = "192.166.234.1", DistrictId = 1, Weight = 25.1f, TimeOfDelivery = DateTime.Now.AddMinutes(33) },
            new Order { IpAddress = "192.166.234.5", DistrictId = 5, Weight = 15.5f, TimeOfDelivery = DateTime.Now.AddMinutes(8) },
            new Order { IpAddress = "192.166.234.34", DistrictId = 1, Weight = 30.0f, TimeOfDelivery = DateTime.Now.AddMinutes(22) },
            
            new Order { IpAddress = "192.166.234.1", DistrictId = 1, Weight = 14.6f, TimeOfDelivery = DateTime.Now.AddMinutes(50) },
            new Order { IpAddress = "192.166.234.3", DistrictId = 3, Weight = 11.4f, TimeOfDelivery = DateTime.Now.AddMinutes(16) },
            new Order { IpAddress = "192.166.234.4", DistrictId = 4, Weight = 19.7f, TimeOfDelivery = DateTime.Now.AddMinutes(2) },
            new Order { IpAddress = "192.166.234.12", DistrictId = 2, Weight = 23.8f, TimeOfDelivery = DateTime.Now.AddMinutes(29) },
            new Order { IpAddress = "192.166.234.5", DistrictId = 5, Weight = 28.4f, TimeOfDelivery = DateTime.Now.AddMinutes(54) },
            new Order { IpAddress = "192.166.234.12", DistrictId = 1, Weight = 12.0f, TimeOfDelivery = DateTime.Now.AddMinutes(17) },
            new Order { IpAddress = "192.166.234.3", DistrictId = 3, Weight = 17.2f, TimeOfDelivery = DateTime.Now.AddMinutes(44) },
            new Order { IpAddress = "192.166.234.34", DistrictId = 1, Weight = 15.1f, TimeOfDelivery = DateTime.Now.AddMinutes(11) },
            new Order { IpAddress = "192.166.234.2", DistrictId = 2, Weight = 26.6f, TimeOfDelivery = DateTime.Now.AddMinutes(36) },
            new Order { IpAddress = "192.166.234.1", DistrictId = 1, Weight = 30.3f, TimeOfDelivery = DateTime.Now.AddMinutes(27) },

            new Order { IpAddress = "192.166.234.5", DistrictId = 5, Weight = 22.1f, TimeOfDelivery = DateTime.Now.AddMinutes(49) },
            new Order { IpAddress = "192.166.234.12", DistrictId = 1, Weight = 16.3f, TimeOfDelivery = DateTime.Now.AddMinutes(20) },
            new Order { IpAddress = "192.166.234.3", DistrictId = 2, Weight = 11.5f, TimeOfDelivery = DateTime.Now.AddMinutes(59) },
            new Order { IpAddress = "192.166.234.4", DistrictId = 3, Weight = 29.9f, TimeOfDelivery = DateTime.Now.AddMinutes(35) },
            new Order { IpAddress = "192.166.234.8", DistrictId = 4, Weight = 18.8f, TimeOfDelivery = DateTime.Now.AddMinutes(9) },
            new Order { IpAddress = "192.166.234.5", DistrictId = 5, Weight = 14.7f, TimeOfDelivery = DateTime.Now.AddMinutes(3) },
            new Order { IpAddress = "192.166.234.1", DistrictId = 1, Weight = 21.3f, TimeOfDelivery = DateTime.Now.AddMinutes(55) },
            new Order { IpAddress = "192.166.234.12", DistrictId = 2, Weight = 12.5f, TimeOfDelivery = DateTime.Now.AddMinutes(32) },
            new Order { IpAddress = "192.166.234.3", DistrictId = 3, Weight = 10.1f, TimeOfDelivery = DateTime.Now.AddMinutes(41) },
            new Order { IpAddress = "192.166.234.4", DistrictId = 4, Weight = 27.0f, TimeOfDelivery = DateTime.Now.AddMinutes(47) },

            new Order { IpAddress = "192.166.234.5", DistrictId = 5, Weight = 13.2f, TimeOfDelivery = DateTime.Now.AddMinutes(18) },
            new Order { IpAddress = "192.166.234.1", DistrictId = 1, Weight = 19.6f, TimeOfDelivery = DateTime.Now.AddMinutes(58) },
            new Order { IpAddress = "192.166.234.12", DistrictId = 2, Weight = 15.9f, TimeOfDelivery = DateTime.Now.AddMinutes(39) },
            new Order { IpAddress = "192.166.234.34", DistrictId = 3, Weight = 28.4f, TimeOfDelivery = DateTime.Now.AddMinutes(46) },
            new Order { IpAddress = "192.166.234.4", DistrictId = 4, Weight = 22.7f, TimeOfDelivery = DateTime.Now.AddMinutes(23) },
            new Order { IpAddress = "192.166.234.5", DistrictId = 5, Weight = 17.5f, TimeOfDelivery = DateTime.Now.AddMinutes(24) },
            new Order { IpAddress = "192.166.234.1", DistrictId = 1, Weight = 29.0f, TimeOfDelivery = DateTime.Now.AddMinutes(52) },
            new Order { IpAddress = "192.166.234.3", DistrictId = 2, Weight = 12.8f, TimeOfDelivery = DateTime.Now.AddMinutes(42) },
            new Order { IpAddress = "192.166.234.12", DistrictId = 3, Weight = 14.9f, TimeOfDelivery = DateTime.Now.AddMinutes(8) },
            new Order { IpAddress = "192.166.234.4", DistrictId = 4, Weight = 25.4f, TimeOfDelivery = DateTime.Now.AddMinutes(31) },

            new Order { IpAddress = "192.166.234.5", DistrictId = 5, Weight = 18.2f, TimeOfDelivery = DateTime.Now.AddMinutes(53) },
            new Order { IpAddress = "192.166.234.1", DistrictId = 1, Weight = 16.7f, TimeOfDelivery = DateTime.Now.AddMinutes(26) },
            new Order { IpAddress = "192.166.234.12", DistrictId = 2, Weight = 20.6f, TimeOfDelivery = DateTime.Now.AddMinutes(40) },
            new Order { IpAddress = "192.166.234.34", DistrictId = 3, Weight = 22.3f, TimeOfDelivery = DateTime.Now.AddMinutes(34) },
            new Order { IpAddress = "192.166.234.4", DistrictId = 4, Weight = 11.0f, TimeOfDelivery = DateTime.Now.AddMinutes(14) },
            new Order { IpAddress = "192.166.234.5", DistrictId = 5, Weight = 27.9f, TimeOfDelivery = DateTime.Now.AddMinutes(4) },
            new Order { IpAddress = "192.166.234.1", DistrictId = 1, Weight = 24.1f, TimeOfDelivery = DateTime.Now.AddMinutes(57) },
            new Order { IpAddress = "192.166.234.2", DistrictId = 2, Weight = 13.6f, TimeOfDelivery = DateTime.Now.AddMinutes(25) },
            new Order { IpAddress = "192.166.234.12", DistrictId = 3, Weight = 19.1f, TimeOfDelivery = DateTime.Now.AddMinutes(48) },
            new Order { IpAddress = "192.166.234.34", DistrictId = 4, Weight = 30.2f, TimeOfDelivery = DateTime.Now.AddMinutes(10) },
        };

        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync(); // Save all orders
    }
    }
}