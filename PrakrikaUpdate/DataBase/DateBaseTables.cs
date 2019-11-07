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

    }
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string NameCity { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public List<Address> Address { get; set; }
    }
    public class Region
    {
        [Key]
        public int Id { get; set; }
        public string NameRegion { get; set; }
        public int CountryId { get; set; }
        public List<City> City { get; set; }        
        public Country Country { get; set; }
    }
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public List<Region> Region { get; set; }
    }

}
