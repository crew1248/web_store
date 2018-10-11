using x_nova_template.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Security.AccessControl;

namespace x_nova_template.Models
{
    public class FileManager
    {

        IProductRepository _prodDb;
        IPostRepository _postDb;
        public FileManager() { }

        public FileManager(IProductRepository prodDb, IPostRepository postDb)
        {
            _prodDb = prodDb;
            _postDb = postDb;
        }

        private byte[] istream = null;
        private string filePath = null;
        private WebImage webimg = null;
        private int imagesCount = 0;
        private static Random random = new Random();
        public string mainDir = "~/Content/Files/Pages";

        public void WriteFiles(IEnumerable<HttpPostedFileBase> files, string path)
        {

            foreach (var file in files)
            {
                if (file.ContentLength > 4000000) throw new HttpException();

                imagesCount = CheckDirectory(path);

                filePath = Path.Combine(path, GetRandomName(imagesCount) + Path.GetExtension(file.FileName));


                if ((file.ContentLength > 1000000 && file.ContentType == "image/png") || (file.ContentLength > 1000000 && file.ContentType == "image/jpeg"))
                {
                    webimg = new WebImage(file.InputStream);
                    istream = webimg.Resize(webimg.Width / 2, webimg.Height / 2).GetBytes();

                }
                else istream = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);

                using (MemoryStream ms = new MemoryStream(istream))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {

                        ms.WriteTo(fs);
                    }
                }
            }
        }
        public void WriteFile(HttpPostedFileBase file, string path)
        {
            if (file.ContentLength > 4000000) throw new HttpException();

            imagesCount = CheckDirectory(path);

            filePath = Path.Combine(path, GetRandomName(imagesCount) + Path.GetExtension(file.FileName));

            if ((file.ContentLength > 1000000 && file.ContentType == "image/png") || (file.ContentLength > 1000000 && file.ContentType == "image/jpeg"))
            {
                webimg = new WebImage(file.InputStream);
                istream = webimg.Resize(webimg.Width / 2, webimg.Height / 2).GetBytes();
            }
            else istream = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);

            using (MemoryStream ms = new MemoryStream(istream))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {

                    ms.WriteTo(fs);
                }
            }
        }
        public void WriteImage(byte[] buffer, string path, bool isWM = false)
        {

            var sn = System.Configuration.ConfigurationManager.AppSettings["SitePath"];
            //MemoryStream destination = new MemoryStream();
            if (isWM)
            {
                buffer = new WebImage(buffer).AddTextWatermark(sn, "black", 18, "Regular", "Impact", "Right", "Bottom", 50).GetBytes();
                buffer = new WebImage(buffer).AddTextWatermark(sn, "white", 18, "Regular", "Impact", "Left", "Bottom", 50).GetBytes();
            }
            //buffer = new WebImage(buffer).Resize(200, 150, false).GetBytes();
            using (MemoryStream ms = new MemoryStream(buffer))
            {

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    fs.CopyTo(ms);
                    ms.CopyTo(fs);

                }
            }

        }
        public int CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return 1;
            }
            else
            {

                if (Directory.GetFiles(path).Count() == 0) return 1;
                else
                {
                    return Directory.GetFiles(path).Count() + 1;


                }


            }

        }
        public string GetRandomName(int c = 1)
        {

            var str = Path.GetRandomFileName();

            string res = null;
            foreach (char ch in str)
            {
                if (!Char.IsLetter(ch)) res += RandomString(2);
                else res += ch;

            }

            res = c.ToString() + res;
            return res;
        }
        public void RemoveDir(string path)
        {
            if (Directory.Exists(path) && path != null)
            {
                Directory.Delete(path, true);
            }
        }

        public void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static string RandomString(int length)
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // DIRECTORIES

        public void CreateDir(string path)
        {


            var newpath = HttpContext.Current.Server.MapPath(mainDir + "/" + path);

            if (Directory.Exists(newpath))
            {
                DirectoryInfo di = Directory.CreateDirectory(newpath + "1");
            }
            else { DirectoryInfo di = Directory.CreateDirectory(newpath); }

            DirectoryInfo dir = new DirectoryInfo(newpath);
            DirectorySecurity security = dir.GetAccessControl();

            //security.AddAccessRule(new FileSystemAccessRule("everyone", FileSystemRights.Read, AccessControlType.Allow));
            //security.AddAccessRule(new FileSystemAccessRule("everyone", FileSystemRights.FullControl, AccessControlType.Allow));
            // security.AddAccessRule(new FileSystemAccessRule("everyone", FileSystemRights.FullControl, AccessControlType.Allow));

            //security.AddAccessRule(new FileSystemAccessRule("everyone", FileSystemRights.Modify, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            dir.SetAccessControl(security);
        }
        public IEnumerable<DirectoryInfo> GetDirs()
        {

            DirectoryInfo dinfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(mainDir));

            var dirs = dinfo.GetDirectories();
            return dirs;
        }
        public void ClearDir(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                File.Delete(file.FullName);
            }
        }
    }
}