using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using ITTicketingProject.Server.Data;

namespace ITTicketingProject.Server.Controllers
{
    public partial class ExportTicketingDBController : ExportController
    {
        private readonly TicketingDBContext context;
        private readonly TicketingDBService service;

        public ExportTicketingDBController(TicketingDBContext context, TicketingDBService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/TicketingDB/tickets/csv")]
        [HttpGet("/export/TicketingDB/tickets/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTicketsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTickets(), Request.Query, false), fileName);
        }

        [HttpGet("/export/TicketingDB/tickets/excel")]
        [HttpGet("/export/TicketingDB/tickets/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTicketsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTickets(), Request.Query, false), fileName);
        }
    }
}
