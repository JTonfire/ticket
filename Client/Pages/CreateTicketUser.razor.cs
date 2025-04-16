using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;



namespace ITTicketingProject.Client.Pages
{
    public partial class CreateTicketUser
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
        protected SecurityService Security { get; set;}

        [Inject]
        protected TicketingDBService DBService {get; set;}

        //Ticket variable to hold ticket information for new ticket
        protected ITTicketingProject.Server.Models.TicketingDB.Ticket ticket;

        //When page loads make a new ticket
        protected override async Task OnInitializedAsync()
        {
            ticket = new Server.Models.TicketingDB.Ticket();
        }

        //When the submit button is clicked get all information then push it to DB
        protected async System.Threading.Tasks.Task Button0Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            //Test information
            ticket.AffectedUser = 1;
            ticket.Owner = 2;
            ticket.Status = "test";
            ticket.TicketId = 4;
            ticket.Description = "test";
            
            //Wait for the DB to create the new ticket
            await DBService.CreateTicket(ticket);

            
             
        }

        
    }
}