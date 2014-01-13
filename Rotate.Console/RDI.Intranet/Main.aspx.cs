using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RDI.Utility;

namespace RDI.Intranet
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //doclink.HRef = "/RDI.Web.Owin/document/15/" + HttpUtility.UrlEncode(EncryptionProvider.Encrypt(Session["ConnectionString"].ToString()));
            //doclink.HRef = "/RDI.Web.Owin/document/"+HttpUtility.UrlEncode(EncryptionProvider.Encrypt("15"))+"/";
            doclink.HRef = "/RDI.Web.Api/api/documents/15";
        }
    }
}