using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticket.Domain.Enum;
using Ticket.Domain.Models;
using Ticket.Repository.Interface;
using Ticket.Service.Interface;

namespace Ticket.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ITicketService _ticketService;

        public AdminController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExportTickets(int genre) 
        {
            try
            {
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "tickets.xlsx";

                var workbook = new XLWorkbook();
                IXLWorksheet worksheet = workbook.Worksheets.Add("Tickets");

                List<TicketModel> tickets = genre == 10 ? _ticketService.GetAllTickets().ToList() : _ticketService.GetTicketsByGenre((Genre)genre).ToList();

                worksheet.Cell(1, 1).Value = "Title";
                worksheet.Cell(1, 2).Value = "Genre";
                worksheet.Cell(1, 3).Value = "Starts";
                worksheet.Cell(1, 4).Value = "Price";
                worksheet.Cell(1, 5).Value = "Seats";
                worksheet.Cell(1, 6).Value = "3D";


                for (int i = 1; i <= tickets.Count; i++)
                {
                    worksheet.Cell(i + 1, 1).Value = tickets[i - 1].Title;
                    worksheet.Cell(i + 1, 2).Value = tickets[i - 1].Genre.ToString()[0] + tickets[i - 1].Genre.ToString()[1..];
                    worksheet.Cell(i + 1, 3).Value = tickets[i - 1].ValidUntil;
                    worksheet.Cell(i + 1, 4).Value = tickets[i - 1].Price;
                    worksheet.Cell(i + 1, 5).Value = tickets[i - 1].Seats;
                    worksheet.Cell(i + 1, 6).Value = tickets[i - 1].In3D;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
