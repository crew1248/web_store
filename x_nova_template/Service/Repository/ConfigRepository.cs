using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

namespace x_nova_template.Service.Repository
{
    public class ConfigRepository:IConfigRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Config> Configs { get { return db.Configs; } }

        public void Edit(Config cnf)
        {
            db.Entry(cnf).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public Config Options() {
            return db.Configs.First();
        }
    }
}