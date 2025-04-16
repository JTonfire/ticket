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
    public partial class ApplicationUsers
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

        //User variable to hold information for new user
        protected IEnumerable<ITTicketingProject.Server.Models.ApplicationUser> users;
        //Grid to hold all current users
        protected RadzenDataGrid<ITTicketingProject.Server.Models.ApplicationUser> grid0;
        //Error string
        protected string error;
        //Boolean to make error visable
        protected bool errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        //When page loads get all users to put in grid
        protected override async Task OnInitializedAsync()
        {
            users = await Security.GetUsers();
        }

        //Admin clicks the add button
        protected async Task AddClick()
        {
            //Dialog to add new user
            await DialogService.OpenAsync<AddApplicationUser>("Add Application User");

            //Get new user list for the grid
            users = await Security.GetUsers();
        }

        //Lets admin click on a row in the grid to edit
        protected async Task RowSelect(ITTicketingProject.Server.Models.ApplicationUser user)
        {
            //Dialog to edit specific user
            await DialogService.OpenAsync<EditApplicationUser>("Edit Application User", new Dictionary<string, object>{ {"Id", user.Id} });

            //Update grid with updated user list
            users = await Security.GetUsers();
        }

        //Lets admin delete a user
        protected async Task DeleteClick(ITTicketingProject.Server.Models.ApplicationUser user)
        {

            try
            {
                //Confirmation
                if (await DialogService.Confirm("Are you sure you want to delete this user?") == true)
                {
                    //Wait for the DB to delete the user
                    await Security.DeleteUser($"{user.Id}");

                    //Update grid with updated user list
                    users = await Security.GetUsers();
                }
            }
            //Exception
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }
    }
}