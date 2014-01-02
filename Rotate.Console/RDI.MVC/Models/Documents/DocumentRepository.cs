using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using RDI.Utility;
using RDI.Web.Utilities;

namespace RDI.MVC.Models.Documents
{
    public class DocumentRepository : IDocumentRepository
    {

        private readonly RDIDocuments _docRepository;
        public DocumentRepository(string connectionString)
        {
            _docRepository = new RDIDocuments(connectionString);
        }
        public DocumentRepository()
        {
            var Cstring = "Data Source=SQL-Intranet2.resdat.com;Initial Catalog=RDI_Development;Integrated Security=SSPI";

            _docRepository = new RDIDocuments(Cstring);
        }
        public Document GetDocument(int id)
        {

            var doc = new Document
                          {
                              Id = id,
                              BodyBytes = _docRepository.GetDocument(id),
                              Filename = _docRepository.GetDocumentFileName(id),
                              MimeType = _docRepository.GetDocumentMimeType(id)
                          };

            return doc;
        }

        public Document UpdateDocument(Document doc)
        {

            _docRepository.UpdateDocument(doc.BodyBytes, doc.Id);
           var udoc = new Document
                     {
                         BodyBytes = _docRepository.GetDocument(doc.Id),
                         Filename = _docRepository.GetDocumentFileName(doc.Id),
                         MimeType = _docRepository.GetDocumentMimeType(doc.Id)
                     };

           return udoc;
        }

        public Document UpdateRotatedDocument(Pdf.Rotationtype rotationtype, int id)
        {
            var doc = RotateDocument(rotationtype, id);
            return UpdateDocument(doc);
        }

        public Document RotateDocument(Pdf.Rotationtype rotationtype, int id)
        {
            var doc = GetDocument(id);
            if (rotationtype == Pdf.Rotationtype.None) return doc;
            Stream stream = new MemoryStream(doc.BodyBytes);
            var docstream = Pdf.Rotate(rotationtype, stream);

            //using (var file = new FileStream(@"D:\Prototypes\PdfRotate\rotated2.pdf", FileMode.Create, FileAccess.Write))
            //{
            //    using(var bw = new BinaryWriter(file))
            //    {
            //        var bytes = new byte[docstream.Length];
            //        docstream.Read(bytes, 0, (int)docstream.Length);
            //        bw.Write(bytes);
            //        bw.Close();
            //    }

            //    docstream.Close();
            //}

            using (var br = new BinaryReader(docstream))
            {
                doc.BodyBytes = br.ReadBytes((int) docstream.Length);
            }

            return doc;
        }

        public string GetFileName(int id)
        {
            return _docRepository.GetDocumentFileName(id);
        }
    }
}
