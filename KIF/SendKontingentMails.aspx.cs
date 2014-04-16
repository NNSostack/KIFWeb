using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KIF_SendKontingentMails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            txtMessage.Text = mailFormat;
    }

    String mailFormat = @"Hej

Hermed kontingentopkrævning for <b>'{0}' - {5}</b>

<a href=""http://{1}{2}{3}?memberId={4}"" title=""Hent girokort"">Tryk her for at hente girokortet</a>

Check venligst at informationerne er rigtige før I betaler.

På forhånd tak

Venlig hilsen

Bestyrelsen
Kauslunde fodbold";

    private void SendMail(Medlem medlem)
    {
        SmtpClient client = new SmtpClient();

        String mail = txtTestMail.Text;

        //if (String.IsNullOrEmpty(mail))
        //    mail = medlem.Email;

        if (mail == "")
            throw new ApplicationException("No email");


        MailMessage mm = new MailMessage("kiffodbold@email.dk", mail, "Kontingentopkrævning for '" + medlem.Navn + "' - " + medlem.Årgang, "");
        mm.Body = GetBody(medlem);
        mm.BodyEncoding = Encoding.UTF8; 
        mm.IsBodyHtml = true;
        mm.Bcc.Add(new MailAddress("kiffodbold@email.dk"));  
        client.Send(mm);
    }

    String GetBody(Medlem medlem)
    {
        String ret = String.Format(txtMessage.Text, medlem.Navn, Request.Url.Host, Request.Url.Port != 80 ? ":" + Request.Url.Port : "", Request.RawUrl.Replace("SendKontingentMails", "Kontingent"), medlem.MemberId, medlem.Årgang);
        ret = ret.Replace("\r\n", "<br/>");
        return ret;

    }

    protected void send_Click(object sender, EventArgs e)
    {
        if (txtSecurity.Text != "prodknt")
        {
            Response.Write("Du skal skrive det hemmelige kodeord i kodeord!!");
            return;
        }

        txtSecurity.Text = "";
        PDFParser parser = new PDFParser();
        String aargang = "";

        foreach (var medlem in Medlem.GetMedlemmer().OrderBy(x => x.Årgang))
        {
            if (aargang != medlem.Årgang)
            {
                aargang = medlem.Årgang;
                Response.Write("************ Årgang: " + aargang + " **********<br/>");

            }

            if (parser.InvoiceExists(medlem.MemberId))
            {
                Response.Write(medlem.Årgang + " - " + Request.RawUrl.Replace("SendKontingentMails", "Kontingent") + "?memberId=" + medlem.MemberId + "<br/>");
                try
                {
                    SendMail(medlem);
                    return;
                }
                catch (Exception ex)
                {
                    Response.Write("<b>FEJL: " + ex.ToString() + "</b>");
                    
                }

            }
            else
                Response.Write("<b>Intet girokort fundet for " + medlem.Navn + ", " + medlem.Årgang + ", " + medlem.MemberId + "</b><br/>");
        }

    }
    protected void showTest_Click(object sender, EventArgs e)
    {
        var medlem = Medlem.GetMedlemmer().Skip(5).Take(1).First();  
        Response.Write(GetBody(medlem));  
    }
}