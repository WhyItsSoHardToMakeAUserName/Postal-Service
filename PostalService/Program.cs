using PostalService;
using PostalService.services;

//Initialized DbContext and FilterService
var context = new DatabaseContext();
var orderFilter = new OrderFilter(context);


// This function is used to populate empty db after migration
// await orderFilter.PopulateDataAsync();


Console.WriteLine("Enter Address and DateTime using : in between");
Console.WriteLine("Example -> hollywod:2024-10-22 18:50:00");

var input = Console.ReadLine();

(string,DateTime) data = Validator.ValidateFilterInput(input,context);

if(data!=("",DateTime.MinValue)){
    await orderFilter.FilterDistrictOrders(data.Item1,data.Item2);
}
else{
    Console.WriteLine("Invalid input");
}