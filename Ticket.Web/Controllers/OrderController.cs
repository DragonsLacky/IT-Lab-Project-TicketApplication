using GemBox.Document;
using GemBox.Document.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ticket.Service.Interface;

namespace Ticket.Web.Controllers
{
    public class OrderController : Controller
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            return View(_orderService.GetOrders());
        }

        public IActionResult Details(Guid id)
        {
            return View(_orderService.GetOrder(id));
        }

        public IActionResult UserOrder()
        {
            return View(nameof(Index), _orderService.GetUserOrders(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        public IActionResult Invoice(Guid id)
        {
            var order = _orderService.GetOrder(id);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "invoice.docx");
            var document = DocumentModel.Load(path);
            Table table = (Table)document.Content.GetChildElements().ToList()[2];

            foreach(var ticket in order.Tickets)
            {
                var row = new TableRow(document);
                table.Rows.Add(row);

                var title = new TableCell(document);
                row.Cells.Add(title);
                title.CellFormat.VerticalAlignment = VerticalAlignment.Center;
                Paragraph titleParagraph = new Paragraph(document, ticket.Ticket.Title);
                titleParagraph.ParagraphFormat.Alignment = HorizontalAlignment.Center;
                title.Blocks.Add(titleParagraph);

                var time = new TableCell(document);
                row.Cells.Add(time);
                time.CellFormat.VerticalAlignment = VerticalAlignment.Center;
                Paragraph timeParagraph = new Paragraph(document, ticket.Ticket.ValidUntil.ToString());
                timeParagraph.ParagraphFormat.Alignment = HorizontalAlignment.Center;
                time.Blocks.Add(timeParagraph);

                var genre = new TableCell(document);
                row.Cells.Add(genre);
                genre.CellFormat.VerticalAlignment = VerticalAlignment.Center;
                Paragraph genreParagraph = new Paragraph(document, ticket.Ticket.Genre.ToString()[0] + ticket.Ticket.Genre.ToString().ToLower()[1..]);
                genreParagraph.ParagraphFormat.Alignment = HorizontalAlignment.Center;
                genre.Blocks.Add(genreParagraph);

                var seats = new TableCell(document);
                row.Cells.Add(seats);
                seats.CellFormat.VerticalAlignment = VerticalAlignment.Center;
                Paragraph steatsParagraph = new Paragraph(document, ticket.Ticket.Seats.ToString());
                steatsParagraph.ParagraphFormat.Alignment = HorizontalAlignment.Center;
                seats.Blocks.Add(steatsParagraph);

                var in3d = new TableCell(document);
                row.Cells.Add(in3d);
                in3d.CellFormat.VerticalAlignment = VerticalAlignment.Center;
                Paragraph in3Dparagraph = new Paragraph(document, ticket.Ticket.In3D ? "yes" : "no");
                in3Dparagraph.ParagraphFormat.Alignment = HorizontalAlignment.Center;
                in3d.Blocks.Add(in3Dparagraph);

                var price = new TableCell(document);
                row.Cells.Add(price);
                price.CellFormat.VerticalAlignment = VerticalAlignment.Center;
                Paragraph priceParagraph = new Paragraph(document, ticket.Ticket.Price.ToString());
                priceParagraph.ParagraphFormat.Alignment = HorizontalAlignment.Center;
                price.Blocks.Add(priceParagraph);

                var quantity = new TableCell(document);
                row.Cells.Add(quantity);
                quantity.CellFormat.VerticalAlignment = VerticalAlignment.Center;
                Paragraph quantityParagraph = new Paragraph(document, ticket.Quantity.ToString());
                quantityParagraph.ParagraphFormat.Alignment = HorizontalAlignment.Center;
                quantity.Blocks.Add(quantityParagraph);
            }

            using var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

    }
}
