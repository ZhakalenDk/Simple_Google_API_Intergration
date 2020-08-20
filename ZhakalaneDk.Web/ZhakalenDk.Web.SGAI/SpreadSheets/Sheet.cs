using Google.Apis.Sheets.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGAI.SpreadSheets
{
    /// <summary>
    /// Represents a Google Spreadsheet
    /// </summary>
    public class Sheet
    {
        /// <summary>
        /// Returns the scopes used to acces this sheet
        /// </summary>
        public string [] Scope { get; } = { SheetsService.Scope.Spreadsheets };

        /// <summary>
        /// The service that connects the application with Googles API
        /// </summary>
        public SheetsService Service { get; private set; }

        /// <summary>
        /// The object containing the info on the Spreadsheet
        /// </summary>
        public SheetInfo Info { get; set; }

        /// <summary>
        /// The name of the file containing the credentials
        /// </summary>
        private readonly string secretName = "client_secret.json";

        /// <summary>
        /// The credentials for accessing Googles API
        /// </summary>
        private GoogleCredential credentials;

        /// <summary>
        /// A collectons of functions to modify spreadsheet data
        /// </summary>
        public SheetOperations Modify { get; }

        /// <summary>
        /// Instantiate the sheet operation connection
        /// </summary>
        /// <param name="_appName">THe name of the app that want to acces the sheet</param>
        /// <returns></returns>
        public async Task InstantiateAsync ( string _appName )
        {
            await GetCredentials ();

            await ServiceInitiliazer ( _appName );
        }

        /// <summary>
        /// GEt the credentials needed to perform any operations on spreadsheet
        /// </summary>
        /// <returns></returns>
        private Task GetCredentials ()
        {
            //  Collects credential data from a {sercretName}.json file
            using ( var stream = new FileStream ( this.secretName, FileMode.Open, FileAccess.Read ) )
            {
                this.credentials = GoogleCredential.FromStream ( stream ).CreateScoped ( Scope );
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Initialize the service handler
        /// </summary>
        /// <param name="_appName">The name of the application that wants acces</param>
        /// <returns></returns>
        private Task ServiceInitiliazer ( string _appName )
        {
            Service = new SheetsService ( new Google.Apis.Services.BaseClientService.Initializer ()
            {
                HttpClientInitializer = credentials,
                ApplicationName = _appName
            } );

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_secretFileName">The name of the file containing the client secrets</param>
        /// <param name="_info">The info about the sheet</param>
        public Sheet ( string _secretFileName, SheetInfo _info )
        {
            this.secretName = _secretFileName;
            Info = _info;

            Modify = new SheetOperations ( this );
        }

        /// <summary>
        /// A class including functionality for modifying a spreadsheet 
        /// </summary>
        public class SheetOperations
        {
            private readonly Sheet sheet;

            internal SheetOperations ( Sheet _sheet )
            {
                this.sheet = _sheet;
            }

            /// <summary>
            /// Get a range of entries covered by the <see cref="Range"/> value. NOTE: Returns null if no entries are found.
            /// <list type="table">
            ///    <listheader>
            ///         <term>Returns</term>
            ///         <description>
            ///             <see cref="List{T}"/>, where <see cref="{T}"/> is of type <see cref="String"/>[],
            ///             in which each element in the array is a cell value of the sheet row
            ///         </description>
            ///     </listheader>
            /// </list>
            /// </summary>
            /// <param name="_range">THe range of which to target inside the sheet</param>
            /// <returns></returns>
            public async Task<List<string []>> GetEntriesAsync ( Range _range )
            {
                SpreadsheetsResource.ValuesResource.GetRequest request = this.sheet.Service.Spreadsheets.Values.Get ( this.sheet.Info.ID, $"{this.sheet.Info.Name}!{_range.ToString ()}" );

                ValueRange data = await request.ExecuteAsync ();

                IList<IList<object>> values = data.Values;

                List<string []> dataToReturn = new List<string []> ();

                int valueCount = GetValueCount ( _range );

                if ( values != null && values.Count > 0 )
                {
                    foreach ( var row in values )
                    {
                        string [] dataValues = new string [ valueCount ];
                        int index = 0;
                        foreach ( var cell in row )
                        {
                            dataValues [ index ] = cell.ToString ();
                            index++;
                        }

                        dataToReturn.Add ( dataValues );
                    }
                }
                else
                {
                    dataToReturn = null;
                }

                return dataToReturn;
            }

            /// <summary>
            /// Extract the cell count of a sheetrow
            /// </summary>
            /// <param name="_range">The range, which covers the cells to count</param>
            /// <returns></returns>
            private int GetValueCount ( Range _range )
            {
                int start = _range.Start [ 0 ];
                int end = _range.End [ 0 ];

                int count = end - start;

                return count + 1;
            }

            /// <summary>
            /// Insert a range of values into the spreadsheet
            /// </summary>
            /// <param name="_range">The range of which to insert the values</param>
            /// <param name="_values">The values to insert</param>
            /// <returns></returns>
            public async Task InsertEntryAsync ( Range _range, List<object> _values )
            {
                ValueRange valueRange = new ValueRange ();
                valueRange.Values = new List<IList<object>> { _values };

                var appendrequest = this.sheet.Service.Spreadsheets.Values.Append ( valueRange, sheet.Info.ID, $"{this.sheet.Info.Name}!{_range.ToString ()}" );

                appendrequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

                await appendrequest.ExecuteAsync ();
            }

            /// <summary>
            ///  Update a range of values in the spreadsheet
            /// </summary>
            /// <param name="_range">The range to update</param>
            /// <param name="_values">THe values to insert into the defined range</param>
            /// <returns></returns>
            public async Task UpdateEntryAsync ( Range _range, List<object> _values )
            {
                ValueRange valueRange = new ValueRange ();
                valueRange.Values = new List<IList<object>> { _values };

                var updateRequest = this.sheet.Service.Spreadsheets.Values.Update ( valueRange, this.sheet.Info.ID, $"{this.sheet.Info.Name}!{_range.ToString ()}" );
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

                await updateRequest.ExecuteAsync ();
            }

            /// <summary>
            /// Delete one or more entries in the spreadsheet, specified by the <see cref="DimensionRange"/> values.
            /// </summary>
            /// <param name="_startIndex">Where the <see cref="DimensionRange"/> begins. NOTE: This is a zero based index value</param>
            /// <param name="_endIndex">Where the <see cref="DimensionRange"/>ends. NOTE: This is a zero based index value</param>
            /// <param name="_dimension">The dimension on which the <see cref="DimensionRange"/> should operate. NOTE: ROWS or COLUMNS</param>
            /// <returns></returns>
            public async Task DeleteEntryAsync ( int _startIndex, int _endIndex, string _dimension )
            {
                BatchUpdateSpreadsheetRequest batchRequest = new BatchUpdateSpreadsheetRequest ();
                Request request = new Request ();
                DeleteDimensionRequest deleteRequest = request.DeleteDimension = new DeleteDimensionRequest ();

                //  Set the range of the deletion request
                DimensionRange range = deleteRequest.Range = new DimensionRange ();
                range.SheetId = this.sheet.Info.SheetID;
                range.Dimension = _dimension;
                range.StartIndex = _startIndex;
                range.EndIndex = _endIndex;

                List<Request> requests = new List<Request> { request };
                batchRequest.Requests = requests;

                await this.sheet.Service.Spreadsheets.BatchUpdate ( batchRequest, this.sheet.Info.ID ).ExecuteAsync ();
            }

            /// <summary>
            /// Delete a range of values in an entry, specified by the <see cref="Range"/> values
            /// </summary>
            /// <param name="_range">The range of which to clear values in</param>
            /// <returns></returns>
            public async Task DeleteValuesAsync ( Range _range )
            {
                ClearValuesRequest request = new ClearValuesRequest ();

                await this.sheet.Service.Spreadsheets.Values.Clear ( request, this.sheet.Info.ID, $"{this.sheet.Info.Name}!{_range.ToString ()}" ).ExecuteAsync ();
            }
        }
    }
}
