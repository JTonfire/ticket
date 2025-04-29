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

        protected string affectedUser;


        protected IEnumerable<ITTicketingProject.Server.Models.ApplicationUser> user;

        //When page loads make a new ticket
        protected override async Task OnInitializedAsync()
        {
            ticket = new Server.Models.TicketingDB.Ticket();

            user = await Security.GetUsers();
            
            
            
        }

        //When the submit button is clicked get all information then push it to DB
        protected async Task Button0Click(ITTicketingProject.Server.Models.TicketingDB.Ticket ticket)
        {
            //Default Ticket Info
            ticket.Status = "Open";
            ticket.Owner = null;
            ticket.TicketId = 3;
            
            
            //Wait for the DB to create the new ticket
            await DBService.CreateTicket(ticket);

            
             
        }

        
    }
}