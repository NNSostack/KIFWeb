using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KIF_Hentgirokort : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var outfile = "";
        outfile += OutputGiroKort("32687");
        outfile += OutputGiroKort("32403");

        if (!String.IsNullOrEmpty(outfile))
        {
            Response.ContentType = "Application/pdf";
            Response.WriteFile(outfile);
            Response.Write("Found");
        }
    }

    String OutputGiroKort(String medlemsNummer)
    {
        return PDFParser.GetInvoice(medlemsNummer);
        
    }
}