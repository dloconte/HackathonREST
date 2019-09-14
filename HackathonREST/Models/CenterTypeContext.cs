using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HackathonREST.Models
{
    public class CenterTypeContext : DbContext
    {
        public CenterTypeContext(DbContextOptions<CenterTypeContext> options) : base(options)
        {
            
        }

        public DbSet<CenterType> CenterTypes { get; set; }

    }
}
