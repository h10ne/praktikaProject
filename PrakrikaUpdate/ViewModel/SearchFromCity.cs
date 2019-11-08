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
    class SearchFromCityVM:INotifyPropertyChanged
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
        public SearchFromCityVM(ObservableCollection<Country> countries, ObservableCollection<Region> regions, ObservableCollection<City> cities, ObservableCollection<Address> addresses)
        {
            this.Cities = cities;
            this.Countries = countries;
            this.Regions = regions;
            this.Addresses = addresses;
            Text = "По какому городу";
            NameRegions = Cities.Select(s => s.NameCity).ToList();
            Selected = NameRegions.FirstOrDefault();
            WhatFind = new List<string>()
            {
                "Клиента"
            };
            Selected2 = "Город";
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
                            var result2 = Addresses.Where(e => e.City.NameCity == Selected).ToList();
                            foreach (var res in result2)
                            {
                                ListSource.Add(res.ToString());
                            }
                            return;
                    }
                }, (obj) => true);
            }
        }
    }
}
