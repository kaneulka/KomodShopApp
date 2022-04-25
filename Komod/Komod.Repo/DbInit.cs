using Komod.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komod.Repo
{
    public class DbInit
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext context)
        {
            if (!context.StockStatus.Any())
            {
                context.StockStatus.AddRange(
                    new StockStatus
                    {
                        Name = "В наличии",
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    },
                    new StockStatus
                    {
                        Name = "Нет в наличии",
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
            if (!context.PaymentMethod.Any())
            {
                context.PaymentMethod.AddRange(
                    new PaymentMethod
                    {
                        Name = "Наличными",
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    },
                    new PaymentMethod
                    {
                        Name = "Банковской картой",
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
            if (!context.OrderStatus.Any())
            {
                context.OrderStatus.AddRange(
                    new OrderStatus
                    {
                        Name = "Отменен",
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    },
                    new OrderStatus
                    {
                        Name = "Подтвержден",
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    },
                    new OrderStatus
                    {
                        Name = "Выполнен",
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    },
                    new OrderStatus
                    {
                        Name = "Обрабатывается",
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    }
                );
                context.SaveChanges();
            }

            if (!context.Color.Any())
            {
                context.Color.AddRange(
                    new Color
                    {
                        Name = "Без цвета",
                        ColorCode = "#000",
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
