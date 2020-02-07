using System.Collections.Generic;

namespace ReleaseNotes.ViewModels
{
    public class HomeControllerViewModel
    {
        public List<ReleaseNoteViewModel> ReleaseNotes { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
