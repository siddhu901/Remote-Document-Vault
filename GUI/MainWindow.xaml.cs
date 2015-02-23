///////////////////////////////////////////////////////////////////////////////
///  MyClient.cs  -  Client for RemoteDocumentVault.         
//Author:Siddhartha Kakarla, Syracuse University                           //
//      (315) 440-5801, skakarla@syr.edu                                  //
                //
/*
 *  Package Contents:
 *  -----------------
 *  This package defines 1 class:
 *  MainWindow.xaml.cs
 *    Defines the behavior of a generated xaml.cs file to handle the events in xaml.
 *    
 *  Methods:
 *  Cat: To display the categories
 *  textFiles(): To display the text files
 *  textQuery(): To display the query results
 *  editMetadata(): To send a message to server to edit the metadata file
 *  fileHandler(): To send a message to upload a text file and create metadata file
 *  fileContent(): To send a message to get the text, metadata,child,parents information of a text file
 *  
 * Required Files:
 * - Client:      Client.cs, Sender.cs, Receiver.cs
 * - Components:  ICommLib, AbstractCommunicator, BlockingQueue
 * - CommService: ICommService, CommService
 *
 *  Required References:
 *  - System.ServiceModel
 *  - System.RuntimeSerialization
 *
 *  Build Command:  devenv Project4HelpF13.sln /rebuild debug
 *
 *  Maintenace History:
 *  ver 1.0 : Nov 25, 2013
    first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DocumentVault;

namespace DocumentVault
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
          /*  winobj = this;
               MyClient mc = new MyClient();
                mc.getCategories();
               

              }

              internal static MainWindow winobj;

              //display the categories retrieved
            public void Cat(List<string> categories)
              {
                //get { return textboxrec.Text.ToString(); }
                Dispatcher.Invoke(

                  (Action)(() =>
                  {

                    listbox1.SelectionMode = SelectionMode.Multiple;
                    listbox2.SelectionMode = SelectionMode.Multiple;
                    //listboxcats.SelectionMode=
                    //filedownloadcats.SelectionMode = SelectionMode.Multiple;

                    foreach (string val in categories)
                    {
                      listbox1.Items.Add(val);
                      listbox2.Items.Add(val);
                      filedownloadcats.Items.Add(val);
                      editmdcats.Items.Add(val);
                      //listboxcats.Items.Add(val);
                    }
                  }));

              }

              //display the textfiles that belong to a particular category
              public void textFiles(List<string> textfiles)
              {

                Dispatcher.Invoke(

                (Action)(() =>
                {
                  textfileslb.SelectionMode = SelectionMode.Single;
                  textfileslb.Items.Clear();
                  textfile.SelectionMode = SelectionMode.Single;
                  textfile.Items.Clear();
                  foreach (string val in textfiles)
                  {
                    textfileslb.Items.Add(val);
                    textfile.Items.Add(val);
                    //files.Items.Add(val);
          
                  }

        
                }));


              }

              //display results if text query
              public void textQuery(List<List<string>> results)
              {

                Dispatcher.Invoke(

              (Action)(() =>
              {
                richta1.Document.Blocks.Clear();
                richta1.SelectAll();
                richta1.Selection.Text = "";
                richta.Document.Blocks.Clear();
                richta.SelectAll();
                richta.Selection.Text = "";
                int count = 0;
                foreach (List<string> s in results)
                {
                  foreach (string s1 in s)
                  {
                    if (count == 0)
                    {
                      richta1.AppendText("The tags belong to " + s1 + "  ,categories:");
                      //richta.AppendText("\n");
                      count++;
                      continue;
                    }
                    else if (s1.Contains(".txt"))
                    {
                      string s2 = s1.Substring(0, s1.Length - 4);
                      richta1.AppendText(s2 + " ,Contents:");
                    }
                    else
                    {
                      richta1.AppendText(s1);
                      richta1.AppendText("||");
                    }
                    //Environment.NewLine);

                  }
                  count = 0;
                  //p.Inlines.Add(b);
                  richta1.AppendText("\n");
                }
              }));



              }

              //display the results of metadata query
              public void Query(List<List<string>> results)
              {

                Dispatcher.Invoke(

                (Action)(() =>
                {
                  richta.Document.Blocks.Clear();

                  richta.SelectAll();
                  richta.Selection.Text = "";
                  richta1.Document.Blocks.Clear();

                  richta1.SelectAll();
                  richta1.Selection.Text = "";
                  int count = 0;
                  foreach (List<string> s in results)
                  {
                    foreach (string s1 in s)
                    {
                      if (count == 0)
                      {
                        richta.AppendText("The tags belong to " + s1 + "  ,categories:");
                        //richta.AppendText("\n");
                        count++;
                        continue;
                      }
                      else if (s1.Contains(".xml"))
                      {
                        string s2 = s1.Substring(0, s1.Length - 4);
                        richta.AppendText(s2 + " ,Contents:");
                      }
                      else
                      {
                        richta.AppendText(s1);
                        richta.AppendText("||");
                      }
                      //Environment.NewLine);

                    }
                    count = 0;
                    //p.Inlines.Add(b);
                    richta.AppendText("\n");
                  }
                }));
      
              }

              //event handler for button click
              private void Button_Click_1(object sender, RoutedEventArgs e)
              {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox
                if (result == true)
                {
                  // Open document
                  string filename = dlg.FileName;
                  FileNameTextBox.Text = filename;


                }



              }

              //retrieve the user inputs and invoke the fileHandler to upload a file and create a metadata file
              private void Button_Click_2(object sender, RoutedEventArgs e)
              {
                MyClient mc = new MyClient();


                List<string[]> l = new List<string[]>();

                string[] filename = { FileNameTextBox.Text };
                l.Add(filename);

                string[] filedeps;
                string guifiledeps = tb1.Text;
                char[] ch = { ',' };
                filedeps = guifiledeps.Split(ch);
                l.Add(filedeps);

                string keywordsstring = tb2.Text;
                string[] keywords = keywordsstring.Split(ch);
                l.Add(keywords);


                int i = 0;
                int count = listbox1.SelectedItems.Count;
                string[] categories = new string[count];
                foreach (var item in listbox1.SelectedItems)
                {
                  categories[i] = (string)item;
                  i++;
                }
                l.Add(categories);

                TextRange textRange = new TextRange(
                  // TextPointer to the start of content in the RichTextBox.
                  textarea.Document.ContentStart,
                  // TextPointer to the end of content in the RichTextBox.
                  textarea.Document.ContentEnd
              );
                string[] descptext = { textRange.Text };
                l.Add(descptext);

                mc.FileHandler(FileNameTextBox.Text, l);
              }

              //displays all the information of a text file
              public void filecontent(List<List<string>> contents)
              {
                Dispatcher.Invoke(

              (Action)(() =>
              {
                /*richta1.Document.Blocks.Clear();

                richta1.SelectAll();
                richta1.Selection.Text = "";*/
            /*
                filecontenttb.Text = String.Empty;
              filecontenttb.Text = contents[0][0];

              metadatacontentsb.Text = String.Empty;
              metadatatb.Text = String.Empty;
              foreach (string itr in contents[1])
              {
                metadatacontentsb.Text+=itr;
                metadatacontentsb.Text +="\n";
                metadatatb.Text+=itr;
                metadatatb.Text +="\n";
              }

              childslb.Items.Clear();
              foreach (string itr in contents[2])
              {
                childslb.Items.Add(itr);    
              }

              parentslb.Items.Clear();
              foreach (string itr in contents[3])
              {
                parentslb.Items.Add(itr);
              }
            }));

            }



      

    

            private void Button_Click_3(object sender, RoutedEventArgs e)
            {



            }

            private void FileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {

            }

            private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
            {

            }

            private void RichTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
            {

            }

            private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
            {

            }

            private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
            {

            }

            //event handler for button to trigger the processing of metadata query
            private void Button_Click_4(object sender, RoutedEventArgs e)
            {

              MyClient mc = new MyClient();
              int i = 0;
              int count = listbox2.SelectedItems.Count;
              //string[] categories = new string[count];
              List<string> categories = new List<string>();
              foreach (var item in listbox2.SelectedItems)
              {
                //categories[i] = (string)item;
                categories.Add((string)item);
                i++;
              }
     
              mc.QueryHandler(textbox2.Text, categories);



            }

            private void richta_TextChanged(object sender, TextChangedEventArgs e)
            {
              //richta.Document.Blocks.Clear();
            }

            private void textbox2_TextChanged(object sender, TextChangedEventArgs e)
            {

            }

           //display the result of edit metadata
            public void displayResult(string message)
            {

              Dispatcher.Invoke(

            (Action)(() =>
            {
              lable1.Content = message;
            }));

      
            }
            private void Button_Click_5(object sender, RoutedEventArgs e)
            {
              string filename = textfile.SelectedValue.ToString();
              Dialog md = new Dialog(filename);
      
              md.ShowDialog();

              MyClient mc = new MyClient();
              List<string> tag = new List<string>();
              tag.Add(md.textbox1.Text);
              tag.Add(md.textbox2.Text);
              tag.Add((string)textfile.SelectedValue);
              mc.EditMetadata(tag);
            }

            //event handler for buttin click which triggers the text query processing
            private void Button_Click_6(object sender, RoutedEventArgs e)
            {
              MyClient mc = new MyClient();
              bool matchallqueries=false;
              int i = 0;
              int count = listbox2.SelectedItems.Count;
              //string[] categories = new string[count];
              List<string> categories = new List<string>();
              foreach (var item in listbox2.SelectedItems)
              {
                //categories[i] = (string)item;
                categories.Add((string)item);
                i++;
              }

              if (searchall.IsEnabled)
              {
                matchallqueries = true;
              }
              mc.textQueryHandler( textbox4.Text , categories,matchallqueries);
    
            }

            private void textbox7_TextChanged(object sender, TextChangedEventArgs e)
            {

            }

            private void listbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {

            }

            private void ListBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
            {

            }

            //double click event handler for categories to trigger the text files retrieval process
            private void filedownloadcats_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
            {
              List<string> category = new List<string>();
              if (filedownloadcats.SelectedValue == null)
              {
                category.Add(editmdcats.SelectedValue.ToString());       
              }
              else
              {
                category.Add(filedownloadcats.SelectedValue.ToString());

              }
      
                    MyClient mc=new MyClient();
                    mc.getTextFiles(category);  

            }

            private void mainview(object sender, MouseButtonEventArgs e)
            {
              List<string> category = new List<string>();
              category.Add(filedownloadcats.SelectedValue.ToString());



              MyClient mc = new MyClient();
              mc.getTextFiles(category);  


            }

            private void Button_Click_7(object sender, RoutedEventArgs e)
            {
              string filename = textfileslb.SelectedValue.ToString();
              MyClient mc = new MyClient();
  

            }


            //double click event handler for textfile to trigger the text content retrieval process    
            private void textfileslb_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
            {
              string file;

              if (textfileslb.SelectedValue == null)
              {
                file = textfile.SelectedValue.ToString();
              }
              else
              {
                file = textfileslb.SelectedValue.ToString();
              }

              MyClient mc = new MyClient();
              mc.getTextContent(file);  
            

            }


            */


            /* public void listbox4_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
             {
               List<String> categories = new List<string>();
               if (listbox4.SelectedItem != null)
               {
                 foreach (string element in listbox4.SelectedItems)
                 {
                   categories.Add(element);
                 }
               }
               List<List<string>> l1 = new List<List<string>>();
               l1.Add(categories);
               VaultClient c = new VaultClient();
               c.getFiles(l1);
             }*/

        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //string filename = textfile.SelectedValue.ToString();
            Dialog md = new Dialog();

            md.ShowDialog();

           /* MyClient mc = new MyClient();
            List<string> tag = new List<string>();
            tag.Add(md.textbox1.Text);
            tag.Add(md.textbox2.Text);
            tag.Add((string)textfile.SelectedValue);
            mc.EditMetadata(tag);*/
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void FileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void listbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        

        private void editmdcats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RichTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void listbox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textbox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void richta_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }


        //test stub
#if(TEST_MainWindow)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is MainWindow.xaml.cs, this contains all the event handlers and other gu related funtions. ");
              
        }
  
#endif




    }

}





