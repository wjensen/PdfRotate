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
        }
    }
}