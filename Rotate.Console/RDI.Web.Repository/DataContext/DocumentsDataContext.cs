using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDI.MVC.Models.Documents;
using RDI.Utility;

namespace RDI.Web.Repository.DataContext
{
    class DocumentsDataContext :IDataContext<Document>
    {
        private readonly RDIDocuments _docRepository;
        //public DocumentRepository(string connectionString)
        //{
        //    _docRepository = new RDIDocuments(connectionString);
        //}
        public DocumentsDataContext()
        {
            var Cstring = "Data Source=SQL-Intranet2.resdat.com;Initial Catalog=RDI_Development;Integrated Security=SSPI";

            _docRepository = new RDIDocuments(Cstring);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Document> Get()
        {
            throw new NotImplementedException();
        }
 
        public Document GetbyId(object id)
        {
            var doc = new Document
            {
                Id = int.Parse(id.ToString()),
                BodyBytes = _docRepository.GetDocument(id),
                Filename = _docRepository.GetDocumentFileName(id),
                MimeType = _docRepository.GetDocumentMimeType(id)
            };

            return doc;
        }

        public Document Set(Document item)
        {
            throw new NotImplementedException();
        }

        public Document Set(object id)
        {
            throw new NotImplementedException();
        }

        public Document Delete(Document item)
        {
            throw new NotImplementedException();
        }

        public Document Delete(object id)
        {
            throw new NotImplementedException();
        }
    }
}
