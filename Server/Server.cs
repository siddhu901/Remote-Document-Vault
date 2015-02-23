///////////////////////////////////////////////////////////////////////////////
// Server.cs - Document Vault Server prototype                               //
//                                                                           //
//Author:Siddhartha Kakarla, Syracuse University                           //
//      (315) 440-5801, skakarla@syr.edu                                  //
//Source:      Jim Fawcett, CST 2-187, Syracuse University                //
///               (315) 443-3948, jfawcett@twcny.rr.com  
///////////////////////////////////////////////////////////////////////////////
/*
 *  Package Contents:
 *  -----------------
 *  This package defines four classes:
 *  Server:
 *    Provides prototype behavior for the DocumentVault server.
 *  EchoCommunicator:
 *    Simply diplays its messages on the server Console.
 *  QueryCommunicator:
 *    Deals with the queries  
 *  NavigationCommunicator:
 *    Serves as a placeholder for navigation processing.  You should be able to
 *    invoke your navigation processing from the ProcessMessages function.
 *   
 * FileContent :
 *    Responsible for facilitating retrieval of all the information of a text file. 

 * EditMetadataCommunicator 
      Responsible for facilitating metadata editing  
 *    
 * TextFilesCommunicator 
      Responsible for facilitating retrieval of text files that belong to a category
        
 *   FileInsertionCommunicator 
 *      Responsible for facilitating file uploading from client to server
 * 
 *  Required Files:
 *  - Server:      Server.cs, Sender.cs, Receiver.cs
 *  - Components:  ICommLib, AbstractCommunicator, BlockingQueue,EditMetadataHandler,FileContentRtr,Metadatahandler,QueryHandler,Server,TextQueryHandler
 *  - CommService: ICommService, CommService
 *  
 *
 *  Required References:
 *  - System.ServiceModel
 *  - System.RuntimeSerialization
 *  -Sender
 *  -Receiver
 *  -ParentTool
 *
 *  Build Command:  devenv Project4HelpF13.sln /rebuild debug
 *
 *  Maintenace History:
 *  ver 2.2 : Nov 25, 2013
 *   -added additional packages
 *  ver 2.1 : Nov 7, 2013
 *  - replaced ServerSender with a merged Sender class
 *  ver 2.0 : Nov 5, 2013
 *  - fixed bugs in the message routing process
 *  ver 1.0 : Oct 29, 2013
 *  - first release
 */
using System;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IO;

namespace DocumentVault
{
  // Echo Communicator

  class EchoCommunicator : AbstractCommunicator
  {
    protected override void ProcessMessages()
    {
      while (true)
      {
        ServiceMessage msg = bq.deQ();
        Console.Write("\n  {0} Recieved Message:\n", msg.TargetCommunicator);
        msg.ShowMessage();
        Console.Write("\n  Echo processing completed\n");
        if (msg.Contents == "quit")
          break;
        else if (msg.ResourceName == "catagories")
        {
          Category c = new Category();
          List<string> cat = c.getCat();
          ServiceMessage reply = ServiceMessage.MakeCategoryMessage("client-echo", "echo", cat, "reply from categories");
          reply.TargetUrl = msg.SourceUrl;
          reply.SourceUrl = msg.TargetUrl;
          AbstractMessageDispatcher dispatcher = AbstractMessageDispatcher.GetInstance();
          dispatcher.PostMessage(reply);
          break;

        }
      }
    }
  }
  class Category
  {
    //returns the categories of the entire fle set
    public List<string> getCat()
    {
      List<string> cats = new List<string>();
      XDocument doc = XDocument.Load(@"..\..\Testfiles\root.xml");
      var q = from x in
                doc.Elements("root")
                .Elements("categories")
                .Descendants()
              select x;
      //string allcat = "";
      foreach (var elem in q)
      {
        cats.Add((string)elem.Value);
      }
      return cats;
    }
  }
  // Query Communicator

