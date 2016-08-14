using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SmtpClient client = new SmtpClient();
        MailMessage mm = new MailMessage("kiffodbold@email.dk", "nns@email.dk", "Subject", "Body");
        client.Send(mm);
    }
}