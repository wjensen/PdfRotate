using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDI.MVC.Models
{
    public interface IModel
    {
        int Id { get; set; }
        string ToXml();
    }
}
