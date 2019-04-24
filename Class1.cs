using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace qwe
{
    public class Address : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int id;
        private string person;
        private string street;
        private int building;
        private int? office;
        private string city;
        public int Id { get { return id; } set { id = value; OnPropertyChanged("AddressId"); } }
        public string Person { get { return person; } set { person = value; OnPropertyChanged("AddressPerson"); } }
        public string Street { get { return street; } set { street = value; OnPropertyChanged("AddressStreet"); } }
        public int Building { get { return building; } set { building = value; OnPropertyChanged("AddressBuilding"); } }
        public int? Office { get { return office; } set { office = value; OnPropertyChanged("AddressOffice"); } }
        public string City
        {
            get { return this.city; }
            set
            {
                this.city = value;
                OnPropertyChanged("AddressCity");
            }
        }
    }
    public class City : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int id;
        private string nameCity;
        private string region;
        public int Id { get => id; set { id = value; OnPropertyChanged("CityId"); } }
        public string NameCity { get { return this.nameCity; } set { this.nameCity = value; OnPropertyChanged("CityNameCity"); } }
        public string Region { get { return region; } set { region = value; OnPropertyChanged("CityRegion"); } }
    }
    public class Region : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int id;
        private string nameRegion;
        private string country;

        public int Id { get { return id; } set { id = value; OnPropertyChanged("RegionId"); } }
        public string NameRegion { get { return nameRegion; } set { nameRegion = value; OnPropertyChanged("RegionNameRegion"); } }
        public string Country { get { return country; } set { country = value; OnPropertyChanged("RegionCountry"); } }
    }
    public class Country : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int id;
        private string fullname;
        private string shortName;
        public int Id { get { return id; } set { id = value; OnPropertyChanged("CountryId"); } }
        public string FullName { get { return fullname; } set { fullname = value; OnPropertyChanged("CountryFullName"); } }
        public string ShortName { get { return shortName; } set { shortName = value; OnPropertyChanged("CountryShortName"); } }
    }

    public class DbCloneTables : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Country> countries;
        public ObservableCollection<Country> Countries { get { return countries; } set { countries = value; OnPropertyChanged("ListCountries"); } }
        public List<Region> Regions { get; set; }
        public List<City> Cities { get; set; }
        public List<Address> Addresses { get; set; }

        public void DownloadInfoToLists()
        {
            Countries = new ObservableCollection<Country>();
            Regions = new List<Region>();
            Cities = new List<City>();
            Addresses = new List<Address>();
            using (var db = new ClientAddresses.ClientAddresses())
            {
                foreach (var c in db.Country)
                {
                    ClientAddresses.Country countr = c;
                    Country country = new Country() { Id = countr.Id, FullName = countr.FullName, ShortName = countr.ShortName };
                    Countries.Add(country);
                }
                foreach (var r in db.Region)
                {
                    Region region = new Region() { Id = r.Id, NameRegion = r.NameRegion, Country = r.Country.FullName };
                    Regions.Add(region);
                }
                foreach (var ci in db.City)
                {
                    City city = new City() { Id = ci.Id, NameCity = ci.NameCity, Region = ci.Region.NameRegion };
                    Cities.Add(city);
                }
                foreach (var ad in db.Address)
                {
                    Address address = new Address() { Id = ad.Id, Building = ad.Building, City = ad.City.NameCity, Office = ad.Office, Person = ad.Person, Street = ad.Street };
                    Addresses.Add(address);
                }

            }
        }
        public DbCloneTables()
        {
            DownloadInfoToLists();
        }
    }
}

