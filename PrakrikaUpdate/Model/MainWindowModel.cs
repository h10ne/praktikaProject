using DateBase;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrakrikaUpdate.Model
{
    public class MainWindowModel
    {
        public void AddCountry(Country c)
        {
            using (var db = new Context())
            {
                db.Country.Add(new Country { FullName = c.FullName, ShortName = c.ShortName });
                db.SaveChanges();
            }
        }
        public void ChangeCountry(Country selC)
        {
            using (var db = new Context())
            {
                Country tempCountry = db.Country.Where(c => c.Id == selC.Id).FirstOrDefault();
                tempCountry.FullName = selC.FullName;
                tempCountry.ShortName = selC.ShortName;
                db.SaveChanges();
            }
        }
        public void DeleteCountry(Country country)
        {
            using (var db = new Context())
            {
                Country co = new Country() { Id = country.Id, FullName = country.FullName, ShortName = country.ShortName };
                db.Country.Attach(co);
                db.Country.Remove(co);
                db.SaveChanges();
            };
        }

        public void AddRegion(string nameregion, string country)
        {
            using (var db = new Context())
            {
                var region = new Region { NameRegion = nameregion, Country = db.Country.Where(c => c.FullName == country).FirstOrDefault() };
                db.Region.Add(region);
                db.SaveChanges();
            }
        }

        public void ChangeRegion(Region region, string nameregion, string country)
        {
            using (var db = new Context())
            {
                Region tempRegion = db.Region.Where(c => c.Id == region.Id).FirstOrDefault();
                tempRegion.NameRegion = nameregion;
                tempRegion.CountryId = db.Country.Where(c => c.FullName == country).FirstOrDefault().Id;
                db.SaveChanges();
            }
        }

        public void DeleteRegion(Region region)
        {
            using (var db = new Context())
            {
                Region re = new Region() { Id = region.Id };
                db.Region.Attach(re);
                db.Region.Remove(re);
                db.SaveChanges();
            };
        }

        public void AddCity(City city)
        {
            using (var db = new Context())
            {
                db.City.Add(city);
                db.SaveChanges();
            }
        }
        public void ChangeCity(City city)
        {
            using (var db = new Context())
            {
                City oldcity = db.City.Where(c => c.Id == city.Id).FirstOrDefault();
                oldcity.NameCity = city.NameCity;
                oldcity.RegionId = db.Region.Where(r => r.NameRegion == city.Region.NameRegion).FirstOrDefault().Id;
                db.SaveChanges();
            }
        }

        public void DownloadInfoToLists(ref ObservableCollection<Country> countries, ref ObservableCollection<Region> regions, ref ObservableCollection<City> cities, ref ObservableCollection<Address> addresses)
        {
            countries = new ObservableCollection<Country>();
            regions = new ObservableCollection<Region>();
            cities = new ObservableCollection<City>();
            addresses = new ObservableCollection<Address>();
            using (var db = new Context())
            {
                foreach (var c in db.Country)
                {
                    Country countr = c;
                    Country country = new Country() { Id = countr.Id, FullName = countr.FullName, ShortName = countr.ShortName };
                    countries.Add(country);
                }
                foreach (var r in db.Region)
                {
                    Region region = new Region() { Id = r.Id, NameRegion = r.NameRegion, Country = r.Country };
                    regions.Add(region);
                }
                foreach (var ci in db.City)
                {
                    City city = new City() { Id = ci.Id, NameCity = ci.NameCity, Region = ci.Region };
                    cities.Add(city);
                }
                foreach (var ad in db.Address)
                {
                    Address address = new Address() { Id = ad.Id, Building = ad.Building, City = ad.City, Office = ad.Office, Person = ad.Person, Street = ad.Street };
                    addresses.Add(address);
                }

            }
        }
    }
}
