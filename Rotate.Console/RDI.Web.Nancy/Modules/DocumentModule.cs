using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using Nancy;
using RDI.MVC.Models.Documents;
using RDI.MVC.ViewModels.Documents;
using RDI.Utility;


namespace RDI.Web.Nancy.Modules
{
    public class DocumentModule : NancyModule
    {
        public DocumentModule()
            : base("/document")
        {

            Get["/{id}"] = parameters =>
            {

                int id = parameters.id;
                var fileName = new DocumentRepository().GetFileName(id);
                var vm = new PdfViewModel { Document = new Document{Id = id,Filename = fileName} };
                var model = vm;
                return View["Document", model];
            };

            Get["/stream/{id}/{rotation}"] = parameters =>
            {
                var rotation =(Pdf.Rotationtype)Enum.Parse(typeof (Pdf.Rotationtype), parameters.rotation, true);
                int id = parameters.id;
                var document = new DocumentRepository().RotateDocument(rotation, id);
                Stream stream = new MemoryStream(document.BodyBytes);
                return Response.FromStream(stream, "application/pdf");
                
            };

            Post["/save/{id}/{rotation}"] = parameters =>
            {
                var rotation = (Pdf.Rotationtype)Enum.Parse(typeof(Pdf.Rotationtype), parameters.rotation, true);
                int id = parameters.id;
                var document = new DocumentRepository().UpdateRotatedDocument(rotation, id);
                //Stream stream = new MemoryStream(document.BodyBytes);
                //return Response.FromStream(stream, "application/pdf");
                return "";
            };
        }
    }
}