using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KIF_DisplayPages : System.Web.UI.Page
{
    public class fight
    {
        public String HjemmeHold { get; set; }
        public String UdeHold { get; set; }
        public String HjemmeHoldScore { get; set; }
        public String UdeHoldScore { get; set; }
        public String Tidspunkt { get; set; }
        public String Dommer { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        var kampe = Kamp.GetKampe(true).Where(x => !x.Oversidder);

        var date = DateTime.Now;

        Boolean showNextWeek = false;

        showNextWeek = Request["nextWeek"] != null;

        while( date.DayOfWeek != DayOfWeek.Monday )
            date = date.AddDays(showNextWeek ? 1 : -1);


        var list = new List<fight>();
        foreach (var kamp in kampe.Where(x => x.Date >= date && x.Date < date.AddDays(7)))
        {
            list.Add(new fight
            {
                HjemmeHold = kamp.Title,
                UdeHold = kamp.Modstander,
                Tidspunkt = kamp.Date.ToString("dddd \\d. dd-MM-yyyy kl. HH:mm"),
                Dommer = kamp.Dommer == "-" ? "DBU Dommer" : kamp.Dommer
            });
        }

        nextWeek.DataSource = list;
        udehold.DataSource = list;
        hjemmehold.DataSource = list;



        var lastWeeksFights = new List<fight>();

        foreach (var kamp in kampe.Where(x => x.Date < date && x.Date >= date.AddDays(-7)))
        {
            lastWeeksFights.Add(new fight
            {
                HjemmeHold = kamp.Title,
                UdeHold = kamp.Modstander,
                Tidspunkt = kamp.Date.ToString("dddd \\d. dd-MM-yyyy kl. HH:mm"),
                UdeHoldScore = kamp.HjemmeholdScore,
                HjemmeHoldScore = kamp.UdeholdScore
            });
        }

        lastWeek.DataSource = lastWeeksFights;
        
        DataBind();


        
    }
}