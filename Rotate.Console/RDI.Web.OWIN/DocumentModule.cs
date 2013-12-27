using System.IO;
using Nancy;
using RDI.MVC.Models.Documents;
using RDI.MVC.ViewModels;
using RDI.MVC.ViewModels.Documents;

namespace RDI.Web.OWIN
{
    public class DocumentModule : NancyModule
    {
        public DocumentModule()
            : base("/document")
        {

            Get["/{id}"] = parameters =>
            {
                int id = parameters.id;
                var vm = new PdfViewModel { Document = new DocumentRepository().GetDocument(id) };
                var model = vm;
                return View["Document", model];
            };

            Get["/stream/{id}/{rotation}"] = parameters =>
            {

                int id = parameters.id;
                var document = new DocumentRepository().GetDocument(id);
                Stream stream = new MemoryStream(document.BodyBytes);
                return Response.FromStream(stream, "application/pdf");
                
            };
        }
    }
}