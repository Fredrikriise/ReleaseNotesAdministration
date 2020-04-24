using ReleaseNotes.ViewModels;
using System;

namespace ReleaseNotes.Models
{
    public class WorkItemApiModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AssignedTo { get; set; }
        public string State { get; set; }

        public object Select(Func<object, ReleaseNoteViewModel> p)
        {
            throw new NotImplementedException();
        }
    }
}
