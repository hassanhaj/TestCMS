using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestCMS.AppDbContext;
using TestCMS.AppDbContext.Models;

namespace TestCMS.Pages
{
    public class PageContentModel : PageModel
    {
        TestCMSContext _db;
        [BindProperty]
        public MenuItems CurrentMenuItem { get; set; }
        public IFormFile ImageFile { get; set; }
        public PageContentModel(TestCMSContext db)
        {
            _db = db;
        }

        public void OnGetAsync(int id)
        {
            FillPageData(id);
        }

        private void FillPageData(int id)
        {
            if (id > 0)
            {
                CurrentMenuItem = _db.MenuItems
                    .AsNoTracking()
                    .FirstOrDefault(e=>e.Id == id);
            }
        }

        public async Task OnPostAsync(int id)
        {
            // Edit
            CurrentMenuItem.Id = id;

            UploadImage();

            var item = _db.Set<MenuItems>()
                .Attach(CurrentMenuItem);
            item.State = EntityState.Modified;

            foreach (var property in item.Properties)
            {
                property.IsModified = false;
            }
            item.Property(e => e.ContentText).IsModified = true;

            await _db.SaveChangesAsync();

            FillPageData(id);
            RedirectToPage();
        }

        private void UploadImage()
        {
            //var filePath = HostingEnvironment.WebRootPath + "\\Photos\\" + Guid.NewGuid().ToString() + "." + ImageFile.FileName.Split(".").Last();

            //using (Stream output = File.Create(Path.Combine(Contrainer, FileName)))
            //{
            //    byte[] buffer = new byte[8 * 1024];
            //    int len;
            //    while ((len = ImageFile.OpenReadStream().Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        output.Write(buffer, 0, len);
            //    }
            //}

        }
    }
}
