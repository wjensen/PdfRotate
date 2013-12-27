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
            doc.Filename = "This Is a pdf file";

            var cstring =
                "Data Source=SQL-Intranet2.resdat.com;Initial Catalog=RDI_Development;User ID=rda;Password=rda";
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
