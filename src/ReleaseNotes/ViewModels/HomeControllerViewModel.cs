using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseNotes.ViewModels
{
    public class HomeControllerViewModel
    {
        public List<ReleaseNoteViewModel> ReleaseNotes { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
