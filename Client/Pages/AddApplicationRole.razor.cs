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
    public partial class AddApplicationRole
    {
        //Injects
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

        //variable to interact with role table
        protected ITTicketingProject.Server.Models.ApplicationRole role;
        //Error variable
        protected string error;
        //Boolean to make error component visable
        protected bool errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }


        //When page loads make a new role
        protected override async Task OnInitializedAsync()
        {
            role = new ITTicketingProject.Server.Models.ApplicationRole();
        }

        //When form submits make role
        protected async Task FormSubmit(ITTicketingProject.Server.Models.ApplicationRole role)
        {
            try
            {
                //Wait for the database call to complete
                await Security.CreateRole(role);

                DialogService.Close(null);
            }
            //Error catching
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

        //If cancel is clicked the close the page and any call to database
        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }
    }
}