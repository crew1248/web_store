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

        void Create(Post post,HttpPostedFileBase file=null);

        Post Get(int id);
        void Delete(Post post);
        void Edit(Post post,HttpPostedFileBase file=null);
    }
}