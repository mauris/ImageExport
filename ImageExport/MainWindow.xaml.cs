using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;

namespace ImageExport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ExportProject project = new ExportProject();

        public MainWindow()
        {
            InitializeComponent();
            this.lbxDisplay.DataContext = project.ExportRules;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.White);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] droppedFilePaths =
                e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string droppedFilePath in droppedFilePaths)
                {
                    ListBoxItem fileItem = new ListBoxItem();

                    fileItem.Content = System.IO.Path.GetFileNameWithoutExtension(droppedFilePath);
                    fileItem.ToolTip = droppedFilePath;
                }
            }
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.Blue);
        }

        private void Window_DragLeave(object sender, DragEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.White);
        }

        private void btnAddExportRule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExportRule rule = new ExportRule();
                rule.Path = txtPath.Text;
                rule.Dimension = new System.Drawing.Size(int.Parse(txtDimensionX.Text), int.Parse(txtDimensionY.Text));
                project.ExportRules.Add(rule);
            }
            catch (Exception ex)
            {

            }
            pnlAddRule.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnShowExportRuleForm_Click(object sender, RoutedEventArgs e)
        {
            if (pnlAddRule.Visibility == System.Windows.Visibility.Collapsed)
            {
                pnlAddRule.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                pnlAddRule.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void btnImportRules_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Title = "Import ImageExport Rule List";
            dlg.DefaultExt = ".erl"; // Default file extension
            dlg.Filter = "ImageExport Rule List (.irl)|*.irl|All files|*.*"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {   
                string filename = dlg.FileName;
                project.importProject(filename);
                txtOriginalFile.Text = project.OriginalFile;
            }

        }

        private void btnExportRules_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Title = "Export ImageExport Rule List";
            dlg.DefaultExt = ".irl"; // Default file extension
            dlg.Filter = "ImageExport Rule List (.irl)|*.irl|All files|*.*"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                project.exportProject(filename);
            }
        }

        private void btnBrowseOriginalFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = txtOriginalFile.Text;
            dlg.Title = "Select Original Image to Export";
            dlg.Filter = "Image Files (.jpg, .gif, .png, .jpeg)|*.jpg;*.gif;*.png;*.jpeg|All files|*.*"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                string filename = dlg.FileName;
                this.txtOriginalFile.Text = filename;
            }
        }

        private void txtOriginalFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            project.OriginalFile = txtOriginalFile.Text;
        }

        private void btnPerformExport_Click(object sender, RoutedEventArgs e)
        {
            project.execute();
        }

        private void lbxDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnRemoveSelectedRules.Visibility = System.Windows.Visibility.Collapsed;
            if (lbxDisplay.SelectedItems.Count > 0)
            {
                btnRemoveSelectedRules.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void btnRemoveSelectedRules_Click(object sender, RoutedEventArgs e)
        {
            while (lbxDisplay.SelectedItems.Count > 0)
            {
                project.ExportRules.Remove((ExportRule)lbxDisplay.SelectedItem);
            }
        }
    }
}
