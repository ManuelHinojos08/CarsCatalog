using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsCatalog.BBL.Models
{
    public class ImageModel : DDL.Images
    {
        public string PresignedUrl { get; set; }
    }

    public class ImageSaveModel : DDL.Images
    {
        public MemoryStream FileToUpload { get; set; }
    }

    public class ImageDisplayModel
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string PreSignedUrl { get; set; }
    }
}
