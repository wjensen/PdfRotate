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
        private string cstring = "Data Source=SQL-Intranet2.resdat.com;Initial Catalog=RDI_Development;Integrated Security=SSPI";
        private SqlConnection con;
        private void OpenCon()
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }
        public Document GetDocument(int id)
        {

            var docRepository = new RDIDocuments(cstring);

            var doc = new Document
                          {
                              Id = id,
                              BodyBytes = docRepository.GetDocument(id),
                              Filename = docRepository.GetDocumentFileName(id),
                              MimeType = docRepository.GetDocumentMimeType(id)
                          };

            return doc;
        }

        public Document RotateDocument(Pdf.Rotationtype rotationtype, int id)
        {
            var doc = GetDocument(id);
            
            var docstream = Pdf.Rotate(rotationtype, new MemoryStream(doc.BodyBytes));

            using (var file = new FileStream(@"D:\Prototypes\PdfRotate\rotated2.pdf", FileMode.Create, FileAccess.Write))
            {
                using(var bw = new BinaryWriter(file))
                {
                    var bytes = new byte[docstream.Length];
                    docstream.Read(bytes, 0, (int)docstream.Length);
                    bw.Write(bytes);
                    bw.Close();
                }

                docstream.Close();
            }

            using (var br = new BinaryReader(docstream))
            {
                doc.BodyBytes = br.ReadBytes((int) docstream.Length);
            }
            return doc;
        }
    }
}
