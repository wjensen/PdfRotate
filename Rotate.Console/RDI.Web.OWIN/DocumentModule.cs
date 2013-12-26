using Nancy;
using RDI.MVC.Models.Documents;
using RDI.MVC.ViewModels;

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
                var vm = new PdfEditor { PdfDocument = new DocumentRepository().GetDocument(id) };
                var model = vm;
                return View["Document", model];
            };
        }
    }
}