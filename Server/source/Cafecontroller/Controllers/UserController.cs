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
using Newtonsoft.Json;
using Inkton.Nester;
using Inkton.Nester.Models;
using Inkton.Nester.Queue;
using Cloud = Inkton.Nester.Cloud;
using Wppod.Models;

namespace Wppod.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly CafeContext _cafeContext;
        private readonly ILogger _logger;
        private readonly Runtime _runtime;        
        
        public UserController(CafeContext cafeContext,
            ILogger<UserController> logger)
        {
            _cafeContext = cafeContext;
            _logger = logger;
            _runtime = new Runtime(QueueMode.Server);
            _runtime.QueueSendType = "OrderItem";
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            Cloud.Result<Wppod.Models.User> result = 
                new Cloud.Result<Wppod.Models.User>();

            result.SetSuccess("users", _cafeContext.Users.ToList());

            return Ok(result);    
        }

        [HttpGet]
        [Route("{email}")]        
        public IActionResult Get(string email)
        {
            Cloud.Result<Wppod.Models.User> result = 
                new Cloud.Result<Wppod.Models.User>();
            
            try
            {                
                Wppod.Models.User user = _cafeContext.Users.FirstOrDefault(
                        searchUser => searchUser.Email == email);

                if (user == null)
                {
                    result.SetFail("User not found");
                }
                else
                {
                    result.SetSuccess("user", user);                
                }
            }
            catch (System.Exception e)
            {
                result.SetFail(e, "Query");
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] Wppod.Models.User user)
        {
            Cloud.Result<Wppod.Models.User> result = 
                new Cloud.Result<Wppod.Models.User>();
            
            try
            {
                if (!ModelState.IsValid)
                {
                    result.SetFail("Invalid model state");
                }
                else
                {
                    string status = string.Format("User Create request, received by Nest-{0}.{1} at {2}",  
                            _runtime.NestTag, _runtime.CushionIndex, DateTime.Now.ToString("t"));
                    _logger.LogInformation(status);
        
                    _cafeContext.Users.Add(user);
                    await _cafeContext.SaveChangesAsync();

                    result.SetSuccess("user", user);
                }                
            }
            catch (System.Exception e)
            {
                result.SetFail(e, "CreateAsync");
            }

            return Ok(result);            
        }

        [HttpPost]
        [Route("{user_id}/orders")]  
        public async Task<IActionResult> CreateOrderAsync(
            long user_id, [FromBody] Order order)
        {
            Cloud.Result<Order> result = new Cloud.Result<Order>();

            try
            {
                if (!ModelState.IsValid)
                {
                    result.SetFail("Invalid model state");
                }
                else
                {
                    _logger.LogInformation(string.Format(
                        "Order Create request from user id {0} received by Nest-{1}.{2} at {3}. {4}", 
                        user_id, _runtime.NestTag, _runtime.CushionIndex, DateTime.Now.ToString("t"),
                        JsonConvert.SerializeObject(order)));

                    Wppod.Models.User user = _cafeContext.Users.FirstOrDefault( 
                            searchUser => searchUser.Id == user_id);

                    if (user == null)
                    {
                        result.SetFail("User not found");
                    }
                    else
                    {
                        order.User = user;
                        _cafeContext.Orders.Add(order);
                        await _cafeContext.SaveChangesAsync();
                                            
                        result.SetSuccess("order", order);
                    }
                }
            }
            catch (System.Exception e)
            {
                result.SetFail(e, "CreateOrderAsync");
            }
            
            return Ok(result);            
        }    

        [HttpPost]
        [Route("{user_id}/orders/{order_id}/order_items")]  
        public async Task<IActionResult> CreateOrderItemAsync(
            long user_id, long order_id, [FromBody] OrderItem orderItem)
        {
            Cloud.Result<OrderItem> result = new Cloud.Result<OrderItem>();

            try
            {
                if (!ModelState.IsValid)
                {
                    result.SetFail("Invalid model state");
                }
                else
                {
                    _logger.LogInformation(string.Format(
                        "Order Item Create request from user id {0} received by Nest-{1}.{2} at {3}. {4}", 
                        user_id, _runtime.NestTag, _runtime.CushionIndex, DateTime.Now.ToString("t"),
                        JsonConvert.SerializeObject(orderItem)));

                    Wppod.Models.Order order = _cafeContext.Orders.FirstOrDefault( 
                            searchOrder => searchOrder.Id == order_id);

                    if (order == null)
                    {
                        result.SetFail("Order not found");
                    }
                    else
                    {
                        _cafeContext.OrderItems.Add(orderItem);
                        await _cafeContext.SaveChangesAsync();
                        
                        _runtime.SendToNest(
                            JsonConvert.SerializeObject(orderItem), 
                            "stockallocator");
                    
                        result.SetSuccess("order_item", orderItem);
                    }
                }
            }
            catch (System.Exception e)
            {
                result.SetFail(e, "CreateOrderAsync");
            }
            
            return Ok(result);            
        }                
    }
}
