using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageExport
{
    class ExportProject
    {
        const int VERSION = 1;

        private string sOriginalFile = "";
        public String OriginalFile
        {
            get { return sOriginalFile; }
            set { sOriginalFile = value; }
        }

        private ObservableCollection<ExportRule> cExportRules = new ObservableCollection<ExportRule>();
        public ObservableCollection<ExportRule> ExportRules {
            get { 
                return cExportRules;
            } 
        }

        public void exportProject(string filename)
        {
            IEnumerable<ExportRule> obsCollection = (IEnumerable<ExportRule>)cExportRules;
            List<ExportRule> list = new List<ExportRule>(obsCollection);

            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, VERSION);
            bFormatter.Serialize(stream, sOriginalFile);
            bFormatter.Serialize(stream, list);
            stream.Close();
        }

        public void importProject(string filename)
        {
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            int version = (int)bFormatter.Deserialize(stream);
            sOriginalFile = (string)bFormatter.Deserialize(stream);
            List<ExportRule> list = (List<ExportRule>)bFormatter.Deserialize(stream);
            stream.Close();
            cExportRules.Clear();
            foreach (ExportRule rule in list)
            {
                cExportRules.Add(rule);
            }
        }

        public void execute()
        {
            Image original = Image.FromFile(this.sOriginalFile);
            foreach (ExportRule rule in ExportRules)
            {
                Image export = resizeImage(original, rule.Dimension);
                string path = rule.Path;
                if (!Path.IsPathRooted(path))
                {
                    path = Path.Combine(Path.GetDirectoryName(this.OriginalFile), path);
                }
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                export.Save(path);
            }
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            Bitmap newImage = new Bitmap(size.Width, size.Height);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(imgToResize, new Rectangle(0, 0, size.Width, size.Height));
            }
            return (Image)newImage;            
        }

    }
}
