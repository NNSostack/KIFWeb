using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KampProgram : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        list.DataSource = Kamp.GetKampe(true);
        DataBind();
    }

    protected Boolean ShowKiosk
    {
        get
        {
            return Request.QueryString["kiosk"] == "1";
        }
    }


    protected String Color(DateTime d)
    {
        String ret = "";
        if (isNext)
        {
            ret = "blue ";
        }
        
        if (d.DayOfWeek == DayOfWeek.Sunday)
            ret += "red";
        else if (d.DayOfWeek == DayOfWeek.Saturday)
            ret += "yellow";

        return ret; 
    }

    Boolean nextFound = false;
    Boolean isNext = false;
    protected String IsNext(DateTime d)
    {
        if ( d > DateTime.Now && !nextFound )
        {
            isNext = true;
            nextFound = true;
            return "blue";
        }

        isNext = false;
        return "";
    }
}