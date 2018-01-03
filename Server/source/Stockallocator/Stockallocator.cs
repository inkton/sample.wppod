/*
    Copyright (c) 2017 Inkton.

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the "Software"),
    to deal in the Software without restriction, including without limitation
    the rights to use, copy, modify, merge, publish, distribute, sublicense,
    and/or sell copies of the Software, and to permit persons to whom the Software
    is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
    OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
    CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
    TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
    OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Inkton.Nester;
using Inkton.Nester.Models;
using Inkton.Nester.Queue;
using Microsoft.EntityFrameworkCore;
using Wppod.Models;

namespace Wppod.Work
{
    class Stockallocator
    {
        static object Parse(IDictionary<string, object> headers, string message)
        {
            if (headers != null && headers.ContainsKey("Type"))
            {
                switch(Encoding.Default.GetString(headers["Type"] as byte[]))
                {
                    case "MenuItem":
                        return JsonConvert.DeserializeObject<MenuItem>(message);
                    case "Order":
                        return JsonConvert.DeserializeObject<Order>(message);                    
                }
            }

            return null;
        } 

        static async void CreateStockAsync(Runtime runtime, MenuItem menuItem)
        {            
            using (var cafeContext = CafeContextFactory.Create(runtime))
            {
                switch(menuItem.FoodType)
                {
                    case FoodType.HotBeverage:
                        AllocateHelper.AddStock(cafeContext, "Coffee");
                        AllocateHelper.AddStock(cafeContext, "Sugar");
                        AllocateHelper.AddStock(cafeContext, "Milk");
                        break;
                    case FoodType.Sandwich:
                        AllocateHelper.AddStock(cafeContext, "Bread");
                        AllocateHelper.AddStock(cafeContext, "Butter");
                        AllocateHelper.AddStock(cafeContext, "Cheese");
                        break;                                  
                    case FoodType.Salad:
                        AllocateHelper.AddStock(cafeContext, "Lettuce");
                        AllocateHelper.AddStock(cafeContext, "Dressing");
                        AllocateHelper.AddStock(cafeContext, "Tomato");
                        break;                                   
                }

                await cafeContext.SaveChangesAsync();
            }        
        }

        static async void AllocateStockAsync(Runtime runtime, Order order)
        {
            // Allocate stock
            using (var cafeContext = CafeContextFactory.Create(runtime))
            {                
                foreach (OrderItem item in order.Items)
                {
                    switch(item.MenuItem.FoodType)
                    {
                        case FoodType.HotBeverage:
                            AllocateHelper.StockUpForHotBeverage(
                                cafeContext, item.Quantity);
                            break;

                        case FoodType.Sandwich:
                            AllocateHelper.StockUpForSandwich(
                                cafeContext, item.Quantity);
                            break;

                        case FoodType.Salad:
                            AllocateHelper.StockUpForSalad(
                                cafeContext, item.Quantity);
                            break;

                    }
                    
                    await cafeContext.SaveChangesAsync();                                    
                }                                                          
            }
        }

        static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddNester(LogLevel.Debug);
            ILogger logger = loggerFactory.CreateLogger<Stockallocator>();
            Runtime runtime = new Runtime(QueueMode.Client);

            try
            {
		        bool isActive = true;
                Object message;
                Runtime.ReceiveParser parser = new Runtime.ReceiveParser(Parse);

                while (isActive) 
                {
                    message = runtime.Receive(parser);

                    if (message != null) 
                    {
                        logger.LogInformation(string.Format(
                            "Worker woke up to process received in Nest-{0}.{1} at {2}. {3}", 
                            runtime.NestTag, runtime.CushionIndex, DateTime.Now.ToString("t"),
                            JsonConvert.SerializeObject(message)));

                        if (message is MenuItem)
                        {
                            CreateStockAsync(runtime, message as MenuItem);
                        }
                        else if (message is Order)
                        {
                            AllocateStockAsync(runtime, message as Order);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogInformation("Exception thrown -> " + e.Message);
            }
        }
    }
}
