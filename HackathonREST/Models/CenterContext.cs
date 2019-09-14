using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HackathonREST.Models
{
    public class CenterContext : DbContext
    {
        public CenterContext(DbContextOptions<CenterContext> options) : base(options)
        {

        }

            public DbSet<Center> Centers { get; set; }
    }
        
}
