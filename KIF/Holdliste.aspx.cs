using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;

public partial class Holdliste : System.Web.UI.Page
{
    

    static List<Medlem> list = null;

    protected String Afdeling 
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
        list = Medlem.GetMedlemmer();

        rptDepartments.DataSource = list.GroupBy(x => x.Årgang).OrderBy(x => x.Key).Select(x => new { Afdeling = x.Key }).ToList();

        if( !IsPostBack )
            dd.Items.AddRange(list.GroupBy(x => x.Årgang).OrderBy(x => x.Key).Select(x => new ListItem { Text = x.Key }).ToArray());

        if( Request.QueryString["afdeling"] != null )
            memberList.DataSource = list.Where(x => x.Årgang.StartsWith(Afdeling));
        DataBind();
    }

    FileInfo[] files = null;
    protected Boolean MissingUpdateInfo(String memberId)
    {
        if (files == null)
        {
            DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/App_Data/KIF/InfoChecks/"));
            files = di.GetFiles("*-*.txt");
        }

        return files.Any(x => x.Name.StartsWith(memberId + "-"));
    }

    protected Boolean MissingPayment(String memberId)
    {
        return list.FirstOrDefault(x => x.MemberId == memberId).MissingPayment;
    }

    PDFParser parser = new PDFParser();
    protected Boolean MissingDownload(String memberId)
    {
        return !parser.HasGiroKortBeenDownloaded(memberId);        
    }

    protected Boolean IsPrinting
    {
        get
        {
            return Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.ToLower().EndsWith("/print_page.aspx") || Request.QueryString["p"] != null;
        }
    }

    protected String allMail = "";
    protected String allMailWithSemiColon = "";
    protected String GetEmail(String email, Boolean showEmail)
    {
        String format = "<a href=\"\" title=\"{0}@{1}\" onclick=\"this.href='mailto:' + '{0}' + '@' + '{1}'\">Send email</a>";
        
        if ( showEmail  )
            format = "<a href=\"\" title=\"{0}@{1}\" onclick=\"this.href='mailto:' + '{0}' + '@' + '{1}'\">{0}@{1}</script></a>";
        
        String[] split = email.Split('@');

        if (!String.IsNullOrEmpty(email))
        {
            allMail = allMail + email + ",";
            allMailWithSemiColon = allMailWithSemiColon + email + ";";
        }

        if (split.Length == 2)
            return String.Format(format, split[0], split[1]);

        return email;
                


    }
}