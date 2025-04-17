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

        partial void OnTicketGet(ref SingleResult<ITTicketingProject.Server.Models.TicketingDB.Ticket> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/TicketingDB/Tickets(TicketId={TicketId})")]
        public SingleResult<ITTicketingProject.Server.Models.TicketingDB.Ticket> GetTicket(int key)
        {
            var items = this.context.Tickets.Where(i => i.TicketId == key);
            var result = SingleResult.Create(items);

            OnTicketGet(ref result);

            return result;
        }
        partial void OnTicketDeleted(ITTicketingProject.Server.Models.TicketingDB.Ticket item);
        partial void OnAfterTicketDeleted(ITTicketingProject.Server.Models.TicketingDB.Ticket item);

        [HttpDelete("/odata/TicketingDB/Tickets(TicketId={TicketId})")]
        public IActionResult DeleteTicket(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Tickets
                    .Where(i => i.TicketId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTicketDeleted(item);
                this.context.Tickets.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTicketDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTicketUpdated(ITTicketingProject.Server.Models.TicketingDB.Ticket item);
        partial void OnAfterTicketUpdated(ITTicketingProject.Server.Models.TicketingDB.Ticket item);

        [HttpPut("/odata/TicketingDB/Tickets(TicketId={TicketId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTicket(int key, [FromBody]ITTicketingProject.Server.Models.TicketingDB.Ticket item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.TicketId != key))
                {
                    return BadRequest();
                }
                this.OnTicketUpdated(item);
                this.context.Tickets.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Tickets.Where(i => i.TicketId == key);
                
                this.OnAfterTicketUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/TicketingDB/Tickets(TicketId={TicketId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTicket(int key, [FromBody]Delta<ITTicketingProject.Server.Models.TicketingDB.Ticket> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Tickets.Where(i => i.TicketId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTicketUpdated(item);
                this.context.Tickets.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Tickets.Where(i => i.TicketId == key);
                
                this.OnAfterTicketUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTicketCreated(ITTicketingProject.Server.Models.TicketingDB.Ticket item);
        partial void OnAfterTicketCreated(ITTicketingProject.Server.Models.TicketingDB.Ticket item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ITTicketingProject.Server.Models.TicketingDB.Ticket item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnTicketCreated(item);
                this.context.Tickets.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Tickets.Where(i => i.TicketId == item.TicketId);

                

                this.OnAfterTicketCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
