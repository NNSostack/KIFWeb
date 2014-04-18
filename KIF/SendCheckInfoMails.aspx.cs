using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KIF_SendCheckInfoMails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            txtMessage.Text = mailFormat;

        //var medlem = Medlem.GetMedlemmer().Skip(10).Take(1).First();  
        //Response.Write(GetBody(medlem, Guid.NewGuid()));  
    }


    private void SendMail(Medlem medlem)
    {
        SmtpClient client = new SmtpClient();

        String mail = txtTestMail.Text;

        //if (String.IsNullOrEmpty(mail))
        //    mail = medlem.Email;

        if (mail == "")
            throw new ApplicationException("No email");


        MailMessage mm = new MailMessage("kiffodbold@email.dk", mail, "Opdatering af informatioiner for '" + medlem.Navn + "'", "");

        Guid g = Guid.NewGuid(); 
        mm.Body = GetBody(medlem, g);
        mm.BodyEncoding = Encoding.UTF8; 
        mm.IsBodyHtml = true;
        mm.Bcc.Add(new MailAddress("kiffodbold@email.dk"));

        System.IO.File.WriteAllText(Server.MapPath("~/App_Data/KIF/InfoChecks/" + medlem.MemberId + "-" + g + ".txt"), "InfoCheck");
        
        
        client.Send(mm);
    }

    String GetBody(Medlem medlem, Guid g)
    {
        Boolean dataOk =
            medlem.Fødselsdato != "" &&
            medlem.Adresse != "" &&
            medlem.Postnummer != "" &&
            medlem.By != "" &&
            medlem.Telefon != "" &&
            medlem.Email != "";

        String ret = String.Format(txtMessage.Text, medlem.Navn, medlem.Årgang, medlem.Navn,
            medlem.Fødselsdato,
            medlem.Adresse,
            medlem.Postnummer,
            medlem.By,
            medlem.Telefon,
            medlem.Email,
            Request.Url.Host, Request.Url.Port != 80 ? ":" + Request.Url.Port : "",
            Request.RawUrl.Replace("SendCheckInfoMails", "InfoOk"),
            medlem.MemberId,
            Request.RawUrl.Replace("SendCheckInfoMails", "InfoNotOk"),
            dataOk ? "block" : "none",
            dataOk ? "none" : "block",
            g); 
        ret = ret.Replace("\r\n", "<br/>");
        return ret;

    }


    String mailFormat = @"Hej

Vi skal bede dig bekræfte de oplysninger vi har i vores system for <b>'{0}' - {1}</b>

Oplysningerne er vist herunder

<b>Navn</b>: {2}
<b>Fødselsdato</b>: {3}
<b>Adresse</b>: {4}
<b>Postnummer</b>: {5}
<b>By</b>: {6}
<b>Telefon</b>: {7}
<b>Email</b>: {8}

<div style=""display:{14}"">
    <div style=""text-align:center;cursor:pointer;background-color:rgb(73, 215, 25);border:1px solid rgb(0, 255, 0);padding:20px;width:300px;""><a style=""text-decoration:none;color:white;"" href=""http://{9}{10}{11}?memberId={12}&g={16}"">Tryk her hvis Informationerne er OK</a></div>
    <div style=""text-align:center;cursor:pointer;background-color:rgb(255, 0, 0);border:1px solid rgb(0, 255, 0);padding:20px;width:300px;""><a style=""text-decoration:none;color:white;"" href=""http://{9}{10}{13}?memberId={12}&g={16}"">Tryk her hvis Informationerne IKKE er OK</a></div>
</div>
<div style=""display:{15}"">
    <div style=""text-align:center;cursor:pointer;background-color:rgb(255, 0, 0);border:1px solid rgb(0, 255, 0);padding:20px;width:300px;""><a style=""text-decoration:none;color:white;"" href=""http://{9}{10}{13}?memberId={12}&g={16}"">Tryk her da vi mangler informationer</a></div>
</div>
På forhånd tak

Venlig hilsen

Bestyrelsen
Kauslunde fodbold";

    protected void send_Click(object sender, EventArgs e)
    {
        if (txtSecurity.Text != "prodknt")
        {
            Response.Write("Du skal skrive det hemmelige kodeord i kodeord!!");
            return;
        }

        txtSecurity.Text = "";
        String aargang = "";

        foreach (var medlem in Medlem.GetMedlemmer().OrderBy(x => x.Årgang))
        {
            if (aargang != medlem.Årgang)
            {
                aargang = medlem.Årgang;
                Response.Write("************ Årgang: " + aargang + " **********<br/>");

            }

            Response.Write(medlem.Årgang + " - " + Request.RawUrl.Replace("SendKontingentMails", "Kontingent") + "?memberId=" + medlem.MemberId + "<br/>");
            try
            {
                SendMail(medlem);
            }
            catch (Exception ex)
            {
                Response.Write("<b>FEJL: " + ex.ToString() + "</b>");

            }
        }
            

    }
    protected void showTest_Click(object sender, EventArgs e)
    {
        var medlem = Medlem.GetMedlemmer().Skip(5).Take(1).First();  
        Response.Write(GetBody(medlem, Guid.NewGuid()));  
    }
}