using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

public class Medlem
{
    public String Navn { get; set; }
    public String Årgang { get; set; }
    public String Fødselsdato { get; set; }
    public String Email { get; set; }
    public String Telefon { get; set; }
    public String Adresse { get; set; }
    public String MemberId { get; set; }
    public String Postnummer { get; set; }
    public String By { get; set; }
    public Boolean Kontingentfritagelse { get; set; }
    public Boolean AllowEmail { get; set; }
    public String Rabat { get; set; }

    static int fornavn, efternavn, adresse, tlf1, tlf2, email1, email2, afdeling, medlemsNr, fødselsdato, by, postnummer, kontingentfritagelse, smsEmail, rabat, rabatText;

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
            else if (s == "Medlemsnr.")
                medlemsNr = i;
            else if (s == "Fødselsdato")
                fødselsdato = i;
            else if (s == "By")
                by = i;
            else if (s == "Postnr.")
                postnummer = i;
            else if (s == "Kontingentfritagelse")
                kontingentfritagelse = i;
            else if (s == "SMS/Email fra klubben")
                smsEmail = i;
            else if (s == "Rabat")
                rabat = i;
            else if (s == "Årsag (Rabat)")
                rabatText = i;

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
        m.MemberId = split[medlemsNr];
        m.Fødselsdato = split[fødselsdato];
        m.Postnummer = split[postnummer];
        m.By = split[by];
        m.Rabat = split[rabat];

        if( m.Rabat != "" )
            m.Rabat = split[rabatText] + " (" + split[rabat] + " kr.)";
        
        Boolean b;

        if (split[kontingentfritagelse].ToLower() == "ja")
            m.Kontingentfritagelse = true;

        m.AllowEmail = false;
        
        if (split[smsEmail].ToLower() == "ja")
            m.AllowEmail = true;

        return m;
    }

    public static List<Medlem> GetMedlemmer()
    {
        String file = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Medlemmer.csv");


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
        var list = new List<Medlem>();

        foreach (String line in lines.Skip(1))
        {
            list.Add(Medlem.GetMedlem(line));
        }
        list = list.OrderBy(x => x.Navn).ToList();
        return list;
    }

}