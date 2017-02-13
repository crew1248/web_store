using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.Service.Interface
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }

        void Create(Post post);
        void RemoveDir(string path);
        Post Get(int id);
        void Delete(Post post);
        void Edit(Post post);
        void SavePhoto(HttpPostedFileBase file, int pid, bool isWM);
        void PhotoDel(string src);
    }
}