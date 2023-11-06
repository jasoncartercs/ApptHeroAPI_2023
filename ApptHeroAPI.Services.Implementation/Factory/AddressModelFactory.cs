using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Factory
{
    public interface IAddressModelFactory
    {
        AddressModel Create(Address address);
    }

    public class AddressModelFactory : IAddressModelFactory
    {
        private readonly IStateProvinceModelFactory _stateProvinceModelFactory;
        public AddressModelFactory(IStateProvinceModelFactory stateProvinceModelFactory)
        {
            _stateProvinceModelFactory = stateProvinceModelFactory;
        }
        public AddressModel Create(Address address)
        {
            if(address == null)
            {
                return new AddressModel();
            }

            StateProvinceModel state = _stateProvinceModelFactory.Create(address.StateProvince);
            return new AddressModel
            {
                AddressId = address.AddressId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                LocationName = address.LocationName,
                ZipCode = address.ZipCode,
                StateCode = state.StateCode,
                StateName = state.StateName,
                StateProvinceId = state.StateProvinceId
            };
        }
    }
}
