using System;

namespace RDI.MVC.Models.Documents
{
    public class Document : BaseModel
    {
        public byte[] BodyBytes { get; set; }

        public byte[] PreviewBytes { get; set; }

        public string Uri { get; set; }

        public string MimeType { get; set; }

        public string Filename { get; set; }
            
    }

}