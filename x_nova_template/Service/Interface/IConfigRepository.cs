using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.Service.Interface
{
   
    public interface IConfigRepository
    {
        IQueryable<Config> Configs { get; }

        void Edit(Config cnf);
        Config Options();
    }
}