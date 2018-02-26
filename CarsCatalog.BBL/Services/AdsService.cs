using CarsCatalog.BBL.Models;
using CarsCatalog.DDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CarsCatalog.BBL.Services
{
    public class AdsService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<Ads> GetAdsList()
        {
            return this.unitOfWork.AdsRepository.Get();
        }

        public IEnumerable<Ads> GetAdsListByUser(int adOwner)
        {
            return this.unitOfWork.AdsRepository.Get().Where(x => x.UserId == adOwner);
        }

        public Ads GetAd(int id)
        {
            //Ads ad = this.unitOfWork.AdsRepository.Get(id);
            //ImagesService iService = new ImagesService();
            //ad.Images = iService.GetImagesList(ad.AdsId).ToList();
            return this.unitOfWork.AdsRepository.Get(id);
        }

        public SaveMessagesModel AddAd(Ads ad)
        {
            bool saved = true;
            SaveMessagesModel savedModel = new SaveMessagesModel();            
            savedModel.ErrorMessage = this.unitOfWork.AdsRepository.Add(ad, out saved);
            savedModel.Saved = saved;           
            return savedModel;
        }

        public SaveMessagesModel AddAd(Ads ad, out int id)
        {
            bool saved = true;
            SaveMessagesModel savedModel = new SaveMessagesModel();            
            savedModel.ErrorMessage = this.unitOfWork.AdsRepository.Add(ad, out saved, out id);
            savedModel.Saved = saved;
            return savedModel;
        }

        public SaveMessagesModel RemoveAd(int adId)
        {
            bool saved = false;
            return new SaveMessagesModel()
            {
                ErrorMessage = this.unitOfWork.AdsRepository.Remove(adId, out saved),
                Saved = saved
            };
        }

        public SaveMessagesModel UpdatedAd(Ads ad)
        {
            bool saved = false;
            return new SaveMessagesModel()
            {
                ErrorMessage = this.unitOfWork.AdsRepository.UpdateAd(ad, out saved),
                Saved = saved
            };
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }        
    }
}
