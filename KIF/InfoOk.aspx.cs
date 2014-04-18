using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KIF_InfoOk : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String memberId = Request.QueryString["memberId"];
        Guid g;
        if (!String.IsNullOrEmpty(memberId) && Guid.TryParse(Request.QueryString["g"], out g))
        {
            String path = Server.MapPath("~/App_Data/KIF/InfoChecks/" + memberId + "-" + g + ".txt");
            if( System.IO.File.Exists(path) ) 
            {
                Response.Write(@"<div style=""color:#49D719""><h1>Oplysningerne er nu bekræftet. Tak for hjælpen.</h1></div>"); 
                System.IO.File.Delete(path);
            }
            else
            {
                if( System.IO.Directory.GetFiles(Server.MapPath("~/App_Data/KIF/InfoChecks/"), memberId + "-*.txt").Count() == 0 )
                    Response.Write(@"<div style=""color:RGB(255, 0, 0)""><h2>Oplysningerne er allerede bekræftet eller ændret. Hvis det var en fejl må du sende en mail til <a href=""mailto:kiffodbold@email.dk"">kiffodbold@email.dk</a></h2></div>"); 
                else
                    Response.Write(@"<div style=""color:RGB(255, 0, 0)""><h2>De angivne oplysninger kan ikke benyttes til at bekræfte dine oplysninger</h2></div>"); 
            }
        }
        else
        {
            Response.Write(@"<div style=""color:RGB(255, 0, 0)""><h2>Oplysningerne kan ikke bekræftes</h2></div>"); 
        }
    }
}