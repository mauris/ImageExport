using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ImageExport
{

    [Serializable]
    class ExportRule
    {
        public String Path { get; set;}
        public Size Dimension { get; set; }
    }
}
