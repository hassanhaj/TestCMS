using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestCMS.AppDbContext.Models
{
    public partial class MenuItems
    {
        public MenuItems()
        {
            Children = new HashSet<MenuItems>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        [StringLength(300)]
        public string Title { get; set; }
        [StringLength(300)]
        public string MenuUrl { get; set; }
        public string ContentText { get; set; }

        [ForeignKey("ParentId")]
        [InverseProperty("Children")]
        public virtual MenuItems Parent { get; set; }
        [InverseProperty("Parent")]
        public virtual ICollection<MenuItems> Children { get; set; }
    }
}
