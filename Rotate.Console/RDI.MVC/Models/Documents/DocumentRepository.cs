using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using RDI.Web.Utilities;

namespace RDI.MVC.Models.Documents
{
    public class DocumentRepository : IDocumentRepository
    {
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
            var doc = new Document();
           
            doc.BodyBytes = File.ReadAllBytes(@"D:\Prototypes\PdfRotate\unrotated.pdf");
            //var tempSql = "SELECT DOC FROM DOCS WHERE DOC_ID = " + id;

            //var cmd = new SqlCommand(tempSql, con);

            //OpenCon();
            //var rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

            //while (rdr.Read())
            //{
            //    int byteLen = (int)rdr.GetBytes(0, 0, null, 0, 0);
            //    doc.BodyBytes = new byte[byteLen];
            //}

            //con.Close();

            return doc;
        }

        public void RotateDocument(Pdf.Rotationtype rotationtype, int id)
        {
            var doc = new Document();
            doc.BodyBytes = File.ReadAllBytes(@"D:\Prototypes\PdfRotate\unrotated.pdf");
            var ms = new MemoryStream(doc.BodyBytes);
            //ms.Write(doc.BodyBytes, 0, doc.BodyBytes.Length);
            var docstream = Pdf.Rotate(rotationtype, ms);

            using (var file = new FileStream(@"D:\Prototypes\PdfRotate\rotated2.pdf", FileMode.Create, FileAccess.Write))
            {
                var bw = new BinaryWriter(file);
                var bytes = new byte[docstream.Length];
                docstream.Read(bytes, 0, (int)docstream.Length);
                bw.Write(bytes);
                bw.Close();
                //file.Write(bytes, 0, bytes.Length);
                docstream.Close();
            }

            //return null;
        }
    }
}
