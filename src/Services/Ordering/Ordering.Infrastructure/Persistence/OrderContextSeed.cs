using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static List<Order> GetPreConfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {UserName = "Lucio", FirstName = "Lucio", LastName = "Mania", EmailAddress = "manialucio@gmail.com", AddressLine = "Via 8 marzo,2", Country = "Italy", TotalPrice = 350 }
            };
    
        }

        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreConfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed ordine di demo context =  {DBContextName}",typeof(OrderContext).Name);
            }
        }
 
    }
}
