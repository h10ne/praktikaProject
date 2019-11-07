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
        public string City { get { return this.city; } set { this.city = value; } }
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

   
}