  class QueryCommunicator : AbstractCommunicator
  {
    //initiates the QueryHandler for metadata query processing and text query processing
    protected override void ProcessMessages()
    {
      while (true)
      {
        ServiceMessage msg = bq.deQ();

        if (!msg.ResourceName.Equals("textquery"))
        {
          QueryHandler queryhandler = new QueryHandler();
          List<List<string>> results = queryhandler.MetaQuery(msg.nlist);

          Console.Write("\n  {0} Recieved Message:\n", msg.TargetCommunicator);
          msg.ShowMessage();
          //Console.Write("\n  Query processing is an exercise for students\n");
          if (msg.Contents == "quit")
            break;
          ServiceMessage reply = ServiceMessage.MakeQueryMessage("client-echo", "query", results, "reply from query");
          reply.TargetUrl = msg.SourceUrl;
          reply.SourceUrl = msg.TargetUrl;
          AbstractMessageDispatcher dispatcher = AbstractMessageDispatcher.GetInstance();
          dispatcher.PostMessage(reply);
        }

        else
        {
          TextQueryHandler textqueryhandler = new TextQueryHandler();
          List<List<string>> results = textqueryhandler.textQuery(msg.nlist);
          if (msg.Contents == "quit")
            break;
          ServiceMessage reply = ServiceMessage.MakeQueryMessage("client-echo", "query", results, "reply from tquery");
          reply.TargetUrl = msg.SourceUrl;
          reply.SourceUrl = msg.TargetUrl;
          AbstractMessageDispatcher dispatcher = AbstractMessageDispatcher.GetInstance();
          dispatcher.PostMessage(reply);
        }
      }
    }

    class EditMetadataCommunicator : AbstractCommunicator
    {
      //initiates the EditMetadataHandler for editing metadata
      protected override void ProcessMessages()
      {
        while (true)
        {
          ServiceMessage msg = bq.deQ();

          EditMetadataHandler queryhandler = new EditMetadataHandler();
          //string message = 
          queryhandler.editMetadata(msg.tag);

          //Console.Write("\n  {0} Recieved Message:\n", msg.TargetCommunicator);
          //msg.ShowMessage();
          //Console.Write("\n  Query processing is an exercise for students\n");
          if (msg.Contents == "quit")
            break;
          ServiceMessage reply = ServiceMessage.MakeMessage("client-echo", "query", " ", "reply from editmetadata");
          reply.TargetUrl = msg.SourceUrl;
          reply.SourceUrl = msg.TargetUrl;
          AbstractMessageDispatcher dispatcher = AbstractMessageDispatcher.GetInstance();
          dispatcher.PostMessage(reply);
        }
      }
    }



    //file Communicator
    class FileInsertionCommunicator : AbstractCommunicator
    {
      //initiates the FileHandler o create a new for the uploaded file and create corresponding metadata file
      protected override void ProcessMessages()
      {

        
        while (true)
        {
          
          ServiceMessage msg = bq.deQ();
          var appDomain = AppDomain.CurrentDomain;
          string temp1 = appDomain.BaseDirectory;
          Directory.SetCurrentDirectory(temp1);
          Directory.SetCurrentDirectory(@"..\\Testfiles\\textfiles");
          if (msg.ResourceName != "metadata")
          {
            string fileName = msg.ResourceName.Remove(msg.ResourceName.IndexOf(","));
            FileStream fs = null;

            if (File.Exists(fileName))
            {
              byte[] block_data = Encoding.UTF8.GetBytes(msg.Contents);
              fs = File.Open(fileName, FileMode.Append, FileAccess.Write);
              fs.Write(block_data, 0, block_data.Length);
            }
            else
            {
              byte[] block_data = Encoding.UTF8.GetBytes(msg.Contents);
              fs = File.Open(fileName, FileMode.Create, FileAccess.Write);
              fs.Write(block_data, 0, block_data.Length);

            }

            fs.Close();
          }

          else
          {

            MetadataHandler mdhandler = new MetadataHandler();
            mdhandler.createMetadata(msg.list);
          }









        }


      }
    }

    // Navigate Communicator
    class NavigationCommunicator : AbstractCommunicator
    {
      protected override void ProcessMessages()
      {
        while (true)
        {
          ServiceMessage msg = bq.deQ();
          Console.Write("\n  {0} Recieved Message:\n", msg.TargetCommunicator);
          msg.ShowMessage();
          Console.Write("\n  Navigation processing is an exercise for students\n");
          if (msg.Contents == "quit")
            break;
          ServiceMessage reply = ServiceMessage.MakeMessage("client-echo", "nav", "reply from nav");
          reply.TargetUrl = msg.SourceUrl;
          reply.SourceUrl = msg.TargetUrl;
          AbstractMessageDispatcher dispatcher = AbstractMessageDispatcher.GetInstance();
          dispatcher.PostMessage(reply);
        }
      }
    }

