using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGAI.SpreadSheets
{
    interface IMySheetEntry
    {
        int Row { get; set; }
        char Column { get; set; }
    }
}
