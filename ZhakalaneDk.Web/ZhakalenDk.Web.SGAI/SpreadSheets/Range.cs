using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGAI.SpreadSheets
{
    /// <summary>
    /// Represents a range in a spreadsheet
    /// </summary>
    public struct Range
    {
        /// <summary>
        /// The beginning of the range
        /// </summary>
        public string Start { get; }
        /// <summary>
        /// THe end of the range
        /// </summary>
        public string End { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cell">The cell this range should target</param>
        public Range ( string _cell )
        {
            Start = _cell;
            End = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_start">Where the range should begin. Ex: A1</param>
        /// <param name="_end">Where the range should end. Ex: J1</param>
        public Range ( string _start, string _end )
        {
            Start = _start;
            End = _end;
        }

        /// <summary>
        /// Returns a string with the following format: "{<see cref="Start"/>}:{<see cref="End"/>}"
        /// </summary>
        /// <returns></returns>
        public override string ToString ()
        {
            if ( End == string.Empty )
            {
                return $"{Start}";
            }

            return $"{Start}:{End}";
        }
    }
}
