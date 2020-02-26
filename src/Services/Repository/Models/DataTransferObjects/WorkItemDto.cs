using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Repository.Models.DataTransferObjects
{
    public class WorkItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AssignedTo { get; set; }
        public string State { get; set; }
    }
}
