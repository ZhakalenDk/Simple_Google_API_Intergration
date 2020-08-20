using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGAI.SpreadSheets
{
    /// <summary>
    /// Represents a collection of values defining a spreadsheet
    /// </summary>
    public struct SheetInfo
    {
        /// <summary>
        /// The name of the spreadsheet
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// THe ID of the spreadsheet
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// THe ID of the targeted sheet inside the spreadsheet
        /// </summary>
        public int SheetID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name">The name of the spreadsheet</param>
        /// <param name="_range">The range that is targeted inside the spreadsheet</param>
        /// <param name="_ID">The ID of the spreadsheet</param>
        public SheetInfo ( string _name, string _ID )
        {
            Name = _name ?? throw new NullReferenceException ( "SheetInfo.Name can't be NULL" );
            ID = _ID;
            SheetID = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name">The name of the spreadsheet</param>
        /// <param name="_range">The range that is targeted inside the spreadsheet</param>
        /// <param name="_ID">The ID of the spreadsheet</param>
        /// <param name="_sheetID">The ID of the targeted sheet inside the spreadsheet</param>
        public SheetInfo ( string _name, string _ID, int _sheetID )
        {
            Name = _name ?? throw new NullReferenceException ( "SheetInfo.Name can't be NULL" );
            ID = _ID;
            SheetID = _sheetID;
        }

    }
}
