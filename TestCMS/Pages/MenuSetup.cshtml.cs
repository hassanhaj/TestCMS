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
    public class MenuSetupModel : PageModel
    {
        TestCMSContext _db;
        public IEnumerable<MenuItems> MenuItems { get; set; }
        [BindProperty]
        public MenuItems CurrentMenuItem { get; set; }
        public MenuSetupModel(TestCMSContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync(int id)
        {
            await FillPageData(id);
        }

        private async Task FillPageData(int id)
        {
            var query = _db.MenuItems
                            .Where(e => e.ParentId == null)
                            .Include(e => e.Children)
                            .AsQueryable();

            MenuItems = await query.ToArrayAsync();

            if (id > 0)
            {
                var currentMenuQuery = _db.MenuItems
                    .Where(e => e.Id == id)
                    .Include(e => e.Children);

                CurrentMenuItem = currentMenuQuery.FirstOrDefault();
            }
        }

        public async Task OnPostAsync(int id)
        {
            if (id > 0)
            {
                // Edit
                CurrentMenuItem.Id = id;
                var item = _db.Set<MenuItems>().Attach(CurrentMenuItem);
                item.State = EntityState.Modified;
                foreach (var property in item.Properties)
                {
                    property.IsModified = false;
                }
                item.Property(e => e.Title).IsModified = true;
                item.Property(e => e.MenuUrl).IsModified = true;
                await _db.SaveChangesAsync();
            }
            else
            {
                // Add
                _db.MenuItems.Add(CurrentMenuItem);
                await _db.SaveChangesAsync();
            }
            await FillPageData(0);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            _db.MenuItems.Remove(new AppDbContext.Models.MenuItems {Id = id });
            await _db.SaveChangesAsync();
            await FillPageData(0);
            return RedirectToPage("MenuSetup", new { id="" });
        }

    }
}
