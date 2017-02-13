using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

namespace x_nova_template.Service.Repository
{
    public class PostRepository : IPostRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private FileManager filemanager = new FileManager();

        public IQueryable<Post> Posts { get { return db.Posts; } }

        public void Create(Post post)
        {

            //WebImage formImg = null;
            //byte[] imgBytes = null;
            //if (file.ContentLength > 1000000)
            //{
            //    formImg = new WebImage(file.InputStream);
            //    imgBytes = formImg.Resize(formImg.Width / 2, formImg.Height / 2).GetBytes();
            //    post.PreviewPhoto = imgBytes;
            //}
            //else post.PreviewPhoto = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);

            db.Posts.Add(post);
            db.SaveChanges();
        }

        public Post Get(int id)
        {
            var item = db.Posts.Find(id);
            return item;
        }
        public void Edit(Post post)
        {


            var item = Get(post.ID);
            item.CreatedAt = post.CreatedAt;
            item.Title = post.Title;
            item.Body = post.Body;
            item.Preview = post.Preview;

            db.SaveChanges();
        }
        public void Delete(Post post)
        {


            db.Posts.Remove(post);
            db.SaveChanges();
        }


        public void SavePhoto(HttpPostedFileBase file, int pid, bool isWM = false)
        {

            if (file.ContentLength > 4000000) throw new HttpException();

            int imagesCount = 0;
            var updateSort = Get(pid).Sortindex + 1;
            Get(pid).Sortindex = updateSort;
            db.SaveChanges();
            var dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Post/" + pid);
            var dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Post/" + pid + "/200x150");
            //watermark filename and path
            if (isWM)
            {
                var dirPathWM = HttpContext.Current.Server.MapPath("~/Content/watermark");
                filemanager.CheckDirectory(dirPathWM);
                var wmName = filemanager.GetRandomName();
                var wmPath = Path.Combine(dirPathWM, wmName + Path.GetExtension(file.FileName));
                var istream = new WebImage(file.InputStream).Resize(1920, 1080, true, true).GetBytes();
                filemanager.WriteImage(istream, wmPath, isWM);
            }
            else
            {
                // without watermark photos
                imagesCount = filemanager.CheckDirectory(dirPath);
                var rndName = filemanager.GetRandomName(Get(pid).Sortindex);
                var filePath = Path.Combine(dirPath, rndName + Path.GetExtension(file.FileName));

                imagesCount = filemanager.CheckDirectory(dirPaths);
                var filePaths = Path.Combine(dirPaths, rndName + Path.GetExtension(file.FileName));
                var istream = new WebImage(file.InputStream).Resize(1920, 1080, true, true).GetBytes();

                filemanager.WriteImage(istream, filePath);
                istream = new WebImage(istream).Resize(200, 150, false, true).GetBytes();
                filemanager.WriteImage(istream, filePaths);
            }
        }
        public void PhotoDel(string src)
        {
            filemanager.RemoveFile(src);

        }
        public void RemoveDir(string src)
        {
            filemanager.RemoveDir(src);

        }
    }
}