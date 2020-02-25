using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class Utils
    {
        string Title;
        string BodyText;
        int Id;
        int ProductId;
        string CreatedBy;
        DateTime CreatedDate;
        string LastUpdatedBy;
        DateTime LastUpdateDate;

        public void setValue()
        {
            Title = "Release notes 0.93 - Talent Onboarding";
            BodyText = "Lorem ipsum oasfjhifhiwhgywhghwi";
            Id = 1;
            ProductId = 1;
            CreatedBy = "Fredrik Riise";
            CreatedDate = DateTime.Now;

        }

        public string DoStuff(string stuff)
        {
            return "Fredrik";
        }
    }
}
