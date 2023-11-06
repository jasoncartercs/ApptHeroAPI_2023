using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface ITeammateService
    {
        public List<PersonModel> GetTeammates(long companyId);

        public bool ArchiveTeamMember(long companyId, long personId);

        public bool ReinstateTeamMember(long companyId, long personId);

        public bool SaveTeamMember(PersonModel personModel, PersonModel adminModel, List<AddOnModel> addonModels);
    }
}
