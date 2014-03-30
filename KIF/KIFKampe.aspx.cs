using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq; 

public partial class _Default : System.Web.UI.Page
{
    



    String channelFormat = @"<?xml version=""1.0"" encoding=""utf-8""?>
         <rss version=""2.0"">
             <channel>
              <title>Kampprogram</title> 
              <link>http://www.kiffodbold.dk</link> 
              <description></description> 
              <pubDate>{0}</pubDate>
              <lastBuildDate>{0}</lastBuildDate>
              <language>da-DK</language>
              <generator>Kauslunde fodbold</generator>
              {1}
            </channel>
        </rss>";

    
    protected void Page_Load(object sender, EventArgs e)
    {
        TimeSpan ts = new TimeSpan(1, 0, 0);

        List<Kamp> kampe = Kamp.GetKampe(false).Take(5).ToList();
        kampe.Add(new Kamp { Title = "Hent kampprogram", Link = "http://noerup-sostack.dk/kif/Kampprogram.aspx" });
        kampe.Add(new Kamp { Title = "Se kioskbemanding", Link = "http://noerup-sostack.dk/kif/Kampprogram.aspx?kiosk=1" }); 

        Response.ContentType = "application/rss+xml";
        
        String rss = "";
        foreach (Kamp k in kampe)
        {
            rss += k.ToRss();
        }

        rss = String.Format(channelFormat, DateTime.Now.ToString("r") , rss); 
        Response.Write(rss);


    }
}