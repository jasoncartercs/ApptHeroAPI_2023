using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Factory
{
    public interface IStateProvinceModelFactory
    {
        StateProvinceModel Create(StateProvince state);
    }

    public class StateProvinceModelFactory : IStateProvinceModelFactory
    {

        public StateProvinceModel Create(StateProvince state)
        {
            if (state == null)
            {
                return new StateProvinceModel();
            }

            return new StateProvinceModel
            {
                StateCode = state.StateCode,
                CountryId = state.CountryId,
                StateName = state.StateName,
                StateProvinceId = state.StateProvinceId
            };
        }
    }
}
