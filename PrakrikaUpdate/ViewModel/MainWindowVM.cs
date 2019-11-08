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
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using DateBase;
using PrakrikaUpdate.Model;

namespace PrakrikaUpdate.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private MainWindowModel _model;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Country> countries;
        private ObservableCollection<Region> regions;
        private ObservableCollection<City> cities;
        private ObservableCollection<Address> addresses;
        public ObservableCollection<Country> Countries
        {
            get
            { return countries; }
            set { countries = value; OnPropertyChanged("Countries"); }
        }
        public ObservableCollection<Region> Regions
        {
            get { return regions; }
            set { regions = value; OnPropertyChanged("Regions"); }
        }
        public ObservableCollection<City> Cities
        {
            get { return cities; }
            set { cities = value; OnPropertyChanged("Cities"); }
        }
        public ObservableCollection<Address> Addresses
        {
            get { return addresses; }
            set { addresses = value; OnPropertyChanged("Addresses"); }
        }

        public void DownloadInfoToLists()
        {
            Countries = new ObservableCollection<Country>();
            Regions = new ObservableCollection<Region>();
            Cities = new ObservableCollection<City>();
            Addresses = new ObservableCollection<Address>();
            _model.DownloadInfoToLists(ref countries, ref regions, ref cities, ref addresses);
            OnPropertyChanged(nameof(Countries));
            OnPropertyChanged(nameof(Regions));
            OnPropertyChanged(nameof(Cities));
            OnPropertyChanged(nameof(Addresses));
        }
        public MainWindowVM(MainWindowModel model)
        {
            _model = model;
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
                        _model.AddCountry(c);
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
                    _model.DeleteCountry(SelectedCountry);
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
                        _model.ChangeCountry(selC);
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
                    vcountry.CountryBox.ItemsSource = Countries.Select(c => c.FullName);
                    try
                    {
                        if (vcountry.ShowDialog() == true)
                        {

                            _model.AddRegion(r.NameRegion, vcountry.CountryBox.SelectedItem.ToString());
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
                    _model.DeleteRegion(SelectedRegion);
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
                    newRegion.CountryBox.SelectedItem = SelectedRegion.Country.FullName;
                    if (newRegion.ShowDialog() == true)
                    {
                        _model.ChangeRegion(SelectedRegion, selR.NameRegion, newRegion.CountryBox.SelectedItem.ToString());
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
                    WindowNewCity winCity = new WindowNewCity();
                    City city = new City();
                    winCity.DataContext = city;
                    winCity.RegionBox.ItemsSource = Regions.Select(c => c.NameRegion);
                    if (winCity.ShowDialog() == true)
                    {
                        var newCity = new City()
                        {
                            NameCity = city.NameCity,
                            RegionId = Regions.Where(r => r.NameRegion == winCity.RegionBox.SelectedItem.ToString()).FirstOrDefault().Id
                        };
                        _model.AddCity(newCity);
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
                    WindowNewCity cityWin = new WindowNewCity()
                    {
                        Title = "Изменить город"
                    };
                    var city = SelectedCity;
                    cityWin.DataContext = city;
                    cityWin.RegionBox.ItemsSource = Regions.Select(c => c.NameRegion);
                    cityWin.RegionBox.SelectedItem = SelectedCity.Region.NameRegion;
                    if (cityWin.ShowDialog() == true)
                    {
                        var newCity = city;
                        newCity.RegionId = Regions.Where(r => r.NameRegion == cityWin.RegionBox.SelectedItem.ToString()).FirstOrDefault().Id;
                        _model.ChangeCity(newCity);
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
                    using (var db = new Context())
                    {
                        City city = db.City.Where(c => c.Id == selectedCity.Id).FirstOrDefault();
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
                    if (newAddress.ShowDialog() == true)
                    {
                        using (var db = new Context())
                        {
                            db.Address.Add(new Address()
                            {
                                Person = address.Person,
                                Building = address.Building,
                                Office = address.Office,
                                Street = address.Street,
                                CityId = db.City.Where(c => c.NameCity == newAddress.CityBox.SelectedItem.ToString()).FirstOrDefault().Id
                            }); ;
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
                    using (var db = new Context())
                    {
                        Address address = db.Address.Where(a => a.Id == SelectedAddress.Id).FirstOrDefault();
                        db.Address.Attach(address);
                        db.Address.Remove(address);
                        db.SaveChanges();
                    }
                    DownloadInfoToLists();
                }, (obj) => selectedAddress != null);
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
                    newAddress.CityBox.SelectedItem = SelectedAddress.City.NameCity;
                    if (newAddress.ShowDialog() == true)
                    {
                        using (var db = new Context())
                        {
                            Address changeAd = db.Address.Where(a => a.Id == address.Id).FirstOrDefault();
                            changeAd.Office = address.Office;
                            changeAd.Person = address.Person;
                            changeAd.Street = address.Street;
                            changeAd.Building = address.Building;
                            changeAd.CityId = db.City.Where(c => c.NameCity == newAddress.CityBox.SelectedItem.ToString()).FirstOrDefault().Id;
                            db.SaveChanges();
                        }
                        DownloadInfoToLists();
                    }
                }, (obj) => SelectedAddress != null);

            }
        }

        Task taskSaveToXml;

        public Commander SaveToXml
        {
            get
            {
                return new Commander((obj) =>
                {
                    taskSaveToXml = new Task(Serialize);
                    taskSaveToXml.Start();



                });
            }
        }


        private void Serialize()
        {
            try
            {
                int cou = 1;
                foreach (var c in countries)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Country));
                    serializer.Serialize(new StreamWriter($"Country {cou}.xml"), c);
                    cou++;
                }
                int r = 1;
                foreach (var c in Regions)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Region));
                    serializer.Serialize(new StreamWriter($"Region {r}.xml"), c);
                    r++;
                }
                int ci = 1;
                foreach (var c in Cities)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(City));
                    serializer.Serialize(new StreamWriter($"City {ci}.xml"), c);
                    ci++;
                }
                int ad = 1;
                foreach (var c in Addresses)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Address));
                    serializer.Serialize(new StreamWriter($"Address {ad}.xml"), c);
                    ad++;
                }
                MessageBox.Show("Успешно сохранено!", "Сохранено!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Cохранить не удалось!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Commander SearchFromRegions
        {
            get
            {
                return new Commander((obj) =>
                {
                    var view = new SearchFromRegionsView();
                    var model = new SearchFromRegionsVM(countries, regions, cities, addresses);
                    view.DataContext = model;
                    view.Show();
                });
            }
        }
        public Commander SearchFromCity
        {
            get
            {
                return new Commander((obj) =>
                {
                    var view = new SearchFromRegionsView();
                    var model = new SearchFromCityVM(countries, regions, cities, addresses);
                    view.DataContext = model;
                    view.Show();
                });
            }
        }
        public Commander SearchFromCountry
        {
            get
            {
                return new Commander((obj) =>
                {
                    var view = new SearchFromRegionsView();
                    var model = new SearchFromCountryVM(countries, regions, cities, addresses);
                    view.DataContext = model;
                    view.Show();
                });
            }
        }

    }
}
