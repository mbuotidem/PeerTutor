using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace PeerTutor.Extensions
{
    public static class ExtensionMethods
    {
        public static async Task<SelectList> ToSelectListAsync<T, W>(this DbSet<T> dbSet, Expression<Func<T, W>> selector) where T : class
        {
            var results = await dbSet.AsNoTracking().Select(selector).ToListAsync();
            return new SelectList(results);
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
}
