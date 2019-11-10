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
    class SpecSearchVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Country> Countries { get; set; }
        public ObservableCollection<Region> Regions { get; set; }
        public ObservableCollection<City> Cities { get; set; }
        public ObservableCollection<Address> Addresses { get; set; }
        public String Street { get { return street; } set { street = value; OnPropertyChanged("Street"); } }

        private ObservableCollection<string> listSource;
        private string street;

        public ObservableCollection<string> ListSource { get { return listSource; } set { listSource = value; OnPropertyChanged(nameof(ListSource)); } }

        public SpecSearchVM(ObservableCollection<Country> countries, ObservableCollection<Region> regions, ObservableCollection<City> cities, ObservableCollection<Address> addresses)
        {
            this.Cities = cities;
            this.Countries = countries;
            this.Regions = regions;
            this.Addresses = addresses;
        }

        public Commander firstBtn
        {
            get
            {
                return new Commander((onj) =>
                {
                    ListSource = new ObservableCollection<string>();
                    using (var db = new Context())
                    {
                        //var result = db.Region.Select(s => s.City.Count).Max();
                        var result = db.Region.Where(e => e.City.Count == db.Region.Select(s => s.City.Count).Max()).ToList();
                        foreach (var region in result)
                        {
                            region.Country = db.Country.First(c => c.Id == region.CountryId);
                            ListSource.Add(region.ToString());
                        }
                    }
                }, (obj) => true);
            }
        }

        public Commander scndBtn
        {
            get
            {
                return new Commander((onj) =>
                {
                    ListSource = new ObservableCollection<string>();
                    using (var db = new Context())
                    {
                        int max = db.City.Select(e => e.Region).Select(e => e.Country).Select(e => e.FullName).GroupBy(x => x).ToList().Select(h => h.Count()).Max();
                        var smth = db.City.Select(e => e.Region).Select(e => e.Country).Select(e => e.FullName).GroupBy(x => x).ToList();
                        var result = smth.Where(e => e.Count() == max).ToList();
                        foreach (var country in result)
                        {
                            ListSource.Add(country.Key);
                        }
                    }
                }, (obj) => true);
            }
        }

        public Commander thrdBtn
        {
            get
            {
                return new Commander((onj) =>
                {
                    ListSource = new ObservableCollection<string>();
                    using (var db = new Context())
                    {
                        var result = db.Address.Where(e => e.Street == Street).Select(e => e.City).Select(e => e.Region).Select(e => e.Country).ToList();
                        foreach (var res in result)
                        {
                            ListSource.Add(res.ToString());
                        }
                    }
                }, (obj) => true);
            }
        }

        public Commander frthBtn
        {
            get
            {
                return new Commander((onj) =>
                {
                    ListSource = new ObservableCollection<string>();
                    using (var db = new Context())
                    {
                        var result = db.Region.Where(e => e.City.Count == db.Region.Select(s => s.City.Count).Max()).ToList().Count;
                        ListSource.Add($"кол-во регионов с максимальным кол-вом городов - {result.ToString()} шутк(и)" );
                    }
                }, (obj) => true);
            }
        }

    }
}
