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
    public partial class ApplicationRoles
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

        //Variable to hold all the roles
        protected IEnumerable<ITTicketingProject.Server.Models.ApplicationRole> roles;
        //Grid elements
        protected RadzenDataGrid<ITTicketingProject.Server.Models.ApplicationRole> grid0;
        //error string
        protected string error;
        //Boolean to make error visable
        protected bool errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        //When make loads get all roles
        protected override async Task OnInitializedAsync()
        {
            roles = await Security.GetRoles();
        }

        //When the user hits the add button open a dialog box to add roles
        protected async Task AddClick()
        {
            await DialogService.OpenAsync<AddApplicationRole>("Add Application Role");

            roles = await Security.GetRoles();
        }

        //Delete click to delete any role in DB
        protected async Task DeleteClick(ITTicketingProject.Server.Models.ApplicationRole role)
        {
            try
            {
                //Confirmation request
                if (await DialogService.Confirm("Are you sure you want to delete this role?") == true)
                {
                    //Wait for the DB to delete the role
                    await Security.DeleteRole($"{role.Id}");

                    //Reset the grid with the new list of roles
                    roles = await Security.GetRoles();
                }
            }
            //Error catching
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }
    }
}