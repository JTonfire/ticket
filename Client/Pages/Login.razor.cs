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
    public partial class Login
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

        //Where to redirect the user
        protected string redirectUrl;
        //Error string
        protected string error;
        //Info about the error
        protected string info;
        //Make the error component visable
        protected bool errorVisible;
        //Make the info component visable
        protected bool infoVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        //When page loads
        protected override async Task OnInitializedAsync()
        {
            //query the DB with the login information to verify
            var query = System.Web.HttpUtility.ParseQueryString(new Uri(NavigationManager.ToAbsoluteUri(NavigationManager.Uri).ToString()).Query);

            //Get error
            error = query.Get("error");

            //get info
            info = query.Get("info");

            //What page to send the user to
            redirectUrl = query.Get("redirectUrl");

            //Make the error visbale
            errorVisible = !string.IsNullOrEmpty(error);

            //Make the info visable
            infoVisible = !string.IsNullOrEmpty(info);
        }
    }
}