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
    public partial class EditApplicationUser
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

        //Role variable to hold the roles assigned to user
        protected IEnumerable<ITTicketingProject.Server.Models.ApplicationRole> roles;
        //User variable to hold the user
        protected ITTicketingProject.Server.Models.ApplicationUser user;
        //List to hold all roles assigned to user
        protected IEnumerable<string> userRoles;
        //Error string
        protected string error;
        //Boolean to make error components visable
        protected bool errorVisible;

        [Parameter]
        public string Id { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //Query the user by their ID
            user = await Security.GetUserById($"{Id}");

            //Get the user roles and store them by the IDs of the role
            userRoles = user.Roles.Select(role => role.Id);

            //Query for the roles
            roles = await Security.GetRoles();
        }

        protected async Task FormSubmit(ITTicketingProject.Server.Models.ApplicationUser user)
        {
            try
            {
                //Assign the user the role(s)
                user.Roles = roles.Where(role => userRoles.Contains(role.Id)).ToList();
                //Update call to the DB with the new roles and user to do it to
                await Security.UpdateUser($"{Id}", user);
                DialogService.Close(null);
            }
            //Exeception
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

        //Cancel to close all operations
        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }
        
    }
}