using CarsCatalog.BBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarsCatalog.Models
{
    public class AdsInfoViewModel
    {
        public int AdsId { get; set; }
        public int VehicleType { get; set; }
        public string VehicleTypeDescription { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public Nullable<int> MileageKM { get; set; }
        public Nullable<decimal> Price { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public ICollection<ImageDisplayModel> Images { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
    }
}