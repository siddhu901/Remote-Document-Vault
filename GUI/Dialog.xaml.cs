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
using System.Windows.Shapes;

namespace DocumentVault
{
  /// <summary>
  /// Interaction logic for Dialog.xaml
  /// </summary>
  public partial class Dialog : Window
  {
    public Dialog()
    {
      InitializeComponent();

    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }

#if(TEST_Dialog)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is Dialog.xaml.cs");
              
        }
  
#endif

}
