using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Wppod.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Wppod.Models
{
    public static class DBinitialize
    {
        public static void EnsureCreated(IServiceProvider serviceProvider)
        {
            var context = new CafeContext(
                serviceProvider.GetRequiredService<DbContextOptions<CafeContext>>());
            context.Database.EnsureCreated();
        }

        public static void EnsureSeeded(IServiceProvider serviceProvider)
        {
            var context = new CafeContext(
                serviceProvider.GetRequiredService<DbContextOptions<CafeContext>>());

            if (!context.Menus.Any())
            {
                // Create menus                
                context.Menus.Add(
                        new Menu()
                            {
                                Name = "Breakfast"
                            });

                context.Menus.Add(
                        new Menu()
                            {
                                Name = "Lunch"
                            });

                context.SaveChangesAsync();
            }
        }
    }
}