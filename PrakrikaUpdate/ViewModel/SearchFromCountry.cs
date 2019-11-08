using DateBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PrakrikaUpdate.ViewModel
{
    class SearchFromCountryVM:INotifyPropertyChanged
    {
        private ObservableCollection<Country> countries;
        private ObservableCollection<Region> regions;
        private ObservableCollection<string> listSource;
        private string text;
        public string Text { get { return text; } set { text = value; OnPropertyChanged("Text"); } }
        public List<string> NameRegions { get; set; }
        public ObservableCollection<string> ListSource { get { return listSource; } set { listSource = value; OnPropertyChanged(nameof(ListSource)); } }

        public List<string> WhatFind { get; set; }
        public string Selected { get; set; }
        public string Selected2 { get; set; }
        public int Item { get; set; }
        public ObservableCollection<Country> Countries { get { return countries; } set { countries = value; OnPropertyChanged(nameof(Countries)); } }
        public ObservableCollection<Region> Regions { get { return regions; } set { regions = value; OnPropertyChanged(nameof(Regions)); } }
        public ObservableCollection<City> Cities { get; set; }
        public ObservableCollection<Address> Addresses { get; set; }
        public SearchFromCountryVM(ObservableCollection<Country> countries, ObservableCollection<Region> regions, ObservableCollection<City> cities, ObservableCollection<Address> addresses)
        {
            this.Cities = cities;
            this.Countries = countries;
            this.Regions = regions;
            this.Addresses = addresses;
            Text = "По какой стране";
            NameRegions = Countries.Select(s => s.FullName).ToList();
            Selected = NameRegions.FirstOrDefault();
            WhatFind = new List<string>()
            {
                "Регион",
                "Город",
                "Клиента"
            };
            Selected2 = "Регион";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Commander Search
        {
            get
            {
                return new Commander((onj) =>
                {
                    ListSource = new ObservableCollection<string>();
                    switch (Item)
                    {
                        case 0:
                            var result2 = Regions.Where(e => e.Country.FullName == Selected).ToList();
                            foreach (var res in result2)
                            {
                                ListSource.Add(res.ToString());
                            }
                            break;
                        case 1:
                            var result3 = Cities.Where(e => e.Region.Country.FullName == Selected).ToList();
                            foreach (var res in result3)
                            {
                                ListSource.Add(res.ToString());
                            }
                            break;
                        case 2:
                            var result4 = Addresses.Where(e => e.City.Region.Country.FullName == Selected).ToList();
                            foreach (var res in result4)
                            {
                                ListSource.Add(res.ToString());
                            }
                            break;
                    }
                }, (obj) => true);
            }
        }
    }
}
