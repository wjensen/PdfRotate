using System;

namespace RDI.MVC.Models.Documents
{
    public class Document : BaseModel
    {

        public byte[] BodyBytes { get; set; }

        public string Bodybase64
        {
            get { return Convert.ToBase64String(BodyBytes); }
        }

        public byte[] PreviewBytes { get; set; } 
        

        //public string Previewbase64
        //{
        //    get { return Convert.ToBase64String(PreviewBytes);}
        //}

        public string Uri { get; set; }

        public string MimeType { get; set; }

        public string Filename { get; set; }
            
    }

}