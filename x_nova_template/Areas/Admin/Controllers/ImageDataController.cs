using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using x_nova_template.Service.Interface;

namespace x_nova_template.Areas.Admin.Controllers
{
    public class ImageDataController : Controller
    {
        //
        // GET: /Admin/ImageData/
        IProductRepository _pRep;
        IPhotoGallery _gRep;
        IPostRepository _postRep;

        public ImageDataController(IProductRepository pRep, IPhotoGallery gRep, IPostRepository postRep)
        {
            _pRep = pRep;
            _gRep = gRep;
            _postRep = postRep;
        }

        public void GetProdImage(int width = 0, int height = 0, int pid = 0, int pimgid = 0, string type = null)
        {
            // System.Threading.Thread.Sleep(2000);
            if (pid != 0)
            {
                var hasPreview = _pRep.GetPreviewImg(pid);

                if (hasPreview && _pRep.Get(pid).ProdImages.SingleOrDefault(x => x.IsPreview == 1).ImageDataType != null)
                {
                    if (width == 0 && height == 0)
                    {
                        new WebImage(_pRep.Get(pid).ProdImages.SingleOrDefault(x => x.IsPreview == 1).ImageDataType)
                            //.Resize(width, height, false, true) // Resizing the image to 100x100 px on the fly...

                            .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                            .Write();
                    }
                    else
                    {
                        new WebImage(_pRep.Get(pid).ProdImages.SingleOrDefault(x => x.IsPreview == 1).ImageDataType)
                           .Resize(width, height, true, true) // Resizing the image to 100x100 px on the fly...
                           .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                           .Write();
                    }
                }
                else
                {

                    new WebImage(Server.MapPath("/Content/main-images/photouser1.jpg"))
                        .Resize(width, height, false, true)
                        .Crop(1, 1).Write();

                }
            }
            else
            {
                var prodImg = _pRep.GetImg(pimgid);
                var prodId = prodImg.ProductID;
                if (prodImg.ImageDataType != null)
                {
                    if (type == "small")
                    {

                        new WebImage(prodImg.ImageDataType)
                           .Resize(100, 100, false) // Resizing the image to 100x100 px on the fly...
                           .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                           .Write();
                    }
                    else if (width == 0 && type == null)
                    {
                        new WebImage(prodImg.ImageDataType)
                            //.Resize(width, height, false, true) // Resizing the image to 100x100 px on the fly...                      
                          .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                          .Write();
                    }
                    else
                    {

                        new WebImage(prodImg.ImageDataType)
                           .Resize(width, height, true, true) // Resizing the image to 100x100 px on the fly...
                           .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                           .Write();

                    }
                }

            }

        }
        public void GetImgAsFile(string src, int width = 0, int height = 0, int t = 0)
        {

            //var folder = "~/Content/Files/";
            //var path = Path.Combine(folder, src);

            if (t == 1)
            {
                new WebImage(src)
                      .Resize(width + 20, height + 20, false, true)
                      .Crop(1, 1)
                      .Write();
            }
            else if (t == 2)
            {
                new WebImage(src)
                         .Resize(width, height, true, true)
                         .Crop(1, 1)
                         .Write();
            }
            else
            {
                new WebImage(src)
                .Crop(1, 1)
                       .Write();
                //var src1 = new Uri(src).AbsolutePath;

                //if (width != 0 && height != 0)
                //{
                //    new WebImage("~/" + src1)
                //        .Resize(width, height, true, true)
                //        .Crop(1, 1)
                //        .Write();
                //}
                //else
                //{
                //    new WebImage(src)
                //    .Crop(1, 1)
                //    .Write();
                //}
            }
        }
        public JsonResult Giu(string folder)
        {
            var path = Server.MapPath("~" + folder);

            var dir = new DirectoryInfo(path);
            var result = dir.GetFiles().Select(x => string.Concat(folder, x.Name)).ToArray();
            return Json(new { res = result }, JsonRequestBehavior.AllowGet);
        }

        public void GetPostImage(int id, int width = 0, int height = 0)
        {
            var item = _postRep.Get(id);

            if (item.PreviewPhoto != null && width > 0)
            {

                //var halfHeigth = new WebImage(item.PreviewPhoto).Height / 2;
                //var halfWidth = new WebImage(item.PreviewPhoto).Width / 2;
                var fileExt = new WebImage(item.PreviewPhoto).ImageFormat;


                new WebImage(item.PreviewPhoto)
                  .Resize(width, height)// Resizing the image to 100x100 px on the fly...
                  .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                  .Write();

            }
            else
            {
                new WebImage(item.PreviewPhoto)
                    // Resizing the image to 100x100 px on the fly...
                 .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                 .Write();
            }
        }


        public void GetPhotoGalleryImage(int id, int width = 0, int height = 0, bool isGallery = true)
        {

            if (!isGallery)
            {
                var item = _gRep.GetImage(id);

                if (item.ImageData != null && width > 0)
                {
                    //var halfHeight = height;
                    //var halfWidth = width;
                    //if (width > 1280)
                    //{
                    //    halfHeight = new WebImage(item.ImageData).Height / 2;
                    //    halfWidth = new WebImage(item.ImageData).Width / 2;
                    //}
                    //var fileExt = new WebImage(item.ImageData).ImageFormat;


                    new WebImage(item.ImageData)
                      .Resize(width, height, true, true)// Resizing the image to 100x100 px on the fly...
                      .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                      .Write();

                }
                else if (width == 0 && height == 0 && item.ImageData != null)
                {
                    new WebImage(item.ImageData)
                        .Resize(800, 600)// Resizing the image to 100x100 px on the fly...
                        .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                        .Write();
                }
                else
                {

                    //var file = new FileInfo(Server.MapPath("/Content/main-images/photouser1.jpg"));
                    new WebImage(Server.MapPath("/Content/main-images/photouser1.jpg")).Resize(width, height, false, true).Crop(1, 1).Write();

                }

            }
            else
            {
                var item = _gRep.GetGallery(id);

                if (item.GalleryData != null)
                {
                    if (width == 0)
                    {
                        new WebImage(item.GalleryData)
                        .Crop(1, 1).Write();
                    }
                    else
                    {
                        var halfHeight = height;
                        var halfWidth = width;
                        //if (width > 1600)
                        //{
                        //    halfHeight = new WebImage(item.GalleryData).Height / 2;
                        //    halfWidth = new WebImage(item.GalleryData).Width / 2;
                        //}
                        var fileExt = new WebImage(item.GalleryData).ImageFormat;
                        var img = new WebImage(item.GalleryData)

                          .Resize(halfWidth, halfHeight, false, false)

                            // Resizing the image to 100x100 px on the fly...
                          .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                          .Write();
                    }

                }
                else
                {

                    //var file = new FileInfo(Server.MapPath("/Content/main-images/photouser1.jpg"));
                    new WebImage(item.GalleryData)
                        .Crop(1, 1).Write();


                }
            }
        }
    }
}
