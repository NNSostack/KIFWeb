using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KIF_InfoNotOk : System.Web.UI.Page
{
    public Boolean InfoUpdated { get; set; }
    public Boolean Valid { get; set; }
    public String MemberId 
    {
        get
        {
            return Request.QueryString["memberId"];
        }

    }

    public Guid Guid
    {
        get
        {
            Guid g;
            if (Guid.TryParse(Request.QueryString["g"], out g))
                return g;
        
            return Guid.Empty;
        }

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        InfoUpdated = false; 

        String memberId = MemberId;
        Guid g = Guid;
        if (!String.IsNullOrEmpty(memberId) && g != Guid.Empty )
        {
            String path = Server.MapPath("~/App_Data/KIF/InfoChecks/" + memberId + "-" + g + ".txt");
            
            if( System.IO.File.Exists(path) ) 
            {
                Valid = true;
                var medlem = Medlem.GetMedlemmer().FirstOrDefault(x => x.MemberId == memberId);
                
                if( medlem != null )
                {
                    if (!IsPostBack)
                    {
                        

                        txtAddress.Text = medlem.Adresse;
                        txtName.Text = medlem.Navn;
                        txtCity.Text = medlem.By;
                        txtZip.Text = medlem.Postnummer;
                        txtPhone.Text = medlem.Telefon;
                        txtEmail.Text = medlem.Email;
                        DateTime dt;
                        if (DateTime.TryParse(medlem.Fødselsdato, out dt))
                        {
                            cal.SelectedDate = dt;
                            cal.VisibleDate = dt;
                        }


                    }
                }
                else
                    Response.Write(@"<div style=""color:RGB(255, 0, 0)""><h2>Medlemmet findes ikke mere!!</h2></div>"); 

            }
            else
            {
                if( System.IO.Directory.GetFiles(Server.MapPath("~/App_Data/KIF/InfoChecks/"), memberId + "-*.txt").Count() > 0 )
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

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        DataBind();

    }
        

    protected void confirmInfo_Click(object sender, EventArgs e)
    {
        String path = Server.MapPath("~/App_Data/KIF/InfoChecks/" + MemberId + "-" + Guid + ".txt");

        if (!System.IO.File.Exists(path))
            return;

        if (txtName.Text != "" && txtAddress.Text != "" && txtEmail.Text != "" && txtZip.Text != "" &&
            txtCity.Text != "" && txtPhone.Text != "" && cal.SelectedDate.Date != DateTime.MinValue.Date)
        {
            var medlem = Medlem.GetMedlemmer().FirstOrDefault(x => x.MemberId == MemberId);
            SendMail(medlem);
            
            System.IO.File.Delete(path);
            InfoUpdated = true;
        }
        else
            Response.Write("ALLE informationerne skal udfyldes");

    }

    private void SendMail(Medlem medlem)
    {
        SmtpClient client = new SmtpClient();

        String mail = "kiffodbold@email.dk";

        //if (String.IsNullOrEmpty(mail))
        //    mail = medlem.Email;

        if (mail == "")
            throw new ApplicationException("No email");
        
        MailMessage mm = new MailMessage("kiffodbold@email.dk", mail, "Der er kommet en opdatering til informationerne for '" + medlem.Navn + "'", "");

        mm.Body = GetBody(medlem);
        mm.BodyEncoding = Encoding.UTF8;
        mm.IsBodyHtml = true;
        
        client.Send(mm);
    }

    String GetBody(Medlem medlem)
    {
        var navn = medlem.Navn;
        DateTime dt;
        DateTime.TryParse(medlem.Fødselsdato, out dt);
        var fødselsdato = dt.ToString();

        var adresse = medlem.Adresse;
        var postnummer = medlem.Postnummer;
        var by = medlem.By;
        var telefon = medlem.Telefon;
        var email = medlem.Email;

        if (txtName.Text != navn)
            navn = "<b>" + txtName.Text + "</b>";

        if (txtAddress.Text != adresse)
            adresse = "<b>" + txtAddress.Text + "</b>";

        if (txtZip.Text != postnummer)
            postnummer = "<b>" + txtZip.Text + "</b>";

        if (txtCity.Text != by)
            by = "<b>" + txtCity.Text + "</b>";

        if (txtPhone.Text != telefon)
            telefon = "<b>" + txtPhone.Text + "</b>";

        if (txtEmail.Text != email)
            email = "<b>" + txtEmail.Text + "</b>";

        if (cal.SelectedDate.ToString() != fødselsdato)
            fødselsdato = "<b>" + cal.SelectedDate.ToString() + "</b>";

        String ret = String.Format(mailFormat, medlem.Navn, medlem.MemberId,
            navn,
            fødselsdato,
            adresse,
            postnummer,
            by,
            telefon,
            email);

        ret = ret.Replace("\r\n", "<br/>");

        return ret;
    }

    String mailFormat = @"Der er kommet ændringer for '{0}' - {1}

Navn: {2}
Fødselsdato: {3}
Adresse: {4}
Postnummer: {5}
By: {6}
Telefon: {7}
Email: {8}";
    

}