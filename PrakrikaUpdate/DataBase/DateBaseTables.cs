using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateBase
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Person { get; set; }
        public string Street { get; set; }
        public int Building { get; set; }
        public int? Office { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public override string ToString()
        {
            return $"Id - {Id}, City - {City.NameCity}, Street - {Street}, Buildiing - {Building}, Office - {Office}, Person - {Person}";
        }

    }
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string NameCity { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public List<Address> Address { get; set; }
        public override string ToString()
        {
            return $"Id - {Id}, Name - {NameCity}, Region - {Region.NameRegion}";
        }
    }
    public class Region
    {
        [Key]
        public int Id { get; set; }
        public string NameRegion { get; set; }
        public int CountryId { get; set; }
        public List<City> City { get; set; }        
        public Country Country { get; set; }
        public override string ToString()
        {
            return $"Id - {Id}, Name - {NameRegion}, Country - {Country.FullName}";
        }
    }
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public List<Region> Region { get; set; }
        public override string ToString()
        {
            return $"Id - {Id}, FullName - {FullName}, ShortName - {ShortName}";
        }
    }

}
