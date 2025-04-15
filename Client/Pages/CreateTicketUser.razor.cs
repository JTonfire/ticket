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

        protected ITTicketingProject.Server.Models.TicketingDB.Ticket ticket;

        protected string problemDesc;
        protected string location;
        protected string phone;
        protected string email;

        protected async System.Threading.Tasks.Task Button0Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            
            
            
             
        }

        
    }
}