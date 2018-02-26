using CarsCatalog.BBL.Models;
using CarsCatalog.BBL.Services;
using CarsCatalog.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CarsCatalog.Controllers
{
    [RoutePrefix("api/Ads")]
    [BasicAuthentication]
    public class AdsController : ApiController
    {
        // GET: api/Ads
        [Route("GetAdsList")]
        [HttpGet]
        public IHttpActionResult GetAdsList()
        {            
            AdsService aService = new AdsService();
            var adsList = aService.GetAdsList().Select(x => new AdsInfoViewModel()
            {
                UserId = x.UserId,
                User = x.Users.UserName,
                VehicleType = x.VehicleType,
                VehicleTypeDescription = x.VehicleTypes.Description,
                Brand = x.Brand,
                Model = x.Model,
                Year = x.Year,
                MileageKM = x.MileageKM,
                Price = x.Price,
                Images = AWSImagesService.GetImagesListUrls(x.Images),
                Estado = x.Estado,
                Municipio = x.Municipio
            }).ToList();
            aService.Dispose();
            return Json(adsList);
        }

        // GET: api/Ads
        [Route("GetAdsListByUser")]
        [HttpPost]
        public IHttpActionResult GetAdsListByUser(int userId)
        {
            AdsService aService = new AdsService();            
            var adsList = aService.GetAdsListByUser(userId).Select(x => new AdsInfoViewModel()
            {
                UserId = x.UserId,
                User = x.Users.UserName,
                VehicleType = x.VehicleType,
                VehicleTypeDescription = x.VehicleTypes.Description,
                Brand = x.Brand,
                Model = x.Model,
                Year = x.Year,
                MileageKM = x.MileageKM,
                Price = x.Price,
                Images = AWSImagesService.GetImagesListUrls(x.Images),
                Estado = x.Estado,
                Municipio = x.Municipio
            }).ToList();
            aService.Dispose();
            return Json(adsList);
        }

        [Route("GetAdInfo")]
        [HttpPost]
        public IHttpActionResult GetAdInfo(int adId)
        {
            AdsService aService = new AdsService();
            DDL.Ads ad = aService.GetAd(adId);
            AdsInfoViewModel result = new AdsInfoViewModel();
            if (ad != null)
            {
                result.UserId = ad.UserId;
                result.User = ad.Users.UserName;
                result.VehicleType = ad.VehicleType;
                result.VehicleTypeDescription = ad.VehicleTypes.Description;
                result.Brand = ad.Brand;
                result.Model = ad.Model;
                result.Year = ad.Year;
                result.MileageKM = ad.MileageKM;
                result.Price = ad.Price;
                result.Images = AWSImagesService.GetImagesListUrls(ad.Images);
                result.Estado = ad.Estado;
                result.Municipio = ad.Municipio;
            }
            aService.Dispose();
            return Json(result);
        }

        [Route("CreateAd")]
        [HttpPost]
        public IHttpActionResult CreateAd(NewAdBindingModel newAd)
        {

            if (ModelState.IsValid)
            {
                List<ImageSaveModel> ImagesToUpload = new List<ImageSaveModel>();
                ImagesService iService = new ImagesService();
                Guid id;
                int adId;

                SaveMessagesModel adSavedModel = new SaveMessagesModel();

                /*Save images on S3*/
                if (newAd.Images != null)
                {
                    foreach (AdImageModel img in newAd.Images)
                    {
                        id = Guid.NewGuid();
                        ImagesToUpload.Add(new ImageSaveModel
                        {
                            FileName = img.FileName,
                            FileToUpload = iService.Base64ToImage(img.FileStream),
                            FileReference = id.ToString()
                        });
                    }
                }
                SaveMessagesModel iSavedModel = new SaveMessagesModel();                
                foreach (ImageSaveModel sm in ImagesToUpload)
                {
                    iSavedModel = AWSImagesService.UploadImages(sm);
                    if (!iSavedModel.Saved)
                    {
                        return Json(iSavedModel);
                    }
                }
                /*Save images on S3*/

                /*If there are no images or the images were succesfully uploaded*/
                if (ImagesToUpload.Count <= 0 || iSavedModel.Saved)
                {
                    /*Save the ad entity*/
                    AdsService aService = new AdsService();
                    DDL.Ads adToSave = new DDL.Ads()
                    {
                        UserId = newAd.UserId,
                        VehicleType = newAd.VehicleType,
                        Brand = newAd.Brand,
                        Model = newAd.Model,
                        MileageKM = newAd.MileageKM,
                        Price = newAd.Price,
                        Year = newAd.Year,
                        Estado = newAd.Estado,
                        Municipio = newAd.Municipio                        
                    };
                    adSavedModel = aService.AddAd(adToSave, out adId);
                    /*Save the ad entity*/

                    /*Save the images entities*/
                    if (adSavedModel.Saved)
                    {                        
                        foreach (ImageSaveModel img in ImagesToUpload)
                        {
                            iSavedModel = iService.AddImage(new DDL.Images {
                                AdsId = adId,
                                FileName = img.FileName,
                                FileReference = img.FileReference
                            });
                            if (!iSavedModel.Saved)
                            {
                                return Json(iSavedModel);
                            }
                        }
                        iService.Dispose();
                    }
                    /*Save the images entities*/
                    aService.Dispose();                    
                }
                /*If there are no images or the images were succesfully uploaded*/
                return Json(adSavedModel);
            }
            //If the model state is invalid means one of the required fields is empty
            else
            {
                var errorList = ModelState.ToDictionary(
                    kvp => kvp.Key.Replace("newAd.", ""),
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage)
                    );
                return Json(errorList);
            }
        }

        [Route("UpdateAds")]
        [HttpPost]
        public IHttpActionResult UpdateAds([FromBody]EditAdsBindingModel editAd)
        {
            AdsService aService = new AdsService();
            SaveMessagesModel savedModel = new SaveMessagesModel();
            if (ModelState.IsValid)
            {
                //Comprobation of the permissions
                UserService uService = new UserService();
                if (!uService.CanEditAds(editAd.EditingUserId, editAd.AdsId))
                {
                    return Json(new SaveMessagesModel()
                    {
                        Saved = false,
                        ErrorMessage = "Your user profile has not enough privileges"
                    });
                }

                /*Find the selected Ad*/
                DDL.Ads toUpdate = aService.GetAd(editAd.AdsId);
                if (toUpdate ==null)
                {
                    return Json(new SaveMessagesModel()
                    {
                        Saved = false,
                        ErrorMessage = "The selected ad was not found"
                    });
                }

                /*Set the new values to the ad*/
                toUpdate.VehicleType = editAd.VehicleType;
                toUpdate.Brand = editAd.Brand;
                toUpdate.Model = editAd.Model;
                toUpdate.MileageKM = editAd.MileageKM;
                toUpdate.Price = editAd.Price;
                toUpdate.Year = editAd.Year;
                toUpdate.UserId = editAd.UserId;
                toUpdate.Estado = editAd.Estado;
                toUpdate.Municipio = editAd.Municipio;
                /*Remove the deleted images*/
                if (editAd.ImagesToDelete != null && editAd.ImagesToDelete.Count() > 0)
                {                    
                    foreach (int i in editAd.ImagesToDelete)
                    {
                        savedModel = AWSImagesService.DeleteImages(toUpdate.Images.Where(x => x.ImageId == i).FirstOrDefault().FileReference);                        
                        toUpdate.Images.Remove(toUpdate.Images.Where(x => x.ImageId == i).FirstOrDefault());
                    }
                }

                if (editAd.Images != null && editAd.Images.Count > 0)
                {
                    List<ImageSaveModel> ImagesToUpload = new List<ImageSaveModel>();
                    ImagesService iService = new ImagesService();
                    Guid id;
                    int adId;

                    /*Save images on S3*/
                    foreach (AdImageModel img in editAd.Images)
                    {
                        id = Guid.NewGuid();
                        ImagesToUpload.Add(new ImageSaveModel
                        {
                            FileName = img.FileName,
                            FileToUpload = iService.Base64ToImage(img.FileStream),
                            FileReference = id.ToString()
                        });
                    }
                    SaveMessagesModel iSavedModel = new SaveMessagesModel();
                    foreach (ImageSaveModel sm in ImagesToUpload)
                    {
                        iSavedModel = AWSImagesService.UploadImages(sm);
                        if (!iSavedModel.Saved)
                        {
                            return Json(iSavedModel);
                        }
                    }
                    savedModel = aService.UpdatedAd(toUpdate);
                    aService.Dispose();

                    /*Save the images entities*/
                    if (savedModel.Saved)
                    {
                        foreach (ImageSaveModel img in ImagesToUpload)
                        {
                            iSavedModel = iService.AddImage(new DDL.Images
                            {
                                AdsId = toUpdate.AdsId,
                                FileName = img.FileName,
                                FileReference = img.FileReference
                            });
                            if (!iSavedModel.Saved)
                            {
                                return Json(iSavedModel);
                            }
                        }
                        iService.Dispose();
                    }
                    /*Save the images entities*/
                }

                return Json(savedModel);
            }
            else
            {
                var errorList = ModelState.ToDictionary(
                    kvp => kvp.Key.Replace("editAd.", ""),
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage)
                    );
                aService.Dispose();
                return Json(errorList);
            }
        }

        [Route("DeleteAdd")]
        [HttpPost]
        public IHttpActionResult DeleteAdd([FromBody]DeleteAdsBindingModel editAd)
        {
            
            if (ModelState.IsValid)
            {                
                //Comprobation of the permissions
                UserService uService = new UserService();
                if (!uService.CanEditAds(editAd.EditingUserId, editAd.AdsId))
                {
                    return Json(new SaveMessagesModel()
                    {
                        Saved = false,
                        ErrorMessage = "Your user profile has not enough privileges"
                    });
                }
                AdsService aService = new AdsService();
                /*Look for the selected ad*/
                DDL.Ads toDelete = aService.GetAd(editAd.AdsId);
                if (toDelete == null)
                {
                    return Json(new SaveMessagesModel()
                    {
                        Saved = false,
                        ErrorMessage = "The selected ad was not found"
                    });
                }                
                SaveMessagesModel savedModel = new SaveMessagesModel();

                /*Delete the ad images from S3 and from the DB*/
                ImagesService iService = new ImagesService();
                ICollection<DDL.Images> imgToDelete = toDelete.Images.ToList();
                foreach (DDL.Images img in imgToDelete)
                {
                    savedModel = AWSImagesService.DeleteImages(img.FileReference);
                    if (!savedModel.Saved)
                    {
                        return Json(savedModel);
                    }
                    savedModel = iService.RemoveImage(img);
                    if (!savedModel.Saved)
                    {
                        return Json(savedModel);
                    }
                    toDelete.Images.Remove(img);
                }
                aService.Dispose();
                aService = new AdsService();
                savedModel = aService.RemoveAd(editAd.AdsId);
                aService.Dispose();
                return Json(savedModel);
            }
            else
            {
                var errorList = ModelState.ToDictionary(
                    kvp => kvp.Key.Replace("editAd.", ""),
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage)
                    );                
                return Json(errorList);
            }
        }
    }
}      
