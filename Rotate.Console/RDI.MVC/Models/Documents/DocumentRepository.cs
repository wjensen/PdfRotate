using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace RDI.MVC.Models.Documents
{
    class DocumentRepository : IDocumentRepository
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
    }
}
