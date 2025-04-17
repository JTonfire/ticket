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
    public partial class Profile
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

        //Old password variable
        protected string oldPassword = "";
        //New password variable
        protected string newPassword = "";
        //confirm password bariable
        protected string confirmPassword = "";
        //User varable to hold the password
        protected ITTicketingProject.Server.Models.ApplicationUser user;
        //Error string
        protected string error;
        //Error component visable
        protected bool errorVisible;
        //Boolean to make the success component visable
        protected bool successVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        //Get user when page loads
        protected override async Task OnInitializedAsync()
        {
            user = await Security.GetUserById($"{Security.User.Id}");
        }

        //When form is submitted
        protected async Task FormSubmit()
        {
            try
            {   
                //Change password method
                await Security.ChangePassword(oldPassword, newPassword);
                //Make succcess component visbale
                successVisible = true;
            }
            //Exception
            catch (Exception ex)
            {
                //Make error component visable
                errorVisible = true;
                error = ex.Message;
            }
        }
    }
}