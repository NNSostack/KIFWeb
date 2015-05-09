using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for PDFParser
/// </summary>
/// <summary>
/// Parses a PDF file and extracts the text from it.
/// </summary>
public class PDFParser
{
    /// BT = Beginning of a text object operator 
    /// ET = End of a text object operator
    /// Td move to the start of next line
    ///  5 Ts = superscript
    /// -5 Ts = subscript

    #region Fields

    #region _numberOfCharsToKeep
    /// <summary>
    /// The number of characters to keep, when extracting text.
    /// </summary>
    private static int _numberOfCharsToKeep = 15;
    #endregion

    #endregion

    static String _invoicePath = null;
    static String GetInvoicePath()
    {
        _invoicePath = System.Web.HttpContext.Current.Session["~/App_Data/Medlemskontingenter.pdf"] as String;    
        
        if (_invoicePath == null)
        {
            _invoicePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Medlemskontingenter.pdf");
            System.Web.HttpContext.Current.Session["~/App_Data/Medlemskontingenter.pdf"] = _invoicePath;

            WebClient client = new WebClient();

            try
            {
                client.DownloadFile("https://dl.dropboxusercontent.com/s/wf28l4ajad0b8b1/Medlemskontingenter.pdf?dl=1&token_hash=AAG5LdLruF-7hfjpbk90086xAlZ2f4zuVPE-CQlmWrBZOQ&expiry=1399492074", _invoicePath);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        return _invoicePath;
    }

    public static String GetInvoicePathNoFrames()
    {
        var invoicePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/MedlemskontingenterUdenRammer.pdf");

        WebClient client = new WebClient();

        try
        {
            client.DownloadFile("https://dl.dropboxusercontent.com/s/xi26d7pcp32bvf0/MedlemskontingenterUdenRammer.pdf?dl=1&token_hash=AAEcAnVVuPRnzeX0pGMo7NW3SUlXk58HWXue4wcorFtj8Q", invoicePath);
        }
        catch (Exception)
        {
            //throw;
        }

        return invoicePath;
    }

    static String GetGiroKortPath(String memberId)
    {
        String outfile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/KIF/GiroKort/" + memberId + ".pdf");
        return outfile;
    }

    public static String GetGiroKortPathForPrint(String memberId)
    {
        String outfile = System.Web.HttpContext.Current.Server.MapPath("~/upload/KIF/" + memberId + ".pdf");
        return outfile;
    }


    public static Boolean InvoiceExists(String memberNumber)
    {
        return GetInvoice(GetInvoicePath(), null, memberNumber, true);
    }
    
    public static String GetInvoice(String memberNumber)
    {
        return GetInvoice(memberNumber, GetGiroKortPath(memberNumber), GetInvoicePath());  
    }

    public static String GetInvoice(String memberNumber, String path, String invoicePath)
    {
        String outfile = path;
        return GetInvoice(invoicePath, outfile, memberNumber, true) ? outfile : null;
    }

    public Boolean HasGiroKortBeenDownloaded(String memberNumber)
    {
        String outfile = GetGiroKortPath(memberNumber);
        return System.IO.File.Exists(outfile);
    }
    
    #region ExtractText
    /// <summary>
    /// Extracts a text from a PDF file.
    /// </summary>
    /// <param name="inFileName">the full path to the pdf file.</param>
    /// <param name="outFileName">the output file name.</param>
    /// <returns>the extracted text</returns>
    private static Boolean GetInvoice(string inFileName, String outputFile, String memberNumber, Boolean notUsed)
    {
        
        try
        {
            // Create a reader for the given PDF file
            PdfReader reader = new PdfReader(inFileName);
            //outFile = File.CreateText(outFileName);
            //outFile = new StreamWriter(outFileName, false, System.Text.Encoding.UTF8);

            //Console.Write("Processing: ");

            int totalLen = 68;
            float charUnit = ((float)totalLen) / (float)reader.NumberOfPages;
            int totalWritten = 0;
            float curUnit = 0;

            //ExtractPages(inFileName, @"C:\Users\Nikolaj Sostack\Downloads\PDF\pdf.pdf", 1, 1);

            for (int page = 1; page <= reader.NumberOfPages; page++)
            {


                //System.IO.File.WriteAllBytes(@"C:\Users\Nikolaj Sostack\Downloads\PDF\pdf.pdf", reader.GetPageContent(page));

                string txt = ExtractTextFromPDFBytes(reader.GetPageContent(page));
                if( txt.Contains("\n\r" + memberNumber + "\n\r") )
                {
                    if( !String.IsNullOrEmpty(outputFile) )
                        ExtractPages(inFileName, outputFile, page, page);
                    return true;
                }

                //// Write the progress.
                //if (charUnit >= 1.0f)
                //{
                //    for (int i = 0; i < (int)charUnit; i++)
                //    {
                //        Console.Write("#");
                //        totalWritten++;
                //    }
                //}
                //else
                //{
                //    curUnit += charUnit;
                //    if (curUnit >= 1.0f)
                //    {
                //        for (int i = 0; i < (int)curUnit; i++)
                //        {
                //            Console.Write("#");
                //            totalWritten++;
                //        }
                //        curUnit = 0;
                //    }

                //}
            }

            //if (totalWritten < totalLen)
            //{
            //    for (int i = 0; i < (totalLen - totalWritten); i++)
            //    {
            //        Console.Write("#");
            //    }
            //}
        }
        catch
        {
            throw;
        }
        finally
        {
            //if (outFile != null) outFile.Close();
        }

        return false;
    }
    #endregion

    private static void ExtractPages(string inputFile, string outputFile, int start, int end)
    {
        // get input document
        PdfReader inputPdf = new PdfReader(inputFile);

        // retrieve the total number of pages
        int pageCount = inputPdf.NumberOfPages;

        if (end < start || end > pageCount)
        {
            end = pageCount;
        }

        // load the input document
        Document inputDoc = new Document(inputPdf.GetPageSizeWithRotation(1));

        // create the filestream
        using (FileStream fs = new FileStream(outputFile, FileMode.Create))
        {
            // create the output writer
            PdfWriter outputWriter = PdfWriter.GetInstance(inputDoc, fs);
            inputDoc.Open();
            PdfContentByte cb1 = outputWriter.DirectContent;

            // copy pages from input to output document
            for (int i = start; i <= end; i++)
            {
                inputDoc.SetPageSize(inputPdf.GetPageSizeWithRotation(i));
                inputDoc.NewPage();

                PdfImportedPage page =
                    outputWriter.GetImportedPage(inputPdf, i);
                int rotation = inputPdf.GetPageRotation(i);

                if (rotation == 90 || rotation == 270)
                {
                    cb1.AddTemplate(page, 0, -1f, 1f, 0, 0,
                        inputPdf.GetPageSizeWithRotation(i).Height);
                }
                else
                {
                    cb1.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                }
            }

            inputDoc.Close();
        }
    }


    #region ExtractTextFromPDFBytes
    /// <summary>
    /// This method processes an uncompressed Adobe (text) object 
    /// and extracts text.
    /// </summary>
    /// <param name="input">uncompressed</param>
    /// <returns></returns>
    static private string ExtractTextFromPDFBytes(byte[] input)
    {
        if (input == null || input.Length == 0) return "";

        try
        {
            string resultString = "";

            // Flag showing if we are we currently inside a text object
            bool inTextObject = false;

            // Flag showing if the next character is literal 
            // e.g. '\\' to get a '\' character or '\(' to get '('
            bool nextLiteral = false;

            // () Bracket nesting level. Text appears inside ()
            int bracketDepth = 0;

            // Keep previous chars to get extract numbers etc.:
            char[] previousCharacters = new char[_numberOfCharsToKeep];
            for (int j = 0; j < _numberOfCharsToKeep; j++) previousCharacters[j] = ' ';


            for (int i = 0; i < input.Length; i++)
            {
                char c = (char)input[i];

                if (inTextObject)
                {
                    // Position the text
                    if (bracketDepth == 0)
                    {
                        if (CheckToken(new string[] { "TD", "Td" }, previousCharacters))
                        {
                            resultString += "\n\r";
                        }
                        else
                        {
                            if (CheckToken(new string[] { "'", "T*", "\"" }, previousCharacters))
                            {
                                resultString += "\n";
                            }
                            else
                            {
                                if (CheckToken(new string[] { "Tj" }, previousCharacters))
                                {
                                    resultString += " ";
                                }
                            }
                        }
                    }

                    // End of a text object, also go to a new line.
                    if (bracketDepth == 0 &&
                        CheckToken(new string[] { "ET" }, previousCharacters))
                    {

                        inTextObject = false;
                        resultString += " ";
                    }
                    else
                    {
                        // Start outputting text
                        if ((c == '(') && (bracketDepth == 0) && (!nextLiteral))
                        {
                            bracketDepth = 1;
                        }
                        else
                        {
                            // Stop outputting text
                            if ((c == ')') && (bracketDepth == 1) && (!nextLiteral))
                            {
                                bracketDepth = 0;
                            }
                            else
                            {
                                // Just a normal text character:
                                if (bracketDepth == 1)
                                {
                                    // Only print out next character no matter what. 
                                    // Do not interpret.
                                    if (c == '\\' && !nextLiteral)
                                    {
                                        nextLiteral = true;
                                    }
                                    else
                                    {
                                        if (((c >= ' ') && (c <= '~')) ||
                                            ((c >= 128) && (c < 255)))
                                        {
                                            resultString += c.ToString();
                                        }

                                        nextLiteral = false;
                                    }
                                }
                            }
                        }
                    }
                }

                // Store the recent characters for 
                // when we have to go back for a checking
                for (int j = 0; j < _numberOfCharsToKeep - 1; j++)
                {
                    previousCharacters[j] = previousCharacters[j + 1];
                }
                previousCharacters[_numberOfCharsToKeep - 1] = c;

                // Start of a text object
                if (!inTextObject && CheckToken(new string[] { "BT" }, previousCharacters))
                {
                    inTextObject = true;
                }
            }
            return resultString;
        }
        catch
        {
            return "";
        }
    }
    #endregion

    #region CheckToken
    /// <summary>
    /// Check if a certain 2 character token just came along (e.g. BT)
    /// </summary>
    /// <param name="search">the searched token</param>
    /// <param name="recent">the recent character array</param>
    /// <returns></returns>
    static private bool CheckToken(string[] tokens, char[] recent)
    {
        foreach (string token in tokens)
        {
            if ((recent[_numberOfCharsToKeep - 3] == token[0]) &&
                (recent[_numberOfCharsToKeep - 2] == token[1]) &&
                ((recent[_numberOfCharsToKeep - 1] == ' ') ||
                (recent[_numberOfCharsToKeep - 1] == 0x0d) ||
                (recent[_numberOfCharsToKeep - 1] == 0x0a)) &&
                ((recent[_numberOfCharsToKeep - 4] == ' ') ||
                (recent[_numberOfCharsToKeep - 4] == 0x0d) ||
                (recent[_numberOfCharsToKeep - 4] == 0x0a))
                )
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}