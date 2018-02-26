using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarsCatalog.Models
{
    public abstract class AdsBindingModels
    {
        [Required]
        [JsonProperty("VehicleType")]
        [Display(Name = "VehicleType")]
        public int VehicleType { get; set; }        

        [Required]
        [JsonProperty("Brand")]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Required]
        [JsonProperty("Model")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required]
        [JsonProperty("Year")]
        [Display(Name = "Year")]
        public int Year { get; set; }
        
        [JsonProperty("MileageKM")]
        [Display(Name = "MileageKM")]
        public Nullable<int> MileageKM { get; set; }
        
        [JsonProperty("Price")]
        [Display(Name = "Price")]
        public Nullable<decimal> Price { get; set; }

        [Required]
        [JsonProperty("UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [Display(Name = "Images")]
        [JsonProperty("Images")]
        public ICollection<AdImageModel> Images { get; set; }

        [Required]
        [JsonProperty("Estado")]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Required]
        [JsonProperty("Municipio")]
        [Display(Name = "Municipio")]
        public string Municipio { get; set; }

    }

    public class NewAdBindingModel : AdsBindingModels
    {
        
    }

    public class EditAdsBindingModel : AdsBindingModels
    {
        [Required]
        [JsonProperty("AdsId")]
        [Display(Name = "AdsId")]
        public int AdsId { get; set; }

        [Required]
        [JsonProperty("EditingUserId")]
        [Display(Name = "EditingUserId")]
        public int EditingUserId { get; set; }

        [JsonProperty("ImagesToDelete")]
        [Display(Name = "ImagesToDelete")]
        public ICollection<int> ImagesToDelete { get; set; }
    }

    public class DeleteAdsBindingModel
    {
        [Required]
        [JsonProperty("AdsId")]
        [Display(Name = "AdsId")]
        public int AdsId { get; set; }

        [Required]
        [JsonProperty("EditingUserId")]
        [Display(Name = "EditingUserId")]
        public int EditingUserId { get; set; }
    }

    public class AdImageModel
    {
        public string FileName { get; set; }

        public string FileStream { get; set; }
    }
}