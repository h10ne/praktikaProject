using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Praktika;
namespace PrakrikaUpdate
{
    public class XMLClass
    {
        ObservableCollection<Country> Countries;
        ObservableCollection<Region> Regions;
        ObservableCollection<City> Cities;
        ObservableCollection<Address> Addresses;
        public void DownloadDatas(ObservableCollection<Country> Countries, ObservableCollection<Region> Regions, ObservableCollection<City> Cities, ObservableCollection<Address> Addresses)
        {
            this.Addresses = new ObservableCollection<Address>(Addresses);
            this.Regions = new ObservableCollection<Region>(Regions);
            this.Cities = new ObservableCollection<City>(Cities);
            this.Countries = new ObservableCollection<Country>(Countries);
        }

    }
}
