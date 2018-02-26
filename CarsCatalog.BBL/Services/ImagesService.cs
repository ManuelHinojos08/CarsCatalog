using CarsCatalog.BBL.Models;
using CarsCatalog.DDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarsCatalog.BBL.Services
{
    public class ImagesService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<Images> GetImagesList()
        {
            return this.unitOfWork.ImagesRepository.Get();
        }

        public Images GetImage(int id)
        {
            return this.unitOfWork.ImagesRepository.Get(id);
        }

        public IEnumerable<Images> GetImagesList(int id)
        {
            return this.unitOfWork.ImagesRepository.GetAdsImages(id);
        }

        public SaveMessagesModel AddImage(Images image)
        {
            bool saved = false;
            return new SaveMessagesModel()
            {
                ErrorMessage = this.unitOfWork.ImagesRepository.Add(image, out saved),
                Saved = saved
            };         
        }

        public SaveMessagesModel RemoveImage(Images image)
        {
            bool saved = false;
            return new SaveMessagesModel()
            {
                ErrorMessage = this.unitOfWork.ImagesRepository.Remove(image, out saved),
                Saved = saved
            };            
        }

        public MemoryStream Base64ToImage(string base64String)
        {
            int stringStart = base64String.IndexOf(',') + 1;
            base64String = stringStart > 0 ? base64String.Substring(stringStart, (base64String.Length - stringStart)) : base64String;
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            //Image image = Image.FromStream(ms, true);           
            return ms;
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }


    }
}
