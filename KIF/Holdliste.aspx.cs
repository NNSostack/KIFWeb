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
        list = Medlem.GetMedlemmer(); 
        
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