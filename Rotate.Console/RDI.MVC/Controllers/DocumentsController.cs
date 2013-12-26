using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using System.Web.Http;
using RDI.MVC.Models;
using RDI.MVC.Models.Documents;

namespace RDI.MVC.Controllers
{
    public class DocumentsController : ApiController
    {
        private readonly IDocumentRepository _documentRepository;
        // GET api/documents

        public DocumentsController([Dependency("Document")] IDocumentRepository documentRepository)
        {
            if(documentRepository == null)
            {
                throw new ArgumentNullException("Document repository injection is null");
            }
            _documentRepository = documentRepository;
        }
        public IEnumerable<IModel> Get()
        {
            return null;
        }

        // GET api/documents/5
        public Document Get(int id)
        {
            return _documentRepository.GetDocument(id);
        }

        // POST api/documents
        public void Post([FromBody]string value)
        {
        }

        // PUT api/documents/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/documents/5
        public void Delete(int id)
        {
        }
    }
}