using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDI.Utility;
using RDI.Web.Model.Documents;

namespace RDI.Web.Repository.DataContext
{
    public class DocumentsDataContext : IDataContext<Document>
    {
        private readonly RDIDocuments _docRepository;


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
 
        public Document Get(object id)
        {
            var docId = int.Parse(id.ToString());
            var doc = new Document
            {
                Id = docId,
                BodyBytes = _docRepository.GetDocument(docId),
                Filename = _docRepository.GetDocumentFileName(docId),
                MimeType = _docRepository.GetDocumentMimeType(docId)
            };

            return doc;
        }

        public Document Set(Document item)
        {
            _docRepository.UpdateDocument(item.BodyBytes, item.Id);
            var udoc = new Document
            {
                BodyBytes = _docRepository.GetDocument(item.Id),
                Filename = _docRepository.GetDocumentFileName(item.Id),
                MimeType = _docRepository.GetDocumentMimeType(item.Id)
            };

            return udoc;
        }

        public IEnumerable<Document> Set(List<Document> items)
        {
            throw new NotImplementedException();
        }

        public Document Delete(object id)
        {
            throw new NotImplementedException();
        }
    }
}
