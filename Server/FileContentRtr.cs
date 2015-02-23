//////////////////////////////////////////////////////////////////////////////
///  FileContentRtr.cs  -  Retrives all infomation of text file        
//Author:Siddhartha Kakarla, Syracuse University                           //
//      (315) 440-5801, skakarla@syr.edu                                  //
//
/*
 *  Package Contents:
 *  -----------------
 *  This package defines 1 class:
 *  FileContentRtr
 *    Retrieves all the text, metadata content, parent, child files of an text file
 *    
 *  Methods:
 *  getContent() : takes file name as input and gets all text, metadata content, parent, child files of that text file
 *  
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
  class FileContentRtr
  {
    //Retrieves all the contents related to the specified text file
    public List<List<string>> getContent(string filename)
    {

      var appDomain = AppDomain.CurrentDomain;
      string temp1 = appDomain.BaseDirectory;
      Directory.SetCurrentDirectory(temp1);
      Directory.SetCurrentDirectory(@"..\\..\\TestFiles\\textfiles");
      string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory());

      List<List<string>> result = new List<List<string>>();
      List<string> dummy1 = new List<string>();
      List<string> dummy2 = new List<string>();
      List<string> dummy3 = new List<string>();



      string file;
      StreamReader reader;
      string filecontent = "";
      foreach (string file1 in allfiles)
      {
        file = Path.GetFileName(file1);
        if (filename == file)
        {
          reader = File.OpenText(file);
          filecontent = reader.ReadToEnd().ToLower().TrimEnd();
          // return filecontent;
        }
      }
      dummy1.Add(filecontent);
      result.Add(dummy1);

      Directory.SetCurrentDirectory(@"..\\xmlfiles\\metadata files");
      string[] allxmlfiles = Directory.GetFiles(Directory.GetCurrentDirectory());
      string xmlfile;
      string line = "";

      filename = Path.GetFileNameWithoutExtension(filename);

      foreach (string xmlfile1 in allxmlfiles)
      {
        xmlfile = Path.GetFileNameWithoutExtension(xmlfile1);
        if (filename == xmlfile)
        {
          StreamReader Reader = new StreamReader(xmlfile1);
          while ((line = Reader.ReadLine()) != null)
          {
            dummy2.Add(line);
          }
          XDocument doc = XDocument.Load(xmlfile1);
          var q = from x in
                    doc.Elements("root")
                      .Elements("dependencies")
                  .Descendants()
                  select x;
          foreach (var elem in q)
          {
            dummy3.Add((string)elem.Value + ".txt");
          }
          break;
        }
      }
      result.Add(dummy2);
      result.Add(dummy3);

       appDomain = AppDomain.CurrentDomain;
       temp1 = appDomain.BaseDirectory;
      Directory.SetCurrentDirectory(temp1);
      ParentTool parenttool = new ParentTool();
      parenttool.Tool();
      result.Add(parenttool.getParents(filename));



      return result;
    }


    
  #if(TEST_FileContentRtr)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is FileContentRtr!! ");
              FileContentRtr obj=new FileContentRtr();
              string filename="diary.txt";
              obj.getContent(filename);
              
        }
  
#endif



  }

}

