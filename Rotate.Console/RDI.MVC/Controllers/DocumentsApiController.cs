using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.Http;
using RDI.MVC.Models;
using RDI.MVC.Models.Documents;

namespace RDI.MVC.Controllers
{
    [Authorize]
    public class DocumentsApiController : ApiController
    {
        
        private readonly IDocumentRepository _documentRepository;
        

        public DocumentsApiController([Microsoft.Practices.Unity.Dependency("Document")] IDocumentRepository documentRepository)
        {
            if(documentRepository == null)
            {
                throw new ArgumentNullException("Document repository injection is null");
            }
            _documentRepository = documentRepository;
        }
        
        // GET api/documents
        public IEnumerable<IModel> Get()
        {
            return null;
        }

        // GET api/documents/5
        public Document Get(int id)
        {
            var session = HttpContext.Current.Session;
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