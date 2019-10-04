using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestCMS.AppDbContext;
using TestCMS.AppDbContext.Models;

namespace TestCMS.Pages
{
    public class IndexModel : PageModel
    {
        private string MenuUrl
        {
            get
            {
                return this.RouteData.Values["menuUrl"]?.ToString();
            }
        }

        TestCMSContext _db;
        public IEnumerable<MenuItems> MenuItems { get; set; }
        public MenuItems CurrentMenuItem { get; set; }
        public IndexModel(TestCMSContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            var query = _db.MenuItems
                .Where(e => e.ParentId == null)
                .Include(e => e.Children)
                .AsQueryable();

            MenuItems = await query.ToArrayAsync();

            if (MenuUrl != null)
            {
                var currentMenuQuery = _db.MenuItems
                    .Where(e => e.MenuUrl == MenuUrl)
                    .Include(e => e.Children);

                this.CurrentMenuItem = currentMenuQuery.First();
            }
        }
    }
}
