using System;
using System.Xml.Linq;

using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentVault
{
  class TextQueryHandler
  {

    List<List<string>> respectivecategories=new List<List<string>>();
    
    //gets the file reference and category of files which contain the text queries 
    public List<List<string>> textQuery(List<List<string>> inputs)
    {
      List<string> dummy = inputs[0];
      string matchalltextqueries = dummy[0];
      List<string> textqueries=inputs[1];
      List<string> categories = inputs[2];
      List<string> textfiles = getTextFiles(categories);
      List<List<string>> results = new List<List<string>>();
      StreamReader reader;
      int matchedqueries;
      int textqueriescount=textqueries.Count;
      int filecount;

      Directory.SetCurrentDirectory(@"..\\..\\textfiles");

      foreach (string file in textfiles)
      {

        dummy = new List<string>();
        dummy.Add(file);
        filecount = 0;
        reader = File.OpenText(file);

        string filecontent = reader.ReadToEnd().ToLower().TrimEnd();
        //If /A is specified in the command line, then searches for all the text queries in the same file
        if (matchalltextqueries.Equals("true"))
        {
          matchedqueries = 0;
          foreach (string textquery in textqueries)
          {
            string textqry = textquery.ToLower();
            if (filecontent.Contains(textqry))
            {
              matchedqueries++;
              continue;
            }
            else
              break;
          }
          //if file contains all the text queries

          if (matchedqueries == textqueriescount)
          {

            foreach (string itr in respectivecategories[filecount])
            {
              dummy.Add(itr);
            }

            dummy.Add(file);

            results.Add(dummy);
          }

        }

        else
        {
          foreach (string textquery in textqueries)
          {
            string textqry = textquery.ToLower();
            if (filecontent.Contains(textqry))
            {
              
              foreach (string itr in respectivecategories[filecount])
              {
                dummy.Add(itr);
              }

              dummy.Add(file);
              results.Add(dummy);
              break;
            }
          }
        }





        filecount++;






      }

      return results;
     




    }

    //get text files that belong to a particular category
    public List<string> getTextFiles(List<string> categories)
    {

      var appDomain = AppDomain.CurrentDomain;
      string temp1 = appDomain.BaseDirectory;
      Directory.SetCurrentDirectory(temp1);

      string path = Directory.GetCurrentDirectory();
      DirectoryInfo path1 = Directory.GetParent(path);
      string path2 = path1.ToString();
      path1 = Directory.GetParent(path2);
      path2 = path1.ToString() + "\\Testfiles\\xmlfiles";
      Directory.SetCurrentDirectory(path2);
      List<string> alltextfiles = new List<string>();

      foreach (var item in categories)
      {
        XDocument doc = XDocument.Load((string)item + ".xml");
        var q = from x in
                  doc.Elements("root")
                    //.Elements("reference")
                  .Descendants()
                select x;


        foreach (var elem in q)
        {
          alltextfiles.Add(Path.GetFileNameWithoutExtension((string)elem.Value)+".txt");
        }

      }

      

      Directory.SetCurrentDirectory(path2+"\\metadata files");
      List<string> dummy;
      foreach(string file in alltextfiles)
      {
        dummy=new List<string>();
        XDocument doc = XDocument.Load(Path.GetFileNameWithoutExtension(file)+".xml");
         var q = from x in
                doc.Elements("root")
                  .Elements("categories")
              .Descendants()
              select x;
          foreach (var elem in q)
          {
            dummy.Add((string)elem.Value);
          }    
          
          respectivecategories.Add(dummy);
      }
      return alltextfiles;
    }


#if(TEST_TextQueryHandler)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is TextQueryHandler!! ");
              TextQueryHandler obj=new TextQueryHandler();
              List<List<string>> inputs=new List<List<string>>();
              List<string> dummy0=new List<string>();              
              List<string> dummy=new List<string>();
              List<string> dummy1=new List<string>();

              dummy0.Add("matchallqueries");
              inputs.Add(dummy0);              
              dummy.Add("this");
              dummy.Add("that");
              inputs.Add(dummy);
              dummy1.Add("personal");
              dummy1.Add("office");              
              inputs.Add(dummy1);                            
              obj.textQuery(List<List<string>> inputs);
              
        }
  
#endif

  }
}
