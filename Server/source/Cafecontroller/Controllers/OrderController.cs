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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Inkton.Nester;
using Inkton.Nester.Models;
using Inkton.Nester.Queue;
using Cloud = Inkton.Nester.Cloud;
using Wppod.Models;

namespace Wppod.Controllers
{
    [Route("orders")]
    public class OrderController : Controller
    {
        private readonly CafeContext _cafeContext;
        private readonly ILogger _logger;

        public OrderController(CafeContext cafeContext,
            ILogger<OrderController> logger)
        {
            _cafeContext = cafeContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string date)
        {
            Cloud.Result<Wppod.Models.Order> result = 
                new Cloud.Result<Wppod.Models.Order>();

            try
            {
                System.Linq.IQueryable<Wppod.Models.Order> items = null;

                if (date != null && date.Length > 0)
                {
                    DateTime checkTime = DateTime.Parse(date);
                    items = _cafeContext.Orders
                        .Where(searchOrder => searchOrder.VisitDate.Date == checkTime.Date);
                }
                else
                {
                    items = _cafeContext.Orders;
                }

                if (items == null)
                {
                    result.SetFail("Orders not found");
                }
                else
                {
                    result.SetSuccess("orders", items.ToList());                
                }
            }
            catch (System.Exception e)
            {
                result.SetFail(e, "GetAsync");
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("{order_id}/order_items")] 
        public IActionResult GetItems(long order_id)
        {
            Cloud.Result<OrderItem> result = new Cloud.Result<OrderItem>();
            
            try
            {                
                var items = _cafeContext.OrderItems.Where(
                    orderItem => orderItem.Order.Id == order_id);

                if (items == null)
                {
                    result.SetFail("Items not found");
                }
                else
                {
                    result.SetSuccess("order_items", items.ToList());                
                }
            }
            catch (System.Exception e)
            {
                result.SetFail(e, "GetItems");
            }

            return Ok(result);
        }        
    }
}
