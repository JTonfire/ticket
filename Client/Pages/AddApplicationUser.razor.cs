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
    public partial class AddApplicationUser
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

        //Lists of all the roles in database
        protected IEnumerable<ITTicketingProject.Server.Models.ApplicationRole> roles;
        //User variable to interact with the user table
        protected ITTicketingProject.Server.Models.ApplicationUser user;
        //List to hold all roles assigned to user
        protected IEnumerable<string> userRoles = Enumerable.Empty<string>();
        //Error string
        protected string error;
        //Boolean to make the error component visable
        protected bool errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        //When table loads make new user and get all roles in DB
        protected override async Task OnInitializedAsync()
        {
            user = new ITTicketingProject.Server.Models.ApplicationUser();

            roles = await Security.GetRoles();
        }

        //When the form submits make new user
        protected async Task FormSubmit(ITTicketingProject.Server.Models.ApplicationUser user)
        {
            try
            {
                //Get the roles picked by the user from the DB and assign it to the user variable
                user.Roles = roles.Where(role => userRoles.Contains(role.Id)).ToList();
                //Wait for the user to be created in DB
                await Security.CreateUser(user);
                DialogService.Close(null);
            }
            //Error catching
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

        //Close page and any call to DB
        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }
    }
}