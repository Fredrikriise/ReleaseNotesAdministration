﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Repository.Models.DatabaseModels
{
    public class WorkItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AssignedTo { get; set; }
        public string State { get; set; }
    }
}
