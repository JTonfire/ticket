using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ITTicketingProject.Server.Models.TicketingDB;

namespace ITTicketingProject.Server.Data
{
    public partial class TicketingDBContext : DbContext
    {
        public TicketingDBContext()
        {
        }

        public TicketingDBContext(DbContextOptions<TicketingDBContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<ITTicketingProject.Server.Models.TicketingDB.Ticket> Tickets { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}