    class FileContent : AbstractCommunicator
    {
      //initiates the FileContentRtr to get all info of a text file      
      protected override void ProcessMessages()
      {
        while (true)
        {
          ServiceMessage msg = bq.deQ();

          FileContentRtr filecontent = new FileContentRtr();
          //string message = 
          List<List<string>> result=filecontent.getContent(msg.filename);
              /*  var appDomain = AppDomain.CurrentDomain;
      string temp1 = appDomain.BaseDirectory;
      Directory.SetCurrentDirectory(temp1);*/
          //Console.Write("\n  {0} Recieved Message:\n", msg.TargetCommunicator);
          //msg.ShowMessage();
          //Console.Write("\n  Query processing is an exercise for students\n");
          if (msg.Contents == "quit")
            break;
          ServiceMessage reply = ServiceMessage.MakeQueryMessage("client-echo", "filecontent", result, "reply from filecontent");
          reply.TargetUrl = msg.SourceUrl;
          reply.SourceUrl = msg.TargetUrl;
          AbstractMessageDispatcher dispatcher = AbstractMessageDispatcher.GetInstance();
          dispatcher.PostMessage(reply);
        }
      }
    }
    
    //not using this
    public void getFileContent(ServiceMessage msg)
    {
      Server s = new Server();
      //s.GetFileContent(msg);
      string file = msg.Contents;
      long blockSize = 512;

      string filename = @"..\\DocumentVault\\" + file;
      try
      {
        FileStream fs;
        fs = File.Open(filename, FileMode.Open, FileAccess.Read);
        int bytesRead = 0;
        while (true)
        {
          long remainder = (int)(fs.Length - fs.Position);
          if (remainder == 0)
            break;
          long size = Math.Min(blockSize, remainder);
          byte[] block = new byte[size];
          bytesRead = fs.Read(block, 0, block.Length);
          ServiceMessage msg4 =
            ServiceMessage.MakeMessage("client-echo", "file_Comm", System.Text.Encoding.Default.GetString(block), "FileContent");
          msg4.TargetUrl = msg.SourceUrl;
          msg4.SourceUrl = msg.TargetUrl;
          AbstractMessageDispatcher dispatcher1 = AbstractMessageDispatcher.GetInstance();
          dispatcher1.PostMessage(msg4);
        }
        fs.Flush();
        fs.Close();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }

      class TextFilesCommunicator : AbstractCommunicator
    {
      //initiates the TextQueryHandler to get the text files that belong to a category 
      protected override void ProcessMessages()
      {
        while (true)
        {
          ServiceMessage msg = bq.deQ();
          Console.Write("\n  {0} Recieved Message:\n", msg.TargetCommunicator);
          msg.ShowMessage();
          TextQueryHandler textqueryhandler = new TextQueryHandler();
          /*List<List<string>> dummy = new List<List<string>>();
          dummy.Add(msg.category);*/
          List<string> textfiles = textqueryhandler.getTextFiles(msg.category);
          ServiceMessage reply = ServiceMessage.MakeTextFilesMessage("client-echo","textfiles", textfiles, "reply from textfiles");
          reply.TargetUrl = msg.SourceUrl;
          reply.SourceUrl = msg.TargetUrl;
          AbstractMessageDispatcher dispatcher = AbstractMessageDispatcher.GetInstance();
          dispatcher.PostMessage(reply);
          //break;
        }
      }


    }



    // Server

    class Server
    {
      static void Main(string[] args)
      {
        Console.Write("\n  Starting CommService");
        Console.Write("\n ======================\n");

        string ServerUrl = "http://localhost:8000/CommService";
        Receiver receiver = new Receiver(ServerUrl);

        string ClientUrl = "http://localhost:8001/CommService";

        Sender sender = new Sender();
        sender.Name = "sender";
        sender.Connect(ClientUrl);
        receiver.Register(sender);
        sender.Start();

        // Test Component that simply echos message

        EchoCommunicator echo = new EchoCommunicator();
        echo.Name = "echo";
        receiver.Register(echo);
        echo.Start();

        // Placeholder for query processor

        QueryCommunicator query = new QueryCommunicator();
        query.Name = "query";
        receiver.Register(query);
        query.Start();

        FileContent filecontent = new FileContent();
        filecontent.Name = "filecontent";
        receiver.Register(filecontent);
        filecontent.Start();

        EditMetadataCommunicator editmetadata = new EditMetadataCommunicator();
        editmetadata.Name = "metadataedit";
        receiver.Register(editmetadata);
        editmetadata.Start();
        // Placeholder for component that searches for and returns 
        // parent/child relationships

        TextFilesCommunicator textfiles = new TextFilesCommunicator();
        textfiles.Name = "textfiles";
        receiver.Register(textfiles);
        textfiles.Start();

        NavigationCommunicator nav = new NavigationCommunicator();
        nav.Name = "nav";
        receiver.Register(nav);
        nav.Start();

        FileInsertionCommunicator fic = new FileInsertionCommunicator();
        fic.Name = "file";
        receiver.Register(fic);
        fic.Start();

        Console.Write("\n  Started CommService - Press key to exit:\n ");
        Console.ReadKey();
      }
    }


  }
}



