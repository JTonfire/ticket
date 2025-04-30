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
    public partial class EditTicket
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
        protected SecurityService Security { get; set; }

        [Inject]
        protected TicketingDBService DBService {get; set;}

        [Parameter]
        public int Tid { get; set; }

        protected ITTicketingProject.Server.Models.TicketingDB.Ticket ticket;

        protected IEnumerable<ITTicketingProject.Server.Models.ApplicationUser> user;

        protected string[] status = ["Open","Taken","Closed"];

        //Error string
        protected string error;
        //Boolean to make error components visable
        protected bool errorVisible;

        protected override async Task OnInitializedAsync()
        {
            ticket = await DBService.GetTicketByTicketId("",Tid);

            user = await Security.GetUsers();
        }

        //When the submit button is clicked get all information then push it to DB
        protected async Task Button0Click(ITTicketingProject.Server.Models.TicketingDB.Ticket ticket)
        {
            
            
            try
            {
                //Wait for the DB to create the new ticket
                await DBService.UpdateTicket(Tid, ticket);

                await DialogService.Alert("Ticket Created");

                DialogService.Close(null);
            }
            //Exeception
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
            

            
             
        }


        protected async System.Threading.Tasks.Task Button1Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            if (await DialogService.Confirm("Are you sure you want to delete this ticket?") == true)
                {
                    try
                    {
                        await DBService.DeleteTicket(Tid);

                        await DialogService.Alert("Ticket Deleted");
                        DialogService.Close(null);
                        
                    }
                    catch (Exception ex)
                    {
                        errorVisible = true;
                        error = ex.Message;
                    }
                }
        }
    }
}