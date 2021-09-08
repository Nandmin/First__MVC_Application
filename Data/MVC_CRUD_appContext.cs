using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC_CRUD_app.Models;

namespace MVC_CRUD_app.Data
{
    public class MVC_CRUD_appContext : DbContext
    {
        public MVC_CRUD_appContext (DbContextOptions<MVC_CRUD_appContext> options)
            : base(options)
        {
        }

        public DbSet<MVC_CRUD_app.Models.BookViewModel> BookViewModel { get; set; }
    }
}
