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
using System.Linq;
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
    public class AllocateHelper
    {
        public static void AddStock(CafeContext cafeContext, string name)
        {
            if (cafeContext.Stocks.FirstOrDefault(
                stock => stock.Name == name) == null)
            {
                cafeContext.Stocks.Add(new Stock { Name = name });
            }
        }

        public static Stock GetStock(CafeContext cafeContext, string name)
        {
            return cafeContext.Stocks.FirstOrDefault(
                stock => stock.Name == name);      
        }

        public static void StockUpForHotBeverage(CafeContext cafeContext, int serves)
        {
            Stock stock;

            stock = GetStock(cafeContext, "Coffee");    
            if (stock != null)
            {
                cafeContext.StockItems.Add(
                    new StockItem { Stock = stock, Quantity = serves * 10, Unit = "g", TimeRequired = DateTime.Now });
            }
            stock = GetStock(cafeContext, "Sugar");
            if (stock != null)
            {
                cafeContext.StockItems.Add(
                    new StockItem { Stock = stock, Quantity = serves * 1, Unit = "g", TimeRequired = DateTime.Now });
            }
            stock = GetStock(cafeContext, "Milk");
            if (stock != null)
            {
                cafeContext.StockItems.Add(
                    new StockItem { Stock = stock, Quantity = serves * 100, Unit = "ml", TimeRequired = DateTime.Now });
            }
        }

        public static void StockUpForSandwich(CafeContext cafeContext, int serves)
        {
            Stock stock;

            stock = GetStock(cafeContext, "Bread");
            if (stock != null)
            {
                cafeContext.StockItems.Add(
                    new StockItem { Stock = stock, Quantity = 2, Unit = "slices", TimeRequired = DateTime.Now });
            }
            stock = GetStock(cafeContext, "Butter");
            if (stock != null)
            {
                cafeContext.StockItems.Add(
                    new StockItem { Stock = stock, Quantity = 5, Unit = "g", TimeRequired = DateTime.Now });
            }
            stock = GetStock(cafeContext, "Cheese");
            if (stock != null)
            {
                cafeContext.StockItems.Add(
                    new StockItem { Stock = stock, Quantity = 100, Unit = "g", TimeRequired = DateTime.Now });
            }
        }

        public static void StockUpForSalad(CafeContext cafeContext, int serves)
        {
            Stock stock;

            stock = GetStock(cafeContext, "Lettuce");
            if (stock != null)
            {
                cafeContext.StockItems.Add(
                    new StockItem { Stock = stock, Quantity = 2, Unit = "slices", TimeRequired = DateTime.Now });
            }
            stock = GetStock(cafeContext, "Dressing");
            if (stock != null)
            {
                cafeContext.StockItems.Add(
                    new StockItem { Stock = stock, Quantity = 5, Unit = "g", TimeRequired = DateTime.Now });
            }
            stock = GetStock(cafeContext, "Tomato");
            if (stock != null)
            {
                cafeContext.StockItems.Add(
                    new StockItem { Stock = stock, Quantity = 100, Unit = "g", TimeRequired = DateTime.Now });
            }
        }
    }
}
