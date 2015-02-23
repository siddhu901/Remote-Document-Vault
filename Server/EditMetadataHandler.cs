//////////////////////////////////////////////////////////////////////////////
///  EditMetadataHandler.cs  -  Edit the metadata        
//Author:Siddhartha Kakarla, Syracuse University                           //
//      (315) 440-5801, skakarla@syr.edu                                  //
//
/*
 *  Package Contents:
 *  -----------------
 *  This package defines 1 class:
 *  EditMetadataHandler.cs
 *    Edits the metadata file based on the user input.
 *    
 *  Methods:
 *  editMetada() : Takes a tag name and value as input and if tag already exists then the new value replaces it. If tag doesn't exist a new tag is added.
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
  class EditMetadataHandler
  {
    //edits the metadata for a metadata file with the user provided inputs 
    public void editMetadata(List<string> tag)
    {
      bool newtag = true;
     /* string path = Directory.GetCurrentDirectory();
      DirectoryInfo path1 = Directory.GetParent(path);
      string path2 = path1.ToString();
      path1 = Directory.GetParent(path2);
      path2 = path1.ToString() + "\\Testfiles\\xmlfiles\\metadata files";
      Directory.SetCurrentDirectory(path2);*/

      var appDomain = AppDomain.CurrentDomain;
      string temp1 = appDomain.BaseDirectory;
      Directory.SetCurrentDirectory(temp1);

      Directory.SetCurrentDirectory(@"..\\.\\..\\Testfiles\\xmlfiles\\metadata files");
      string file = Path.GetFileNameWithoutExtension(tag[2])+".xml";

      XDocument doc = XDocument.Load(file);
      var q = from x in
                doc.Elements("root")
                  .Elements(tag[0])
              //.Descendants()
              select x;

      foreach (var elem in q)
      {
        elem.ReplaceWith(new XElement(tag[0], tag[1]));
        newtag = false;
      }
      if (newtag)
      {

        XElement ntag = doc.Descendants("root").FirstOrDefault();
        ntag.Add(new XElement(tag[0], tag[1]));
      }
      doc.Save(file);

    }



    #if(TEST_EditMetadataHandler)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is EditMetadataHandler!! ");
              EditMetadataHandler obj=new EditMetadataHandler();
              List<string> dummy=new List<string>();
              dummy.Add("description");
              obj.editMetadata(dummy);

              
        }
  
#endif




  }

}


