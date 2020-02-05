using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReleaseNotes.Models;

namespace ReleaseNotes
{
    public class DBContext
    {
        public List<releaseNotes> MockDataList()
        {
        var bodytextData = "Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis id duis, nisl nulla risus.";

            List<releaseNotes> releaseNotesList = new List<releaseNotes>
            {
                new releaseNotes {
                    title = "Release note 0.1 - Onboarding",
                    bodytext = bodytextData,
                    id = 1,
                    productId = 2,
                    createdBy = "Fredrik Svevad Riise",
                    createdDate = DateTime.ParseExact("27/01/2020", "dd/MM/yyyy", null),
                    lastUpdatedBy = null,
                    lastUpdatedDate = null,
                },
                new releaseNotes {
                    title = "Release note 0.93 - Manager",
                    bodytext = bodytextData,
                    id = 2,
                    productId = 3,
                    createdBy = "Felix Thu Falkendal Nilsen",
                    createdDate = DateTime.ParseExact("28/01/2020", "dd/MM/yyyy", null),
                    lastUpdatedBy = "Felix Thu Falkendal Nilsen",
                    lastUpdatedDate = DateTime.ParseExact("31/01/2020", "dd/MM/yyyy", null),
                },
                 new releaseNotes {
                    title = "Release note 1.03 - Manager",
                    bodytext = bodytextData,
                    id = 3,
                    productId = 3,
                    createdBy = "Fredrik Svevad Riise",
                    createdDate = DateTime.ParseExact("04/02/2020", "dd/MM/yyyy", null),
                    lastUpdatedBy = null,
                    lastUpdatedDate = null
                }
            };
            return releaseNotesList;
        }
        
    }
}
