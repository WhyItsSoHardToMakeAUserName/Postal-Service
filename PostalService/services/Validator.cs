using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace PostalService.services
{
    public  class Validator
    {

        public static (string,DateTime) ValidateFilterInput(string input , DatabaseContext context){
            
            if(input == null) {
                Logger.Log("input is null");
                return("",DateTime.MinValue);
            }
            
            int index = input.IndexOf(":");
            
            //Check if district and datetime is seperated by :
            if(index == -1) {
                Logger.Log("Invalid input ->  no ':' used in input");
                return("",DateTime.MinValue);
            }

            //Initialize return values
            string district = input.Substring(0,index);
            DateTime dateTime;

            //Check if District exists and is valid
            try{
                context.Districts.First(d => d.Name == district);
            }
            catch{
                Logger.Log("Invalid District");
                return("",DateTime.MinValue);
            }

            //Check if datetime is in valid format
            try{
                dateTime = DateTime.Parse(input.Substring(index + 1));
            }
            catch{
                Logger.Log("Invalid dateTime");
                return("",DateTime.MinValue);
            }

            Logger.Log("Validation completed successfully");


            return (district,dateTime);
        }
    }
}