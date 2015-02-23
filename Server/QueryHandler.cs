//////////////////////////////////////////////////////////////////////////////
///  QueryHandler.cs  -  Handles the metadata queries       
//Author:Siddhartha Kakarla, Syracuse University                           //
//      (315) 440-5801, skakarla@syr.edu                                  //
//
/*
 *  Package Contents:
 *  -----------------
 *  This package defines 1 class:
 *  QueryHandler
 *    Processes the metadata query and gives back the result.
 *    
 *  Methods:
 *  MetaQuery() : Takes the user inputs like tags and categories and returns the contents of them along with file reference and categories.
 *  getXmlFiles() : Retrives the list of metadata files that belong to the given categories
 *
 *  Maintenace History:
 *  ver 1.0 : Nov 25, 2013
    first release
 */

using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentVault
{
  class QueryHandler
  {

    //Retrieves the contents of the tags mentioned in the query along with file reference and category names
    public List<List<string>> MetaQuery(List<List<string>> list)
    {
      List<string> categories = list[1];
      List<string> xmlfiles=getXmlFiles(categories);
      List<List<string>> results = new List<List<string>>();
      List<string> dummy;
      string temp;
      //string[] dummy;
      Directory.SetCurrentDirectory(@".\\metadata files");
      try
      {
        //XmlTextReader tr1 = null;
        //foreach (string metadaquery in list[0])
        //{
        //Console.Write("\nThe query is:{0}", metadaquery);
        foreach (string file in xmlfiles)
        {
          dummy = new List<string>();
          temp = Path.GetFileNameWithoutExtension(file);
          dummy.Add(temp+".txt");

          XDocument doc = XDocument.Load(file);
         var q = from x in
                doc.Elements("root")
                  .Elements("categories")
              .Descendants()
              select x;
          foreach (var elem in q)
          {
            dummy.Add((string)elem.Value+".xml");
          }    
          //results.Add(dummy);
          foreach (string metadaquery in list[0])
          {
            //dummy = new List<string>();
             q = from x in
                      doc.Elements("root")
                        .Elements(metadaquery)
                    //.Descendants()
                    select x;
             foreach (var elem in q)
             {
               dummy.Add((string)elem.Value);
             }

          }
          results.Add(dummy);
        }

      }

        
      catch (Exception exp)
      {
        Console.WriteLine(exp.Message);
      }

      var appDomain = AppDomain.CurrentDomain;
      string temp1 = appDomain.BaseDirectory;
      Directory.SetCurrentDirectory(temp1);
        return results;
     }//end of metadata search



    //retrieves all the metadata files that belong to the specified categories
    public List<string> getXmlFiles(List<string> categories)
    {

      string path = Directory.GetCurrentDirectory();
      DirectoryInfo path1 = Directory.GetParent(path);
      string path2=path1.ToString();
      path1 = Directory.GetParent(path2);
      path2 = path1.ToString() + "\\Testfiles\\xmlfiles";
      Directory.SetCurrentDirectory(path2);
       List<string> allxmlfiles=new List<string>();

      foreach (var item in categories)
      {
        XDocument doc = XDocument.Load((string)item+".xml");
        var q = from x in
                  doc.Elements("root")
                  //.Elements("reference")
                  .Descendants()
                select x;

        foreach (var elem in q)
        {
          allxmlfiles.Add((string)elem.Value);
        }


      }

      return allxmlfiles;


    }


#if(TEST_QueryHandler)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is QueryHandler!! ");
              QueryHandler obj=new QueryHandler();
              List<List<string>> list=new List<List<string>>();
              List<string> dummy=new List<string>();
              List<string> dummy1=new List<string>();
              
              dummy.Add("keywords");
              dummy.Add("dependencies");
              list.Add(dummy);
              dummy1.Add("personal");
              dummy1.Add("office");              
              list.Add(dummy1);                            
              obj.MetaQuery(List<List<string>> list)
              
        }
  
#endif


  }


  }



