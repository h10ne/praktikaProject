using PrakrikaUpdate;
using PrakrikaUpdate.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Praktika
{
    public class Address
    {        
        private int id;
        private string person;
        private string street;
        private int building;
        private int? office;
        private string city;
        public int Id { get { return id; } set { id = value; } }
        public string Person { get { return person; } set { person = value; } }
        public string Street { get { return street; } set { street = value; } }
        public int Building { get { return building; } set { building = value;  } }
        public int? Office { get { return office; } set { office = value;  } }
        public string City
        {
            get { return this.city; }
            set { this.city = value;
            }
        }
    }
    public class City
    {
        private int id;
        private string nameCity;
        private string region;
        public int Id { get => id; set { id = value;  } }
        public string NameCity { get { return this.nameCity; } set { this.nameCity = value; } }
        public string Region { get { return region; } set { region = value;} }
    }
    public class Region
    {
        private int id;
        private string nameRegion;
        private string country;

        public int Id { get { return id; } set { id = value; } }
        public string NameRegion { get { return nameRegion; } set { nameRegion = value; } }
        public string Country { get { return country; } set { country = value; } }
    }
    public class Country
    {
        private int id;
        private string fullname;
        private string shortName;
        public int Id { get { return id; } set {  id = value; } }
        public string FullName { get { return fullname; } set { fullname = value;  } }
        public string ShortName { get { return shortName; } set { shortName = value; } }
    }

    public class DbCloneTables : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Country> countries;
        private ObservableCollection<Region> regions;
        private ObservableCollection<City> cities;
        private ObservableCollection<Address> addresses;
        public ObservableCollection<Country> Countries { get { return countries; } set { countries = value; OnPropertyChanged("Countries"); } }
        public ObservableCollection<Region> Regions { get { return regions; } set { regions = value; OnPropertyChanged("Regions"); } }
        public ObservableCollection<City> Cities { get { return cities; } set { cities = value; OnPropertyChanged("Cities"); } }
        public ObservableCollection<Address> Addresses { get { return addresses; } set { addresses = value; OnPropertyChanged("Addresses"); } }

        public void DownloadInfoToLists()
        {
            Countries = new ObservableCollection<Country>();
            Regions = new ObservableCollection<Region>();
            Cities = new ObservableCollection<City>();
            Addresses = new ObservableCollection<Address>();
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
        private Country selectedCountry;
        public Country SelectedCountry
        {
            get
            {
                return selectedCountry;
            }
            set
            {
                selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
                DeleteCountry.CanExecute(true);
            }
        }
        public Commander AddCountry
        {
            get
            {
                return new Commander((obj) =>
                {
                    WindowNewCountry vcountry = new WindowNewCountry();
                    Country c = new Country();
                    vcountry.DataContext = c;
                    if (vcountry.ShowDialog() == true)
                    {
                        using (var db = new ClientAddresses.ClientAddresses())
                        {
                            db.Country.Add(new ClientAddresses.Country { FullName = c.FullName, ShortName = c.ShortName });
                            db.SaveChanges();
                        }
                        DownloadInfoToLists();
                    }
                });

            }
        }

        public Commander DeleteCountry
        {
            get
            {
                return new Commander((obj) =>
                {
                    Country country = SelectedCountry;
                    using (var db = new ClientAddresses.ClientAddresses())
                    {
                        ClientAddresses.Country co = new ClientAddresses.Country() { Id = country.Id, FullName = country.FullName, ShortName = country.ShortName };
                        db.Country.Attach(co);
                        db.Country.Remove(co);
                        db.SaveChanges();
                    };
                    DownloadInfoToLists();

                }, (obj) => SelectedCountry != null);

            }
        }

        public Commander ChangeCountry
        {
            get
            {
                return new Commander((obj) =>
                {
                    WindowNewCountry newCountry = new WindowNewCountry()
                    {
                        Title = "Изменить страну"
                    };
                    Country selC = SelectedCountry;
                    newCountry.DataContext = selC;

                    if (newCountry.ShowDialog() == true)
                    {
                        using (var db = new ClientAddresses.ClientAddresses())
                        {
                            ClientAddresses.Country tempCountry = db.Country.Where(c => c.Id == selC.Id).FirstOrDefault();
                            tempCountry.FullName = selC.FullName;
                            tempCountry.ShortName = selC.ShortName;
                            db.SaveChanges();
                        }
                    }

                    DownloadInfoToLists();
                }, (obj) => SelectedCountry != null);
            }
        }

        public Commander AddRegion
        {
            get
            {
                return new Commander((obj) =>
                {
                    WindowNewRegion vcountry = new WindowNewRegion();
                    Region r = new Region();
                    vcountry.DataContext = r;
                    vcountry.CountryBox.ItemsSource = Countries.Select(c=>c.FullName);
                    try
                    {
                        if (vcountry.ShowDialog() == true)
                        {
                            using (var db = new ClientAddresses.ClientAddresses())
                            {
                                db.Region.Add(new ClientAddresses.Region { NameRegion = r.NameRegion, Country = db.Country.Where(c => c.FullName == r.Country).FirstOrDefault() });
                                db.SaveChanges();
                            }
                            DownloadInfoToLists();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Введенной страны не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                });
            }
        }

        private Region selectedRegion;
        public Region SelectedRegion
        {
            get
            {
                return selectedRegion;
            }
            set
            {
                selectedRegion = value;
                OnPropertyChanged(nameof(SelectedRegion));
            }
        }
        public Commander DeleteRegion
        {
            get
            {
                return new Commander((obj) =>
                {
                    Region region = SelectedRegion;
                    using (var db = new ClientAddresses.ClientAddresses())
                    {
                        ClientAddresses.Region re = new ClientAddresses.Region() { Id = SelectedRegion.Id };
                        db.Region.Attach(re);
                        db.Region.Remove(re);
                        db.SaveChanges();
                    };
                    DownloadInfoToLists();

                }, (obj) => SelectedRegion != null);

            }
        }

        public Commander ChangeRegion
        {
            get
            {
                return new Commander((obj) =>
                {
                    WindowNewRegion newRegion = new WindowNewRegion()
                    {
                        Title = "Изменить регион"
                    };
                    Region selR = SelectedRegion;
                    newRegion.DataContext = selR;
                    newRegion.CountryBox.ItemsSource = Countries.Select(c => c.FullName);
                    newRegion.CountryBox.SelectedItem = SelectedRegion.Country;
                    if (newRegion.ShowDialog() == true)
                    {
                        using (var db = new ClientAddresses.ClientAddresses())
                        {
                            ClientAddresses.Region tempRegion = db.Region.Where(c => c.Id == selR.Id).FirstOrDefault();
                            tempRegion.NameRegion = selR.NameRegion;
                            tempRegion.CountryId = db.Country.Where(c => c.FullName == selR.Country).FirstOrDefault().Id;
                            db.SaveChanges();
                        }
                    }
                    DownloadInfoToLists();
                }, (obj) => SelectedRegion != null);
            }
        }

        public City selectedCity;
        public City SelectedCity
        {
            get
            {
                return selectedCity;
            }
            set
            {
                selectedCity = value;
                OnPropertyChanged((nameof(selectedRegion)));
            }
        }

        public Commander AddCity
        {
            get
            {
                return new Commander((obj) =>
                {
                    WindowNewCity newCity = new WindowNewCity();
                    City city = new City();
                    newCity.DataContext = city;
                    newCity.RegionBox.ItemsSource = Regions.Select(c=>c.NameRegion);
                    if (newCity.ShowDialog() == true)
                    {
                        using (var db = new ClientAddresses.ClientAddresses())
                        {
                            db.City.Add(new ClientAddresses.City() { NameCity = city.NameCity, RegionId = db.Region.Where(r => r.NameRegion == city.Region).FirstOrDefault().Id });
                            db.SaveChanges();
                        }
                        DownloadInfoToLists();
                    }
                });
            }
        }

        public Commander ChangeCity
        {
            get
            {
                return new Commander((obj) =>
                {
                    WindowNewCity newCity = new WindowNewCity()
                    {
                        Title = "Изменить город"
                    };
                    newCity.DataContext = SelectedCity;
                    newCity.RegionBox.ItemsSource = Regions.Select(c => c.NameRegion);
                    newCity.RegionBox.SelectedItem = SelectedCity.Region;
                    if (newCity.ShowDialog() == true)
                    {
                        using (var db = new ClientAddresses.ClientAddresses())
                        {
                            ClientAddresses.City city = db.City.Where(c => c.Id == SelectedCity.Id).FirstOrDefault();
                            city.NameCity = selectedCity.NameCity;
                            city.RegionId = db.Region.Where(r => r.NameRegion == SelectedCity.Region).FirstOrDefault().Id;
                            db.SaveChanges();
                        }
                    }
                    DownloadInfoToLists();
                }, (obj) => SelectedCity != null);
            }
        }

        public Commander DeleteCity
        {
            get
            {
                return new Commander((obj) =>
                {
                    using (var db = new ClientAddresses.ClientAddresses())
                    {
                        ClientAddresses.City city = db.City.Where(c => c.Id == selectedCity.Id).FirstOrDefault();
                        db.City.Attach(city);
                        db.City.Remove(city);
                        db.SaveChanges();
                    }
                    DownloadInfoToLists();
                }, (obj) => SelectedCity != null);
            }
        }

        private Address selectedAddress;
        public Address SelectedAddress
        {
            get
            {
                return selectedAddress;
            }
            set
            {
                selectedAddress = value;
                OnPropertyChanged(nameof(SelectedAddress));                
            }
        }
        public Commander AddAddress
        {
            get
            {
                return new Commander((obj) =>
                {
                    WindowNewAddress newAddress = new WindowNewAddress()
                    {
                        Title = "Новый адрес"
                    };
                    Address address = new Address();
                    newAddress.DataContext = address;
                    newAddress.CityBox.ItemsSource = Cities.Select(c => c.NameCity);
                    if (newAddress.ShowDialog()==true)
                    {
                        using (var db = new ClientAddresses.ClientAddresses())
                        {
                            db.Address.Add(new ClientAddresses.Address()
                            {
                                Person = address.Person,
                                Building = address.Building,
                                Office = address.Office,
                                Street = address.Street,
                                CityId = db.City.Where(c => c.NameCity == address.City).FirstOrDefault().Id
                            });
                            db.SaveChanges();
                        }
                        DownloadInfoToLists();
                    }
                });
            }
        }

        public Commander DeleteAddress
        {
            get
            {
                return new Commander((obj) =>
                {
                    using (var db = new ClientAddresses.ClientAddresses())
                    {
                        ClientAddresses.Address address = db.Address.Where(a => a.Id == SelectedAddress.Id).FirstOrDefault();
                        db.Address.Attach(address);
                        db.Address.Remove(address);
                        db.SaveChanges();
                    }
                    DownloadInfoToLists();
                }, (obj)=>selectedAddress!=null);
            }
        }

        public Commander ChangeAddress
        {
            get
            {
                return new Commander((obj) =>
                {
                    WindowNewAddress newAddress = new WindowNewAddress()
                    {
                        Title = "Изменить адрес"
                    };
                    Address address = SelectedAddress;
                    newAddress.DataContext = address;
                    newAddress.CityBox.ItemsSource = Cities.Select(c => c.NameCity);
                    newAddress.CityBox.SelectedItem = SelectedAddress.City;
                    if (newAddress.ShowDialog() == true)
                    {
                        using (var db = new ClientAddresses.ClientAddresses())
                        {
                            ClientAddresses.Address changeAd = db.Address.Where(a => a.Id == address.Id).FirstOrDefault();
                            changeAd.Office = address.Office;
                            changeAd.Person = address.Person;
                            changeAd.Street = address.Street;
                            changeAd.Building = address.Building;
                            changeAd.CityId = db.City.Where(c => c.NameCity == address.City).FirstOrDefault().Id;
                            db.SaveChanges();
                        }
                        DownloadInfoToLists();
                    }
                }, (obj)=>SelectedAddress!=null);

            }
        }
                


    }
}
