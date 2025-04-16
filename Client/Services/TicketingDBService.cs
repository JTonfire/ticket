
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace ITTicketingProject.Client
{
    public partial class TicketingDBService
    {
        //Variables to hold for API calls
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        //Default constructor
        public TicketingDBService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/TicketingDB/");
        }

        //Export tickets to excel
        public async System.Threading.Tasks.Task ExportTicketsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ticketingdb/tickets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ticketingdb/tickets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        //Export tickets to CSV
        public async System.Threading.Tasks.Task ExportTicketsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ticketingdb/tickets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ticketingdb/tickets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        //Partial class to hold message
        partial void OnGetTickets(HttpRequestMessage requestMessage);

        //API call to get all tickets from DB
        public async Task<Radzen.ODataServiceResult<ITTicketingProject.Server.Models.TicketingDB.Ticket>> GetTickets(Query query)
        {
            return await GetTickets(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        //API call to get all tickets from DB based on a filter
        public async Task<Radzen.ODataServiceResult<ITTicketingProject.Server.Models.TicketingDB.Ticket>> GetTickets(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Tickets");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTickets(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ITTicketingProject.Server.Models.TicketingDB.Ticket>>(response);
        }

        //Partial class to hold when ticket is created
        partial void OnCreateTicket(HttpRequestMessage requestMessage);

        //Create ticket API call
        public async Task<ITTicketingProject.Server.Models.TicketingDB.Ticket> CreateTicket(ITTicketingProject.Server.Models.TicketingDB.Ticket ticket = default(ITTicketingProject.Server.Models.TicketingDB.Ticket))
        {
            var uri = new Uri(baseUri, $"Tickets");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ticket), Encoding.UTF8, "application/json");

            OnCreateTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ITTicketingProject.Server.Models.TicketingDB.Ticket>(response);
        }

        //Delete ticket API call message
        partial void OnDeleteTicket(HttpRequestMessage requestMessage);

        //Delete ticket API call
        public async Task<HttpResponseMessage> DeleteTicket(int ticketId = default(int))
        {
            var uri = new Uri(baseUri, $"Tickets({ticketId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTicket(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        //Get ticket by ID message
        partial void OnGetTicketByTicketId(HttpRequestMessage requestMessage);

        //Get tickets by ID API call
        public async Task<ITTicketingProject.Server.Models.TicketingDB.Ticket> GetTicketByTicketId(string expand = default(string), int ticketId = default(int))
        {
            var uri = new Uri(baseUri, $"Tickets({ticketId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicketByTicketId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ITTicketingProject.Server.Models.TicketingDB.Ticket>(response);
        }

        //Update ticket message
        partial void OnUpdateTicket(HttpRequestMessage requestMessage);
        
        //API call to update a ticket
        public async Task<HttpResponseMessage> UpdateTicket(int ticketId = default(int), ITTicketingProject.Server.Models.TicketingDB.Ticket ticket = default(ITTicketingProject.Server.Models.TicketingDB.Ticket))
        {
            var uri = new Uri(baseUri, $"Tickets({ticketId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ticket), Encoding.UTF8, "application/json");

            OnUpdateTicket(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}