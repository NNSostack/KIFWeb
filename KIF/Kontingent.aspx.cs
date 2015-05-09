using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KIF_Kontingent : System.Web.UI.Page
{
    protected String invoicePath { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        var medlemsNummer = Request.QueryString["memberId"];

        if (!String.IsNullOrEmpty(medlemsNummer) && medlemsNummer.Length == 5)
        {
            String outfile = PDFParser.GetInvoice(medlemsNummer);
            if ( !String.IsNullOrEmpty(outfile) )
            {
                invoicePath = outfile;
                Response.ContentType = "Application/pdf";
                Response.WriteFile(outfile); 
                Response.Write("FOund");
            }
            else
                Response.Write("Intet girokort fundet for medlem");
  
        }
    }


}