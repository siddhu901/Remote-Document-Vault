/////////////////////////////////////////////////////////////////////////////
// ICommServiceLib.cs - interface for Document Vault communication service //
//                                                                         //
//Author:Siddhartha Kakarla, Syracuse University                           //
//      (315) 440-5801, skakarla@syr.edu                                  //
//Source:      Jim Fawcett, CST 2-187, Syracuse University                //
///               (315) 443-3948, jfawcett@twcny.rr.com                   //
/////////////////////////////////////////////////////////////////////////////
/*
 *  Package Contents:
 *  -----------------
 *  This package defines the service contract, ICommService, for messaging and 
 *  defines the class ServiceMessage as a data contract.
 *  
 * Methods: ShowMessage(): For displaying message ; MakeMessage(-,-,-,-),MakeTextContentMessage(-,-,-,-) etc :To create messages which are sent to server.
 * 
 *
 *  Required Files:
 *  - None
 *
 *  Required References:
 *  - System.ServiceModel
 *  - System.Runtime.Serialization
 *
 *  Build Command:  devenv Project4HelpF13.sln /rebuild debug
 *
 *  Maintenance History:
 *  --------------------
 *  ver 1.0 : Oct 29, 2013 
 *  - first release
 *  ver 1.1 : Nov 25, 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DocumentVault
{
  [ServiceContract]
  public interface ICommService
  {
    [OperationContract(IsOneWay = true)]
    void PostMessage(ServiceMessage msg);
  }

  [DataContract]
  public class ServiceMessage
  {
    [DataMember]
    public string TargetUrl { get; set; }
    [DataMember]
    public string SourceUrl { get; set; }
    [DataMember]
    public string TargetCommunicator { get; set; }
    [DataMember]
    public string SourceCommunicator { get; set; }
    [DataMember]
    public string Contents { get; set; }
    [DataMember]
    public string ResourceName { get; set; }
    [DataMember]
    public List<string[]> list { get; set; }
    [DataMember]
    public List<List<string>> nlist { get; set; }
    [DataMember]
    public List<string> categories { get; set; }
    [DataMember]
    public List<string> tag { get; set; }
    [DataMember]
    public List<string> category { get; set; }
    [DataMember]
    public string filename { get; set; }
    

    public void ShowMessage()
    {
      Console.Write(
        "\n  Message Contents:\n  TargetUrl:\t{0}\n  SourceUrl:\t{1}\n  TargetCommunicator:\t{2}\n  SourceCommunicator:\t{3}\n  contents:\t{4}\n  name:\t{5}\n",
        TargetUrl, SourceUrl, TargetCommunicator, SourceCommunicator, Contents, ResourceName
      );
    }
    public static void ShowText(string text)
    {
      Console.Write("\n  {0}", text);
    }

    public static ServiceMessage MakeMessage(string TargetCommunicator, string SourceCommunicator, string contents, string resName = "")
    {
      ServiceMessage msg = new ServiceMessage();
      msg.TargetCommunicator = TargetCommunicator;
      msg.SourceCommunicator = SourceCommunicator;
      msg.ResourceName = resName;
      msg.Contents = contents;
      return msg;
    }

    public static ServiceMessage MakeTextContentMessage(string TargetCommunicator, string SourceCommunicator, string contents, string resName = "")
    {
      ServiceMessage msg = new ServiceMessage();
      msg.TargetCommunicator = TargetCommunicator;
      msg.SourceCommunicator = SourceCommunicator;
      msg.ResourceName = resName;
      msg.filename = contents;
      return msg;
    }
    public static ServiceMessage MakeTextFilesMessage(string TargetCommunicator, string SourceCommunicator, List<string> contents, string resName = "")
    {
      ServiceMessage msg = new ServiceMessage();
      msg.TargetCommunicator = TargetCommunicator;
      msg.SourceCommunicator = SourceCommunicator;
      msg.ResourceName = resName;
      msg.category = contents;
      return msg;
    }

    public static ServiceMessage MakeCategoryMessage(string TargetCommunicator, string SourceCommunicator, List<string> contents, string resName = "")
    {
      ServiceMessage msg = new ServiceMessage();
      msg.TargetCommunicator = TargetCommunicator;
      msg.SourceCommunicator = SourceCommunicator;
      msg.ResourceName = resName;
      msg.categories = contents;
      return msg;
    }





    public static ServiceMessage MakeMDEditMessage(string TargetCommunicator, string SourceCommunicator, List<string> tag, string resName = "")
    {
      ServiceMessage msg = new ServiceMessage();
      msg.TargetCommunicator = TargetCommunicator;
      msg.SourceCommunicator = SourceCommunicator;
      msg.ResourceName = resName;
      msg.tag = tag;
      return msg;


    }

    public static ServiceMessage MakeQueryMessage(string TargetCommunicator, string SourceCommunicator, List<List<string>> list, string resName = "")
    {
      ServiceMessage msg = new ServiceMessage();
      msg.TargetCommunicator = TargetCommunicator;
      msg.SourceCommunicator = SourceCommunicator;
      msg.ResourceName = resName;
      msg.nlist = list;
      return msg;
    }


    public static ServiceMessage MakeFileMessage(string TargetCommunicator, string SourceCommunicator, List<string[]> list, string resName = "")
    {
      ServiceMessage msg = new ServiceMessage();
      msg.TargetCommunicator = TargetCommunicator;
      msg.SourceCommunicator = SourceCommunicator;
      msg.ResourceName = resName;
      msg.list = list;
      return msg;
    }
  }
}