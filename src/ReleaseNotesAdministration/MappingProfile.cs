using AutoMapper;
using ReleaseNotesAdministration.Models;
using ReleaseNotesAdministration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseNotesAdministration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<List<ReleaseNoteAdminApiModel>, ReleaseNoteAdminViewModel>();
            CreateMap<List<ReleaseNoteAdminViewModel>, ReleaseNoteAdminApiModel>();
            CreateMap<ReleaseNoteAdminViewModel, ReleaseNoteAdminApiModel>();
            CreateMap<ReleaseNoteAdminApiModel, ReleaseNoteAdminViewModel> ();
        }
    }
}
