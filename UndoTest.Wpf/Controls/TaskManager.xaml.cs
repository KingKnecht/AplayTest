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

namespace UndoTest.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for TaskManager.xaml
    /// </summary>
    public partial class TaskManager : UserControl
    {
        public TaskManager()
        {
            InitializeComponent();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            ((TaskManagerVm)DataContext).SetDescription(textBox.Text);
        }

        private void ButtonBase_OnClickAdd(object sender, RoutedEventArgs e)
        {
            ((TaskManagerVm) DataContext).AddNewTask();
        }

        private void ButtonBase_OnClickRemove(object sender, RoutedEventArgs e)
        {
            ((TaskManagerVm)DataContext).RemoveTask();
        }
    }
}
