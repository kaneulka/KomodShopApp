using Komod.Data;
using Komod.Repo.CountryRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.CountrySer
{
    public class CountryService : ICountryService
    {
        private ICountryRepository countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public IEnumerable<Country> GetCountries()
        {
            return countryRepository.GetAll();
        }

        public Country GetCountry(long id)
        {
            return countryRepository.Get(id);
        }

        public void InsertCountry(Country country)
        {
            countryRepository.Insert(country);
        }
        public void UpdateCountry(Country country)
        {
            countryRepository.Update(country);
        }

        public void DeleteCountry(long id)
        {
            Country country = GetCountry(id);
            countryRepository.Remove(country);
            countryRepository.SaveChanges();
        }
    }
}
