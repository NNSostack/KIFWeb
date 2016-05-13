using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for Kamp
/// </summary>
public class Kamp
{
    public String Title { get; set; }
    public DateTime Date { get; set; }
    public String Modstander { get; set; }

    public String Række { get; set; }
    public String Hjemmehold { get; set; }
    public String Dag { get; set; }

    public String Link { get; set; }
    public Boolean Oversidder { get; set; }

    public String HjemmeholdScore { get; set; }
    public String UdeholdScore { get; set; }

    public String Kl
    {
        get
        {
            if (Date.TimeOfDay == new TimeSpan(0,0,0,0,0) )
                return "";
            return Date.ToString("HH:mm");
        }
    }

    public Kamp()
    {
        Link = "#";
    }


    public String Dommer { get; set; }
    public String Kiosk { get; set; }

    public String Dato
    {
        get
        {
            return Date.ToString("dd-MM-yyyy");
        }
    }

    static String[] teams = { "Oldboys", "Old Boys", "Herre S1", "Herre S2", "Herre S3", "HS3", "Herre S4", "HS4", "Herre S5", "HS5",  "U17 drenge", "U-17 drenge", "U16 drenge", "U-16 drenge", "U15 drenge", "U-15 drenge", "U14 drenge", "U-14 drenge", "U13 drenge", "U-13 drenge", "U12 drenge", "U-12 drenge", "U11 drenge", "U-11 drenge", "U10 drenge", "U-10 drenge", "U9 drenge", "U-9 drenge", "U8 drenge", "U-8 drenge" };
    static String[] teamTitle = { "Oldboys", "Oldboys", "Serie 1", "Serie 2", "Serie 3", "Serie 3", "Serie 4", "Serie 4", "Serie 5", "Serie 5", "U-17", "U-17", "U-16", "U-16", "U-15", "U-15", "U-14", "U-14", "U-13", "U13", "U-12", "U-12", "U-11", "U-11", "U-10", "U-10", "U-9", "U-9", "U-8", "U-8" };
    static String[] stævneTeams = { "U8 drenge", "U9 drenge", "U10 drenge", "U11 drenge", "U-8 drenge", "U-9 drenge", "U-10 drenge", "U-11 drenge" };

    public static Kamp GetKamp(String line, Boolean all)
    {
        String[] split = line.Split(';');
        Kamp k = null;
        DateTime dt;

        //  If oversidder then just return null
        if (split[9] != "" && split[9] == "SAND")
            return null;

        if (DateTime.TryParse(split[2], out dt))
        {
            TimeSpan ts;
            TimeSpan.TryParse(split[3], out ts);
            //if ()
            {
                dt = dt.Add(ts);
                if (all || dt.Date >= DateTime.Now.Date)
                {

                    String title = split[4];
                    int count = 0;

                    //var team = teams.Where(x => title.ToLower().StartsWith(x.ToLower())).FirstOrDefault();
                    //if (team == null)
                    //    team = title;

                    String myTeamTitle = title;
                    foreach (String team in teams)
                    {
                        if (title.ToLower().StartsWith(team.ToLower()))
                        {
                            myTeamTitle = teamTitle[count];
                            break;
                        }
                        count++;
                    }

                    //foreach (String team in teams)
                    {
                        //if (title.ToLower().StartsWith(team.ToLower()))
                        {
                            k = new Kamp();
                            k.Title = myTeamTitle;// team;// +", " + dt.ToString("dddd \\d. dd\\/MM-yyyy kl. HH:mm");
                            String modstander = split[6];
                            int start = modstander.IndexOf("(");
                            if (start > -1)
                                modstander = modstander.Substring(0, start);

                            if (!all)
                            {
                                foreach (String stævne in stævneTeams)
                                {
                                    if (myTeamTitle.ToLower() == stævne.ToLower())
                                    {
                                        modstander = "stævne";
                                    }
                                }
                            }

                            k.Modstander = modstander;
                            k.Date = dt;
                            k.Hjemmehold = split[5];
                            k.Række = split[4];
                            k.Dommer = split[7];
                            k.Kiosk = split[8];
                            k.Dag = split[1];
                            k.HjemmeholdScore = split[10];
                            k.UdeholdScore = split[11];
                            if ( k.Modstander.ToLower() == "oversidder")
                                k.Oversidder = true;
                        }
                        count++;
                    }
                }

            }
        }
        
        return k;
    }

    public static List<Kamp> GetKampe(Boolean all)
    {
        List<Kamp> kampe = new List<Kamp>();
        WebClient client = new WebClient();
        String file = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/kampe.csv");

        try
        {
            client.DownloadFile("https://dl.dropboxusercontent.com/s/75i71rwlqa60xml/kampe.csv", file);
        }
        catch (Exception)
        {
            //throw;
        }

        foreach (String line in System.IO.File.ReadAllLines(file, System.Text.Encoding.Default))
        {
            Kamp k = Kamp.GetKamp(line, all);

            if ( k == null || ( !all && k.Oversidder) )
                continue;

            Kamp last = null;

            if (kampe.Count > 0)
                last = kampe[kampe.Count - 1];

            if (k != null && (all || last == null || last.Title != k.Title || kampe[kampe.Count - 1].Date.Date != k.Date.Date))
                kampe.Add(k);
        }

        return kampe.OrderBy(x => x.Date).ToList();

    }

    public String ToRss()
    {
        return String.Format(itemFormat, Title + (!String.IsNullOrEmpty(Modstander) ? " - " + Modstander : ""), Date.ToString("r"), ToString(Date), Link);
    }

    String ToString(DateTime d)
    {
        if (d == DateTime.MinValue)
            return "";

        DateTime now = DateTime.Now;
        String dateString = "";
        String timeFormat = " kl. HH:mm";

        if (now.Date == d.Date)
            dateString = "i dag";
        else if (now.Date.AddDays(1) == d.Date)
            dateString = "i morgen";
        else if (d.Date.AddDays(-7) < DateTime.Now.Date)
            dateString = d.ToString("på dddd \\d. dd\\/MM");
        else
            dateString = d.ToString("dddd \\d. dd\\/MM");

        return dateString + d.ToString(timeFormat);
    }

    static String itemFormat = @"<item><title><![CDATA[{0}]]></title> 
          <link>{3}</link> 
          <description><![CDATA[{2}<br/><br/>]]></description> 
          <pubDate>{1}</pubDate> 
          <guid>http://www.kiffodbold.dk</guid>
          </item>";
}