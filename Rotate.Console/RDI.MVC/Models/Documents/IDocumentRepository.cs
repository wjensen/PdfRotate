﻿namespace RDI.MVC.Models.Documents
{
    public interface IDocumentRepository
    {
        Document GetDocument(int id);

        Document UpdateDocument(Document doc);
    }
}
