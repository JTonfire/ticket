using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace ITTicketingProject.Client.Pages
{
    public partial class Index
    {
        //Injections
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected TicketingDBService DBService {get; set;}

        IEnumerable<ITTicketingProject.Server.Models.TicketingDB.Ticket> tickets;

        IEnumerable<ITTicketingProject.Server.Models.TicketingDB.Ticket> openTickets;


        protected int count;

        protected bool visable = false;
        
        
        protected override async Task OnInitializedAsync()
        {
           if(Security.IsInRole(["user", "admin"]))
           {
                var result = await DBService.GetTickets(
                filter: $@"Owner eq '{Security.User.Name}' and Owner ne '{null}'",
                orderby: $@"TicketId desc",
                expand: "", 
                top: null,   
                skip: null, 
                count: false,
                format: null,
                select: "TicketId,AffectedUser,Description,Location,Status,Owner");
                tickets = result.Value;
                StateHasChanged();

                visable = true;

                var resultOpen = await DBService.GetTickets(
                filter: $@"Status eq 'Open' and Owner eq ''",
                orderby: $@"TicketId desc",
                expand: "", 
                top: null,   
                skip: null, 
                count: false,
                format: null,
                select: "TicketId,AffectedUser,Description,Location,Status,Owner");
                openTickets = resultOpen.Value;
                StateHasChanged();
                
            
            }
            else{
                var result = await DBService.GetTickets(
                    filter: $@"AffectedUser eq '{Security.User.Name}' and Owner ne '{null}'",
                orderby: $@"TicketId desc",
                expand: "", 
                top: null,   
                skip: null, 
                count: false,
                format: null,
                select: "TicketId,AffectedUser,Description,Location,Status,Owner");
                tickets = result.Value;
                StateHasChanged();
            }

            
            
            
        
        }

        protected async Task openRowSelect(ITTicketingProject.Server.Models.TicketingDB.Ticket ticket)
        {
            if(await DialogService.Confirm($"Are you sure you want to take ownership of ticket: {ticket.TicketId}?") == true)
            {
                ticket.Owner = Security.User.UserName;
                ticket.Status = "Taken";
                await DBService.UpdateTicket(ticket.TicketId, ticket);
                StateHasChanged();
            }
            else
            {
                DialogService.Close(null);
            }
        }

        protected async Task RowSelect(ITTicketingProject.Server.Models.TicketingDB.Ticket ticket)
            {
                await DialogService.OpenAsync<EditTicket>("Edit Ticket", new Dictionary<string, object>{ {"Tid", ticket.TicketId} });
                
                //Dialog to edit specific user
                
                var result = await DBService.GetTickets(
                filter: $@"Owner eq '{Security.User.Name}' and Owner ne '{null}'",
                orderby: $@"TicketId desc",
                expand: "", 
                top: null,   
                skip: null, 
                count: false,
                format: null,
                select: "TicketId,AffectedUser,Description,Location,Status,Owner"
                ); 

                
                
                tickets = result.Value;
                StateHasChanged();

             
            }
    }
}