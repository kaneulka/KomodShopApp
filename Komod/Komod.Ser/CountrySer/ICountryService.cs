using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.CountrySer
{
    public interface ICountryService
    {
        IEnumerable<Country> GetCountries();
        Country GetCountry(long id);
        void InsertCountry(Country Country);
        void UpdateCountry(Country Country);
        void DeleteCountry(long id);
    }
}
