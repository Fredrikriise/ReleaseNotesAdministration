using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseNotesAdministration.ViewModels
{
    public class ProductAdminViewModel
    {
        [Key]
        public int? ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public string ProductDescription { get; set; }
    }
}
