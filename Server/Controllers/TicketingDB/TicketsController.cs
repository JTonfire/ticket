using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ITTicketingProject.Server.Controllers.TicketingDB
{
    [Route("odata/TicketingDB/Tickets")]
    public partial class TicketsController : ODataController
    {
        private ITTicketingProject.Server.Data.TicketingDBContext context;

        public TicketsController(ITTicketingProject.Server.Data.TicketingDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ITTicketingProject.Server.Models.TicketingDB.Ticket> GetTickets()
        {
            var items = this.context.Tickets.AsQueryable<ITTicketingProject.Server.Models.TicketingDB.Ticket>();
            this.OnTicketsRead(ref items);

            return items;
        }

        partial void OnTicketsRead(ref IQueryable<ITTicketingProject.Server.Models.TicketingDB.Ticket> items);
    }
}
