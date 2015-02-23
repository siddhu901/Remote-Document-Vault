//////////////////////////////////////////////////////////////////////////////
///  MetadataHandler.cs  -  Creates associated metadata file for a text file       
//Author:Siddhartha Kakarla, Syracuse University                           //
//      (315) 440-5801, skakarla@syr.edu                                  //
//
/*
 *  Package Contents:
 *  -----------------
 *  This package defines 1 class:
 *  MetadataHandler
 *    Generates an xml file for a text file
 *    
 *  Methods:
 *  createMetadata() : Takes the user inputs like keywords, dependencies and creates a xml file 
 *  
 *
 *  Maintenace History:
 *  ver 1.0 : Nov 25, 2013
    first release
 */


using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentVault
{
  class MetadataHandler
  {

    //creates the metadata form the inputs given from the user
    public void createMetadata(List<string[]> list)
    {

      string[] dummy = list[0];
      string xmlfile = dummy[0];
      string textfile = xmlfile;
      textfile = Path.GetFileName(textfile);
      string[] dependencies=list[1];
      string[] keywords = list[2];
      string[] cats = list[3];
        dummy = list[4];
      string descptext = dummy[0];
      
      XmlTextWriter tw = null;
      try
      {
        // attempt to create reader attached to an xml file in debug directory
        Directory.SetCurrentDirectory(@"..\\xmlfiles\\metadata files");
        xmlfile = Path.GetFileNameWithoutExtension(xmlfile);
        xmlfile += ".xml";
        tw = new XmlTextWriter(xmlfile, null);
        Console.Write("\nThe name of xml file created is: {0}", xmlfile);
        Console.WriteLine("\n  successfully opened {0}", xmlfile);
        tw.Formatting = Formatting.Indented;
        tw.WriteStartDocument();
        tw.WriteStartElement("root");
        tw.WriteStartElement("filename");
        tw.WriteString(textfile);
        tw.WriteEndElement();
        tw.WriteStartElement("time-date");
        tw.WriteString(DateTime.Now.ToString());
        tw.WriteEndElement();
        tw.WriteStartElement("Verison-no");
        tw.WriteString("1.0");
        tw.WriteEndElement();
        tw.WriteStartElement("description");
        tw.WriteString(descptext);
        tw.WriteEndElement();        
        tw.WriteStartElement("dependencies");
        foreach (string dependency in dependencies)
        {          tw.WriteElementString("child",dependency);
         }
        tw.WriteEndElement();         
        tw.WriteStartElement("categories");
        foreach (string category in cats)
        {          tw.WriteElementString("category", category);
                 }
        tw.WriteEndElement();
        tw.WriteStartElement("size");
        tw.WriteString("variable size!!");
        tw.WriteEndElement();

        
        tw.WriteStartElement("keywords");
        foreach (string keyword in keywords)
        {
          tw.WriteElementString("keyword",keyword);
          
        }
        tw.WriteEndElement();

        tw.Flush();
        tw.Close();
        string path = Directory.GetCurrentDirectory();
        string[] fileEntries = Directory.GetFiles(path, xmlfile);
        StreamReader Reader = new StreamReader(fileEntries[0]);
        string line;
        while ((line = Reader.ReadLine()) != null)
        {
          Console.Write("\n  {0}", line);
        }
        var appDomain = AppDomain.CurrentDomain;
        string temp1 = appDomain.BaseDirectory;
        Directory.SetCurrentDirectory(temp1);
      }
      catch (XmlException xmlexp)
      {
        Console.WriteLine
        (xmlexp.Message);
      }



    }


#if(TEST_MetadataHandler)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is MetadataHandler!! ");
              MetadataHandler obj=new MetadataHandler();
              List<string[]> list=new List<string[]>();
              string[] dummy1={"k1",k2"};
               string[] dummy2={"sjnsjndfjv"};
              string[] dummy3={"diary.txt"};
              list.Add(dummy1);
              list.Add(dummy2);
              list.Add(dummy3);                            
              obj.createMetadata(list)
              
        }
  
#endif

  }

}
