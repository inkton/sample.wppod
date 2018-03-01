/*
    Copyright (c) 2017 Inkton.

    Permission is hereby granted, free of    charge, to any person obtaining
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
    [Route("menus")]
    public class MenuController : Controller
    {
        private readonly CafeContext _cafeContext;
        private readonly ILogger _logger;
        private readonly Runtime _runtime;        
        
        public MenuController(CafeContext cafeContext,
            ILogger<MenuController> logger)
        {
            _cafeContext = cafeContext;
            _logger = logger;
            _runtime = new Runtime(QueueMode.Server);
            _runtime.QueueSendType = "MenuItem";
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                Cloud.Result<Menu> result = new Cloud.Result<Menu>();

                result.SetSuccess("menus", _cafeContext.Menus.ToList());

                return Ok(result);                
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("{menu_id}/menu_items")]
        public IActionResult Get(int menu_id)
        {
            try
            {
                var items = _cafeContext.MenuItems.Where(
                    menuItem => menuItem.Menu.Id == menu_id).ToList();

                Cloud.Result<MenuItem> result = new Cloud.Result<MenuItem>();

                result.SetSuccess("menu_items", items);

                return Ok(result);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("{menu_id}/menu_items/{menu_item_id}")]
        public IActionResult GetItem(int menu_id, int menu_item_id)
        {
            try
            {
                MenuItem menuItem = _cafeContext.MenuItems.FirstOrDefault(
                        searchMenuItem =>
                            searchMenuItem.MenuId == menu_id &&
                            searchMenuItem.Id == menu_item_id );
                
                Cloud.Result<MenuItem> result = new Cloud.Result<MenuItem>();

                result.SetSuccess("menu_item", menuItem);

                return Ok(result);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        [Route("{menu_id}/menu_items")]
        public async Task<IActionResult> CreateAsync(int menu_id, [FromBody] MenuItem item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(422, ModelState);
                }

                Cloud.Result<MenuItem> result = new Cloud.Result<MenuItem>();

                Menu menu = _cafeContext.Menus.FirstOrDefault(
                        searchMenu => searchMenu.Id == menu_id);

                if (menu == null)
                {
                    result.SetFail("Menu not found");
                }
                else
                {
                    item.Menu = menu;

                    string status;
                        status = string.Format("Menu Item Create request, received by Nest {0}.{1} at {2}",  
                            _runtime.NestTag,   
                            _runtime.CushionIndex, 
                        DateTime.Now.ToString("t"));
                    _logger.LogInformation(status);                        

                    status = string.Format("Create under Menu id {0}, the Menu Item {1}", 
                        menu_id, JsonConvert.SerializeObject(item));
                    _logger.LogInformation(status);

                    _cafeContext.MenuItems.Add(item);
                    await _cafeContext.SaveChangesAsync();

                    _runtime.SendToNest(
                        JsonConvert.SerializeObject(item), 
                        "stockallocator");

                    result.SetSuccess("menu_item", item);
                }

                return Ok(result);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [Route("{menu_id}/menu_items/{item_Id}")]                
        [HttpDelete]
        public IActionResult Delete(int menu_id, int item_Id)
        {
             MenuItem item = _cafeContext.MenuItems.FirstOrDefault(
                    menuItem => menuItem.Menu.Id == menu_id && 
                                menuItem.Id == item_Id);
            if (item == null)
            {
                return NotFound();
            }

            _cafeContext.Remove(item);
            _cafeContext.SaveChanges();
            
            _logger.LogInformation(
                "Menu " + menu_id.ToString() + "-" + item_Id.ToString() + " removed from the database"); 

            Cloud.Result<MenuItem> result = new Cloud.Result<MenuItem>();

            result.SetSuccess();

            return Ok(result);            
        }
    }
}
