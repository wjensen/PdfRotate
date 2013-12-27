using System;
using System.Collections.Generic;
using RDI.MVC.Models;
using RDI.Web.Utilities;

namespace RDI.MVC.ViewModels.Documents
{
    public class PdfViewModel
    {
        public IModel Document { get; set; }
        public List<string> Rotation { get { return new List<string>(Enum.GetNames(typeof (Pdf.Rotationtype))); } }
    }
}
