using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using ITTicketingProject.Server.Data;

namespace ITTicketingProject.Server
{
    public partial class TicketingDBService
    {
        TicketingDBContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly TicketingDBContext context;
        private readonly NavigationManager navigationManager;

        public TicketingDBService(TicketingDBContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportTicketsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ticketingdb/tickets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ticketingdb/tickets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTicketsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ticketingdb/tickets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ticketingdb/tickets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTicketsRead(ref IQueryable<ITTicketingProject.Server.Models.TicketingDB.Ticket> items);

        public async Task<IQueryable<ITTicketingProject.Server.Models.TicketingDB.Ticket>> GetTickets(Query query = null)
        {
            var items = Context.Tickets.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTicketsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTicketGet(ITTicketingProject.Server.Models.TicketingDB.Ticket item);
        partial void OnGetTicketByTicketId(ref IQueryable<ITTicketingProject.Server.Models.TicketingDB.Ticket> items);


        public async Task<ITTicketingProject.Server.Models.TicketingDB.Ticket> GetTicketByTicketId(int ticketid)
        {
            var items = Context.Tickets
                              .AsNoTracking()
                              .Where(i => i.TicketId == ticketid);

 
            OnGetTicketByTicketId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTicketGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTicketCreated(ITTicketingProject.Server.Models.TicketingDB.Ticket item);
        partial void OnAfterTicketCreated(ITTicketingProject.Server.Models.TicketingDB.Ticket item);

        public async Task<ITTicketingProject.Server.Models.TicketingDB.Ticket> CreateTicket(ITTicketingProject.Server.Models.TicketingDB.Ticket ticket)
        {
            OnTicketCreated(ticket);

            var existingItem = Context.Tickets
                              .Where(i => i.TicketId == ticket.TicketId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Tickets.Add(ticket);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ticket).State = EntityState.Detached;
                throw;
            }

            OnAfterTicketCreated(ticket);

            return ticket;
        }

        public async Task<ITTicketingProject.Server.Models.TicketingDB.Ticket> CancelTicketChanges(ITTicketingProject.Server.Models.TicketingDB.Ticket item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTicketUpdated(ITTicketingProject.Server.Models.TicketingDB.Ticket item);
        partial void OnAfterTicketUpdated(ITTicketingProject.Server.Models.TicketingDB.Ticket item);

        public async Task<ITTicketingProject.Server.Models.TicketingDB.Ticket> UpdateTicket(int ticketid, ITTicketingProject.Server.Models.TicketingDB.Ticket ticket)
        {
            OnTicketUpdated(ticket);

            var itemToUpdate = Context.Tickets
                              .Where(i => i.TicketId == ticket.TicketId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(ticket);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTicketUpdated(ticket);

            return ticket;
        }

        partial void OnTicketDeleted(ITTicketingProject.Server.Models.TicketingDB.Ticket item);
        partial void OnAfterTicketDeleted(ITTicketingProject.Server.Models.TicketingDB.Ticket item);

        public async Task<ITTicketingProject.Server.Models.TicketingDB.Ticket> DeleteTicket(int ticketid)
        {
            var itemToDelete = Context.Tickets
                              .Where(i => i.TicketId == ticketid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTicketDeleted(itemToDelete);


            Context.Tickets.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTicketDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}