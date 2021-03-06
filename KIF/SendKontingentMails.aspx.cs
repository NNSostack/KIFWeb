﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KIF_SendKontingentMails : System.Web.UI.Page
{
    protected String allEmails = "";

    protected String SetEmail(String email)
    {
        allEmails += email + ",";
        return "";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            txtMessage.Text = mailFormat;

        if( !Page.IsPostBack )
            txtSubject.Text = "Kontingentopkrævning for '{Navn}' for " + GetSeasonAndYear();
    }

    String mailFormat = @"Hej

Hermed kontingentopkrævning for <b>'{0}' - {5}</b>

<a href=""http://{1}{2}{3}?memberId={4}"" title=""Hent girokort"">Tryk her for at hente girokortet</a>

Check venligst at informationerne er rigtige før I betaler.

<strong>NB!! Hvis ikke kontingentindbetalingen er modtaget senest en uge efter betalingsfristen udelukkes spiller fra træning og kamp.</strong>

På forhånd tak

Venlig hilsen

Bestyrelsen
Kauslunde fodbold";

    private void SendMail(Medlem medlem)
    {
        SmtpClient client = new SmtpClient();

        String mail = txtTestMail.Text;

        if (String.IsNullOrEmpty(mail))
            mail = medlem.Email;

        if (mail == "")
            throw new ApplicationException("No email");

        medlem.Navn = medlem.Navn.Replace("ø", "oe");
        medlem.Navn = medlem.Navn.Replace("Ø", "Oe");

        medlem.Navn = medlem.Navn.Replace("æ", "ae");
        medlem.Navn = medlem.Navn.Replace("Æ", "Ae");

        medlem.Navn = medlem.Navn.Replace("å", "aa");
        medlem.Navn = medlem.Navn.Replace("Å", "Aa");


        MailMessage mm = new MailMessage(
            new MailAddress("noreply@nørup-sostack.dk", "Kauslunde fodbold"), new MailAddress(mail));
        
        mm.Subject = txtSubject.Text.Replace("{Navn}", medlem.Navn);
        mm.ReplyTo = new MailAddress("kiffodbold@email.dk");
        mm.Body = GetBody(medlem);
        mm.SubjectEncoding = Encoding.UTF8; 
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


    Boolean kontingentMails = true;
    protected void send_Click2(object sender, EventArgs e)
    {
        kontingentMails = false;
        send_Click(sender, e); 
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

        foreach (var medlem in Medlem.GetMedlemmer().Where(x => txtMedlemsnummer.Text == "" || txtMedlemsnummer.Text == x.MemberId).OrderBy(x => x.Årgang))
        {
            if (aargang != medlem.Årgang)
            {
                aargang = medlem.Årgang;
                Response.Write("************ Årgang: " + aargang + " **********<br/>");

            }

            if (PDFParser.InvoiceExists(medlem.MemberId) || !kontingentMails)
            {
                if (kontingentMails)
                {
                    if (chkOnlySendToMembersWhoHaveNotDownloadedGiro.Checked)
                    {
                        //  If giro has been downloaded then dont send
                        if ( parser.HasGiroKortBeenDownloaded(medlem.MemberId) )
                            continue;
                    }

                    if (chkOnlySendToMembersWithNoPayment.Checked)
                        //  If member does not miss payment we continue and dont send
                        if (!medlem.MissingPayment)
                            continue;

                    Response.Write(medlem.Årgang + " - " + Request.RawUrl.Replace("SendKontingentMails", "Kontingent") + "?memberId=" + medlem.MemberId + "<br/>");
                }
                else
                {
                    Response.Write(medlem.MemberId + ": " + medlem.Navn + ", " + medlem.Email);
                    if (!medlem.AllowEmail)
                    {
                        Response.Write(" - VIL IKKE MODTAGE MAILS FRA KLUBBEN<br/>");
                        continue;
                    }
                    else
                        Response.Write("<br/>");
                }

                Response.Flush();
                
                try
                {
                    SendMail(medlem);
                    if (txtTestMail.Text != "")
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
    protected void showNoDownload_Click(object sender, EventArgs e)
    {
        PDFParser parser = new PDFParser();
        var fullList = Medlem.GetMedlemmer();
        var list = fullList.Where(x => !parser.HasGiroKortBeenDownloaded(x.MemberId)).OrderBy(x => x.Årgang).ThenBy(x => x.Navn);

        var fritaget = fullList.Where(x => x.Kontingentfritagelse).ToList();
        rptFritaget.DataSource = fritaget;
        rptNoDownload.DataSource = list.Where(x => !x.Kontingentfritagelse && PDFParser.InvoiceExists(x.MemberId)).ToList();
        rptNoInvoice.DataSource = list.Where(x => !x.Kontingentfritagelse && !PDFParser.InvoiceExists(x.MemberId)).ToList(); 
        rptDiscount.DataSource = fullList.Where(x => !String.IsNullOrEmpty(x.Rabat)).ToList();
        rptHasDownloaded.DataSource = fullList.Except(list);
        DataBind();
    }
    protected void cmdGetGiroKort_Click(object sender, EventArgs e)
    {
        PDFParser parser = new PDFParser();
        var list = Medlem.GetMedlemmer().Where(x => !parser.HasGiroKortBeenDownloaded(x.MemberId)).OrderBy(x => x.Årgang).ThenBy(x => x.Navn);

        foreach( var medlem in list )
        {
            String medlemsNummer = medlem.MemberId;
            var source = PDFParser.GetGiroKortPathForPrint(medlemsNummer);
            String outfile = PDFParser.GetInvoice(medlemsNummer, source, PDFParser.GetInvoicePathNoFrames());
            if (outfile != null)
            {
                var filename = outfile.Replace(medlemsNummer, medlem.Årgang + "-" + medlemsNummer);
                Response.Write("Found giro: " + filename + "<br/>");
                Response.Flush();

                if (System.IO.File.Exists(filename) )
                    System.IO.File.Delete(filename);
                System.IO.File.Move(outfile, filename);

                if (!String.IsNullOrEmpty(outfile))
                {
                    TheDownload("http://" + Request.Url.Host + ":" + Request.Url.Port + "/Upload/KIF/" + medlemsNummer.ToString() + ".pdf", txtDownloadPath.Text + "\\" + medlemsNummer.ToString() + ".pdf");
                }
                else
                    Response.Write("Intet girokort fundet for medlem");
            }
        }
    }

    public void TheDownload(String source,  string fileName)
    {
        //WebClient client = new WebClient();
        //client.DownloadFile(source, Server.MapPath("/KIF/App_Data/KIF/NotDownloaded/"); 
    }

    protected String GetSeasonAndYear()
    {
        DateTime now = DateTime.Now;
        var ret = "";

        if (now.Month < 6)
            ret = "forår ";
        else
            ret = "efterår ";

        return ret + now.Year;

    }
}