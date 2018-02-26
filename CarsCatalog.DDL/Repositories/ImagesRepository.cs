using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsCatalog.DDL.Repositories
{
    public class ImagesRepository
    {
        private CarCatalogEntities context { get; set; }

        public ImagesRepository(CarCatalogEntities context)
        {
            this.context = context;
        }

        public IEnumerable<Images> Get()
        {
            return context.Images.ToList();
        }

        public Images Get(int id)
        {
            return context.Images.Find(id);
        }

        public IEnumerable<Images> GetAdsImages(int id)
        {
            return context.Images.Where(x => x.AdsId == id).ToList();
        }

        public string Add(Images entity, out bool saved)
        {
            string Message = string.Empty;
            try
            {
                context.Images.Add(entity);
                context.SaveChanges();
                saved = true;
                return Message;
            }
            catch (Exception e)
            {
                Message = e.Message;
                saved = false;
                return Message;
            }          
        }

        public string Remove(Images entity, out bool saved)
        {
            string Message = string.Empty;
            try
            {
                var obj = context.Images.Find(entity.ImageId);
                context.Images.Remove(obj);
                context.SaveChanges();
                saved = true;
                return Message;
            }
            catch (Exception e)
            {
                Message = e.Message;
                saved = false;
                return Message;
            }            
        }
    }
}
