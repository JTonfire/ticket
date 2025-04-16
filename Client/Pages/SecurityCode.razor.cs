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
    public partial class SecurityCode
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

        //Check if verfication code is valid
        async Task VerifySecurityCode(string code)
        {
            if (code.Count() == 6)
            {
                //Wait to check the code
                await JSRuntime.InvokeVoidAsync("eval", "document.forms[0].submit()");
            }
        }

        //Variable to hold message
        string message;
        protected override async Task OnInitializedAsync()
        {
            //Wait for the page to load
            await base.OnInitializedAsync();

            //Make an API call to the DB and query the users email to send them the code 
            var uri = new Uri(NavigationManager.ToAbsoluteUri(NavigationManager.Uri).ToString());
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            message = $"We sent a verification code to {query.Get("email")}. Enter the code from the email below.";
        }

        RadzenSecurityCode sc;

        [Inject]
        protected SecurityService Security { get; set; }

        //After the page renders
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //Wait for the first render
            await base.OnAfterRenderAsync(firstRender);

            //If it is the first render wait for the security code
            if (firstRender)
            {
                await sc.FocusAsync();
            }
        }
    }
}
