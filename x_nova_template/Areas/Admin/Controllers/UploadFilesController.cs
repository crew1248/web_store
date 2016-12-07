
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using x_nova_template.ViewModel;

namespace x_nova_template.Areas.Admin.Controllers
{
    public class UploadFilesController : Controller
    {
        //
        // GET: /Admin/UploadFiles/

        public ActionResult GetFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/Content/Files"));
            if (!directory.Exists)
            {
                Directory.CreateDirectory(Server.MapPath("~/Content/Files"));

            }
            var allFiles = directory.GetFiles().Count();
            FilesViewModel vm = new FilesViewModel
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = allFiles
                }

            };
            return View(vm);
        }
        public const int FilesPerPage = 15;
        public ActionResult Index(int page = 1)
        {

            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/Content/Files"));
            var allFiles = directory.GetFiles().Count();
            var files = directory.GetFiles().Skip(FilesPerPage * (page - 1)).Take(FilesPerPage).ToList();

            FilesViewModel vm = new FilesViewModel
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = allFiles,
                    CurrentPage = page,
                    ItemsPerPage = FilesPerPage,
                    Service = "Files"
                },
                Files = files
            };
            return PartialView(vm);
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.ContentLength > 4000000) return Json(new { type = "length" }, "text/plain", JsonRequestBehavior.AllowGet);
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    string fileName = null;
                    string physicalPath = null;
                    physicalPath = Path.Combine(Server.MapPath("~/Content/Files"), file.FileName.ToLower());
                    if (file.ContentType == "image/png" || file.ContentType == "image/jpeg")
                    {

                        WebImage formImg = null;
                        byte[] imgBytes = null;
                        if (file.ContentLength > 1000000)
                        {
                            formImg = new WebImage(file.InputStream);
                            imgBytes = formImg.Resize(formImg.Width / 2, formImg.Height / 2).GetBytes();

                        }
                        else imgBytes = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
                        using (MemoryStream ms = new MemoryStream(imgBytes))
                        {
                            using (FileStream fs = new FileStream(physicalPath, FileMode.Create))
                            {
                                ms.WriteTo(fs);
                            }
                        }

                        var webImg = new WebImage(file.InputStream);
                        // random file name
                        //physicalPath = Path.Combine(Server.MapPath("~/Content/Files"),Path.GetRandomFileName());

                        // current file name
                        //physicalPath = Path.Combine(Server.MapPath("~/Content/Files"), file.FileName.ToLower());
                        //webImg
                        //    .Resize(1920, 1080)
                        //    .Crop(1, 1)
                        //    .Save(physicalPath);


                    }
                    else
                    {

                        using (MemoryStream ms = new MemoryStream(new BinaryReader(file.InputStream).ReadBytes(file.ContentLength)))
                        {
                            using (FileStream fs = new FileStream(physicalPath, FileMode.Create))
                            {
                                ms.WriteTo(fs);
                            }
                        }
                    }


                }

            }

            // Return an empty string to signify success
            return Content("");
        }


        [HttpPost]
        public ActionResult Delete(string fileName)
        {

            var file = Path.GetFileName(fileName);
            var physicalPath = Path.Combine(Server.MapPath("~/Content/Files"), file);
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
                Dispose();

            }
            return Json("");
        }

        public ActionResult Cancel(string fileName)
        {


            var file = Path.GetFileName(fileName);
            var physicalPath = Path.Combine(Server.MapPath("~/Content/Files"), file);
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);

            }

            return Json(Content(""), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Files"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                        Dispose();
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

    }
}
