using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsCatalog.DDL.Repositories
{
    public class AdsRepository
    {
        private CarCatalogEntities context { get; set; }

        public AdsRepository(CarCatalogEntities context)
        {
            this.context = context;
        }

        public IEnumerable<Ads> Get()
        {
            return context.Ads.ToList();
        }

        public Ads Get(int id)
        {
            return context.Ads.Find(id);
        }       

        public string Add(Ads entity, out bool saved, out int id)
        {
            string Message = string.Empty;            
            try
            {
                context.Ads.Add(entity);
                context.SaveChanges();
                id = entity.AdsId;
                saved = true;
                return Message;
            }
            catch (Exception e)
            {
                Message = e.Message;
                id = -1;
                saved = false;
                return Message;
            }
        }

        public string Add(Ads entity, out bool saved)
        {
            string Message = string.Empty;
            try
            {
                context.Ads.Add(entity);
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

        public string Remove(int adId, out bool saved)
        {
            string Message = string.Empty;
            try
            {
                var obj = context.Ads.Find(adId);
                if (obj == null)
                {
                    Message = "Ad not found";
                    saved = false;
                    return Message;
                }
                context.Ads.Remove(obj);
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

        public string UpdateAd(Ads entity, out bool saved)
        {
            string Message = string.Empty;
            try
            {
                var obj = context.Ads.Find(entity.AdsId);
                if (obj == null)
                {
                    Message = "Ad not found";
                    saved = false;
                    return Message;
                }
                context.Entry(obj).CurrentValues.SetValues(entity);
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

        private IEnumerable<Images> GetImages(int id)
        {
            ImagesRepository imagesRepository = new ImagesRepository(context);
            IEnumerable<Images> images = imagesRepository.GetAdsImages(id);

            return images;
        }
    }
}
