///////////////////////////////////////////////////////////////////////////////
///  MyClient.cs  -  Client for RemoteDocumentVault.         
//Author:Siddhartha Kakarla, Syracuse University                           //
//      (315) 440-5801, skakarla@syr.edu                                  //
//
/*
 *  Package Contents:
 *  -----------------
 *  This package defines 1 class:
 *  ParentTool.cs
 *    Keeps the track of all paarent files for each and evry file.
 *    
 *  Methods:
 *  Tool() : will scan the whole database of files to create a datasturcture of key-values (key:file, values:list of parents)
 *  getParents(): query the datastruture of the tool to retrive the parents of a particular file.  
 *  
 *
 *  Build Command:  devenv Project4HelpF13.sln /rebuild debug
 *
 *  Maintenace History:
 *  ver 1.0 : Nov 25, 2013
    first release
 */



using System;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentVault
{
  public class ParentTool
  {

    Dictionary<string, List<string>> parentracker = new Dictionary<string, List<string>>();
    static void Main(string[] args)
    {

      new ParentTool().Tool();


    }

    //scans all the files and records the parents for each file
    public void Tool()
    {

      Directory.SetCurrentDirectory(@"..\\..\\..\\Server\\Testfiles\\xmlfiles\\metadata files");

      string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory());

      foreach (string file1 in allfiles)
      {

        string file = Path.GetFileName(file1);
        List<string> dummy = new List<string>();
        List<string> dummy1 = new List<string>();

        dummy.Add(file);
        XDocument doc = XDocument.Load(file);
        var q = from x in
                  doc.Elements("root")
                    .Elements("dependencies")
                .Descendants()
                select x;

        foreach (var item in q)
        {
          if (parentracker.ContainsKey((string)item))
          {
            List<string> newList = parentracker[(string)item];
            if (newList.Contains(file)) { break; }
            newList.Add(file);
            parentracker.Remove((string)item);
            parentracker.Add((string)item, newList);
          }

          else
            parentracker.Add((string)item, dummy);
        }
      }


      foreach (KeyValuePair<string, List<string>> itr in parentracker)
      {
        Console.Write("\n file: {0}", (string)itr.Key);
        foreach (string itr1 in itr.Value)
        {
          Console.Write("Parents: {0}", itr1);
        }
      }

    }

    //get the list pf parents of a spoecified text file
    public List<string> getParents(string filename)
    {
      List<string> parents = new List<string>();
      foreach (KeyValuePair<string, List<string>> itr in parentracker)
      {
        if (((string)itr.Key).Equals(filename))
        {
          foreach (string itr1 in itr.Value)
          {
            parents.Add(itr1);
          }
          break;
        }

       
      }
      return parents;
    }



#if(TEST_ParentTool)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is ParentTool!! ");
              ParentTool parenttool=new ParentTool();
              parenttool.Tool();
              parenttool.getParents("girlfriends.txt");
              
        }
  
#endif




  }





}