using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Net;
using System.Web;

public partial class Holdliste : System.Web.UI.Page
{
    protected class Medlem
    {
        public String Navn { get; set; }
        public String Årgang { get; set; }
        public String Email { get; set; }
        public String Telefon { get; set; }
        public String Adresse { get; set; }

        static int fornavn, efternavn, adresse, tlf1, tlf2, email1, email2, afdeling;

        public static void Initialize(String line)
        {
            //String[] match = { "Fornavn", "Efternavn", "Adresse 1", "Tlf. privat", "E-mail 1" };

            String[] split = line.Split(';');
            for (int i = 0; i < split.Length; i++)
            {
                String s = split[i];
                if (s == "Fornavn")
                    fornavn = i;
                else if (s == "Efternavn")
                    efternavn = i;
                else if (s == "Adresse 1")
                    adresse = i;
                else if (s == "Tlf. mobil")
                    tlf1 = i;
                else if (s == "Tlf. privat")
                    tlf2 = i;
                else if (s == "E-mail 1")
                    email1 = i;
                else if (s == "E-mail 2")
                    email2 = i;
                else if (s == "Afdeling")
                    afdeling = i;
            }
        }

        public static Medlem GetMedlem(String line)
        {
            Medlem m = new Medlem(); 
            
            String[] split = line.Split(';');
            m.Navn = split[fornavn] + " " + split[efternavn];
            m.Årgang = split[afdeling];
            
            m.Email = split[email1];
            if (String.IsNullOrEmpty(m.Email))
                m.Email = split[email2]; 
            
            m.Telefon = split[tlf1];
            if (String.IsNullOrEmpty(m.Telefon))
                m.Telefon = split[tlf2];

            m.Telefon = m.Telefon.Replace(" ", "");
            m.Adresse = split[adresse];

            return m;
        }


    }

    static List<Medlem> list = null;

    String Afdeling 
    {
        get
        {
            if (Request["afdeling"] != null)
            {
                dd.Visible = false;
                return Request["afdeling"].Replace("-", "");
            }

            

            return dd.SelectedValue;    
        }
    
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (list == null || true)
        {
            String file = Server.MapPath("~/App_Data/Medlemmer.csv");


			WebClient client = new WebClient();

            try
            {
                client.DownloadFile("https://dl.dropboxusercontent.com/sh/o2a9ik799civ4ta/GwEsX4vEsJ/Medlemmer.csv?dl=1&token_hash=AAG-S0B406BGKkETFY7gaKSMIfQ8C-j5mOM2Gu35U6t86A", file);
            }
            catch (Exception)
            {
                //throw;
            }

            String[] lines = System.IO.File.ReadAllLines(file, System.Text.Encoding.Default);
            Medlem.Initialize(lines[0]);
            list = new List<Medlem>(); 

            foreach (String line in lines.Skip(1))
            {
                list.Add(Medlem.GetMedlem(line));  
            }
            list = list.OrderBy(x => x.Navn).ToList(); 
        }

        if( !IsPostBack )
            dd.Items.AddRange(list.GroupBy(x => x.Årgang).OrderBy(x => x.Key).Select(x => new ListItem { Text = x.Key }).ToArray());

        memberList.DataSource = list.Where(x => x.Årgang.StartsWith(Afdeling));
        DataBind();
    }

    protected Boolean IsPrinting
    {
        get
        {
            return Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.ToLower().EndsWith("/print_page.aspx");
        }
    }

    protected String allMail = "";
    protected String GetEmail(String email)
    {
        String format = "<a href=\"\" title=\"{0}@{1}\" onclick=\"this.href='mailto:' + '{0}' + '@' + '{1}'\">Send email</a>";
        
        if ( IsPrinting )
            format = "<a href=\"\" title=\"{0}@{1}\" onclick=\"this.href='mailto:' + '{0}' + '@' + '{1}'\">{0}@{1}</script></a>";
        
        String[] split = email.Split('@');

        if( !String.IsNullOrEmpty(email) )
            allMail = allMail + email + ";";

        if (split.Length == 2)
            return String.Format(format, split[0], split[1]);

        return email;
                


    }
}