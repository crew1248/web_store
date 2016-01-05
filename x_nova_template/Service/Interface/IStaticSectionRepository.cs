using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.Service.Interface
{
    public interface IStaticSectionRepository
    {
        IQueryable<StaticSection> StaticSections { get; }

        void Edit(StaticSection section);
        StaticSection GetSection(int type);
        StaticSection Get(int id);
        void Delete(StaticSection section);
    }
}