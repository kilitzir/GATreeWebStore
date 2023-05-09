using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Data;
using GAtreeWebStore.Models;
using Newtonsoft.Json;
using System.IO;
using GAtreeWebStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;
/// <summary>
/// Parses the .arff file
/// </summary>

namespace GAtreeWebStore
{
    public static partial class Utils
    {
        private static ApplicationDBContext db; // to 
        /// <summary>
        /// adds a record to the data database based on URL
        /// </summary>
        /// <param name="newData"> The data given to the create data controller
        /// </param>
        /// <param name="db"> The database
        /// </param>

        /// <returns></returns>
        public static bool AddNewDataRecord(Data newData, DataAddFromFileViewModel StreamData, ApplicationDBContext _db)
        {
            db = _db;
            // Decompose the file 
            List<string> sHeaderList = new(); // The lines of the header
            List<string> sAttributesList = new(); // The lines with the attributes
            List<string> sDataList = new(); // the lines with the data
            string errorMessage = null;
            if (newData != null) {
                (sHeaderList, sAttributesList, sDataList, errorMessage) = DecomposeArffFromUrl(newData.Data_URL, null);
                if (errorMessage != string.Empty)
                {
                    LogError("AddNewRecordWithURL: error in AddNewRecordWithURL:" + errorMessage, db);
                    return false;
                }
            }
            else
            {
                (sHeaderList, sAttributesList, sDataList, errorMessage) = Utils.DecomposeArffFromUrl(null, StreamData.file1);
                newData = new() {
                    Data_friendlyname = StreamData.Data_friendlyname,
                    Data_notes = StreamData.Data_notes,
                    Data_URL = "(file) : " + StreamData.file1.FileName,
                }; 
            }
            newData.attributes_text = String.Join("\n\r", sAttributesList.ToArray());
            // TODO: check for error message
            List<DataAttributes> attributesList = new();
            //2. create the attributes classes
            (attributesList, errorMessage) = Utils.CreateAttributesClasses(sAttributesList);
            errorMessage = null;
            errorMessage = Utils.VerifyDataWithAttributes(attributesList, sDataList);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                LogError("AddNewRecordWithURL" + errorMessage,db);
                return false;
            }
            //3. create the json file
            (newData.Data_json, errorMessage) = Utils.CreateJsonFromData(attributesList, sDataList);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                LogError("AddNewRecordWithURL" + errorMessage,db);
                return false;
            }
            //4. we have all the data to save the data
            newData.Data_header = string.Join('\n', sHeaderList);
            _db.Add(newData);
            _db.SaveChanges();
            var Data_id = newData.Data_id;
            string guid = newData.Data_guid;
            foreach (var at in attributesList)
            {
                at.data_DataAttributes_data_ID = Data_id;
                at.DataAttributes_guid = guid;
                _db.Add(at);
            }
            _db.SaveChanges();
            return true;
        }
        /// <summary>
        /// Reads the URL, decomposes it and returns the appropriate structure in string format
        /// 
        /// </summary>
        /// <paramref name="URl"> the string with the URL
        /// </paramref>
        /// <returns>
        /// (sHeaderList, sAttributesList, sDataList, errorMessage)
        /// tuple with the Data, parameters and optionally an error message
        /// </returns>
        public static Tuple<List<string>, List<string>, List<string>, string> DecomposeArffFromUrl(string URl, IFormFile file1)
        {
            List<string> emptyList = new(); //retlist if error
            string[] lines;
            if (URl != null)
            {
                WebClient Client = new();
                string URLContent;
                try
                {
                    URLContent = Client.DownloadString(URl);
                }
                catch (Exception ex)
                {
                    return Tuple.Create(emptyList, emptyList, emptyList, "error - couldn't find the specidied URL : Error :" + ex.Message);
                }


                lines = URLContent.Split(
                                new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None
                                             );
            }
            else if (file1 != null)
            {
                using StreamReader reader = new(file1.OpenReadStream()); List<string> list1 = new();
                string line;
                while ((line = reader.ReadLine()) != null)
                    list1.Add(line);
                lines = list1.ToArray();
                reader.Close();
            }
            else
                return Tuple.Create(emptyList, emptyList, emptyList, "Error - no URL or stream");



            List<string> headerList = new(); // The lines of the header
            List<string> attributesList = new(); // The lines with the attributes
            List<string> dataList = new(); // the lines with the data
            string relation = null;
            bool Isheader = true;
            int linesCount = lines.Length;
            for (int i = 0; i < linesCount; i++)
            {
                string s = lines[i];
                if (Isheader)
                {
                    if (String.IsNullOrEmpty(s) || s.StartsWith("%"))
                    {
                        headerList.Add(s);
                    }
                    if (s.ToUpper().StartsWith("@RELATION"))
                    {
                        if (String.IsNullOrEmpty(relation))
                        {
                            s = s.Trim().Replace("\"", String.Empty);
                            relation = s;
                            continue;
                        }
                        else// Two relations!
                        {
                            Utils.AddLogging($"DecomposeArffFromUrl: two relations! Line: {i} : {s}", db);

                            return Tuple.Create(emptyList, emptyList, emptyList, "error, line:" + i + "  - two @RELATION");
                        }
                    }
                    if (s.ToUpper().StartsWith("@ATTRIBUTE"))
                    {
                        attributesList.Add(s);
                        continue;
                    }
                    if (s.ToUpper().StartsWith("@DATA"))
                    {
                        Isheader = false;
                        continue;
                    }
                } // if is header
                // we are in data
                if (String.IsNullOrEmpty(s) || s.StartsWith("%"))
                       continue;
                if (s.StartsWith("@ATTRIBUTE")) //error
                {
                    Utils.AddLogging($"DecomposeArffFromUrl: attribute inside data Line: {i} : {s}", db);
                    return Tuple.Create(emptyList, emptyList, emptyList, "error , line:" + i + "  attribute inside data!!");
                }
                dataList.Add(s);
            }// for (int i = 0; i < linesCount; i++)
            if (attributesList.Count == 0)
            {
                Utils.AddLogging($"DecomposeArffFromUrl: Attribute list is empty!!", db);
                return Tuple.Create(emptyList, emptyList, emptyList, "Attribute list is empty!!");
            }
            if (dataList.Count == 0) 
            {
                Utils.AddLogging($"DecomposeArffFromUrl: Data list is empty!!", db);
                return Tuple.Create(emptyList, emptyList, emptyList, "Data list is empty!!");
            }

            return Tuple.Create(headerList, attributesList, dataList, "");
        }
        /// <summary>
        /// Creates a list of attributes to be saved to database
        /// </summary>
        /// <param name="l"> list of lines with attributes</param>
        /// <returns>
        /// a list of classes Dataattributes, ready to be stored to the database. Well, y need to add the data_id first
        /// </returns>
        public static Tuple<List<Models.DataAttributes>, string> CreateAttributesClasses(List<string> slist)
        {
            List<Models.DataAttributes> emptyList = new();
            List<Models.DataAttributes> retList = new();
            int sn = 1;
            /// TODO: it seems like /t is embeddined in the string?!?!
            foreach (var s in slist)
            {
                //find the name;             
                string ss = s["@ATTRIBUTE ".Length..];
                string sname;
                if (ss[0] == '"') // the name is "quoted"  // TODO: ?: statement
                {
                    sname = ss.Split("\"")[0];
                    ss = ss[(sname.Length + 2)..];
                }
                else
                {
                    sname = ss.Split(new char[] { ' ', '\u0009' })[0]; //remove space or tab
                    ss = ss[(sname.Length + 1)..];
                }
                var tempAttribute = new DataAttributes {
                    DataAttributes_sn = sn, 
                    DataAttributes_name = sname
                };
                string atrName = null;
                if (sname.ToUpper() != "CLASS") // If it's not class
                {
                    ss = ss.Trim(new char[] { ' ', '\u0009' }); // TODO: regex remove trailing spaces and tabs
                    // find the attribute's type, remove any leading trailing "spaces" or tabs
                    atrName = ss.Split(new char[] { ' ', '\u0009' })[0].Substring(0, 4).ToUpper(); //TODO: regex expression
                }
                else
                    atrName = "CLAS"; // class is a specific type
                switch (atrName)
                {
                    case "NUME":
                    case "REAL":
                        tempAttribute.DataAttributes_type = "NUMERIC";
                        break;
                    case "DATE":
                        tempAttribute.DataAttributes_type = "DATE";
                        if (ss.Length > 1)
                        {
                            Tuple.Create(emptyList, emptyList, emptyList, "Date Attributes has not implemented - yet!!");
                        }
                        //tempAttribute.DataAttributes_attributesData = getDate(ss); //local function
                        break;
                    case "STRI":
                        tempAttribute.DataAttributes_type = "STRING";
                        break;
                    case "CLAS":
                        tempAttribute.DataAttributes_isClass = true;
                        tempAttribute.DataAttributes_type = "CLASS";
                        tempAttribute.DataAttributes_attributesData = string.Join(";", getAttributes(ss));
                        break;
                    default: // Nominal attributes
                        if (ss[0] != '{')
                            return Tuple.Create(emptyList, "CreateAttributesClasses: attribute name: " + sname + ": error found");
                        tempAttribute.DataAttributes_type = "NOMINAL";
                        tempAttribute.DataAttributes_attributesData = string.Join(";", getAttributes(ss));
                        break;
                }//switch
                sn++;
                retList.Add(tempAttribute);
            }//foreach
            return Tuple.Create(retList, " ");
            /// local function to get the date in different format TODO: implement it !!
            string getDate(string datetype)
            {
                return null;
            }
            /// Local function to get the list of allowed attributes
            /// It assumes that the attributes are in the form : { 'attr1', 'attr2'}
            /// It assumes that the attributes don't contain the character: " 
            List<string> getAttributes(string s)
            {
                List<string> ret = new();
                s = s.Replace("}", "").Replace("{", "").Replace("\t",""); 
                var ss = s.Split(",");
                foreach (var sss in ss)
                {
                    ret.Add(sss.Replace("\"", "").Replace("'", ""));
                }
                return ret;
            }
        }
        /// <summary>
        /// Checks the data against the attributes. Yes, it could be done during making the json, but we want a
        /// function to do one thing...
        /// </summary>
        /// <param name="attributesList">List of attributes classes</param>
        /// <param name="sDataList">list of lines of data</param>
        /// <returns>string with the error, otherwise null</returns>
        public static string VerifyDataWithAttributes(List<DataAttributes> attributesList, List<string> sDataList)
        {
            int length = attributesList.Count;
            foreach (var s in sDataList)
            {
                var ss = s.Split(',');
                if (ss.Length != length)
                {
                    Utils.LogError($"VerifyDataWithAttributes : ss in sDataList != count. s: {s}", db);
                    return (" verifyDataWithAttributes: error: different lengths in line: << " + s + " >>");
                }
                // check the line
                for (int i = 0; i < length; i++)
                {
                    if (ss[i] == "?")
                    { //undefined value
                        attributesList[i].DataAttributes_hasNullValues = true;
                        continue;
                    }
                    
                    switch (attributesList[i].DataAttributes_type)
                    {
                        case "NUMERIC":
                            float float1;
                            int int1;
                            if (int.TryParse(ss[i], out int1))
                            {
                                attributesList[i].data_DataAttributes_intMin = Math.Min(attributesList[i].data_DataAttributes_intMin, int1);
                                attributesList[i].data_DataAttributes_intMax = Math.Max(attributesList[i].data_DataAttributes_intMax, int1);

                            }
                            else
                            {
                                var cultureinfo = NormalizeFloatFromCulture(ss[i]); // Float can have comma or dot, i.e. 1,2 1.2
                                NumberStyles styles = NumberStyles.Number;

                                if (float.TryParse(ss[i], styles, cultureinfo, out float1))
                                {
                                    attributesList[i].data_DataAttributes_floatMin = Math.Min(attributesList[i].data_DataAttributes_floatMin, float1);
                                    attributesList[i].data_DataAttributes_floatMax = Math.Max(attributesList[i].data_DataAttributes_floatMax, float1);
                                }
                                else
                                    return (" error: float or int is expected in line: << " + s + " >>");
                            }
                            break;
                        case "STRING":
                            attributesList[i].DataAttributes_attributesData = ss[i];
                            break;
                        case "DATE":
                            break;
                        case "CLASS":
                            if (!attributesList[i].DataAttributes_attributesData.Contains(ss[i], StringComparison.CurrentCulture))
                                return ("VerifyDataWithAttributes: error: Undentified class param: << " + s + " >> in line:"+i);
                            break;
                        default:
                            break;
                    }
                }//for
            }//foeach
            MergevaluesInAttributes(attributesList);
            return null;
        }
        /// <summary>
        /// It return the culture info for the float. Float could either 1,3 or 1.3
        /// </summary>
        /// <param name="s"> the string with the float</param>
        /// <returns>Culture info</returns>
        private static CultureInfo NormalizeFloatFromCulture(string s)
        {
            if (Regex.IsMatch(s, @"^(:?[\d,]+\.)*\d+$") || Regex.IsMatch(s, @"^(:?[-\d,]+\.)*\d+$"))
            {
                return new CultureInfo("en-US");
            }
            // if the second regex matches, the number string is in DE culture
            if (Regex.IsMatch(s, @"^(:?[\d.]+,)*\d+$") || Regex.IsMatch(s, @"^(:?[-\d.]+,)*\d+$"))
            {
                return new CultureInfo("de-DE");
            }
            return CultureInfo.InvariantCulture;
        }
        /// <summary>
        /// merge values from int to float
        /// </summary>
        /// <param name="dataAttributes"> The attrivbute</param>
        private static void MergevaluesInAttributes(List<DataAttributes> attributesList)
        {
            // An attribute could be float but represent as an integer in one line. For ex. in line 10
            // it's 1 and in line 39 4.5. The line 10 would go to int but the line 39 will go to float.
            // So, if we have values in both int and float we have to merge these values to float and default the int
            for (int i=0;i<attributesList.Count;i++) 
            {
                var attribute = attributesList[i];
                if (DataAttributeHasFloatvalues(attribute)) // If float has values then we merge anyway ?!!!
                {
                    attribute.data_DataAttributes_floatMax = Math.Max(attribute.data_DataAttributes_floatMax, (float)attribute.data_DataAttributes_intMax);
                    attribute.data_DataAttributes_floatMin = Math.Min(attribute.data_DataAttributes_floatMin, (float)attribute.data_DataAttributes_intMin);
                    attribute.data_DataAttributes_intMin = int.MaxValue;
                    attribute.data_DataAttributes_intMax = int.MinValue;
                }
            }
            /// <summary>
            /// Function to verify for NUMERIC that both min and max has values
            /// </summary>
            /// <param name="attribute"></param>
            /// <returns>false if there is a problem</returns>
             bool DataAttributeHasFloatvalues(DataAttributes a)
            {

                if (a.DataAttributes_type != "NUMERIC")
                    return false;

                if (a.data_DataAttributes_floatMax != float.MinValue ||
                   a.data_DataAttributes_floatMin != float.MaxValue)
                    return true;

                return false;
            }
        }
        /// <summary>
        /// creates a Json file from the data
        /// </summary>
        /// <param name="attributesList"> the list with class attributes</param>
        /// <param name="data">the data in an array of strings</param>
        /// <returns>tuple(data in json, errormessage)</returns>
        public static Tuple<string, string> CreateJsonFromData(List<DataAttributes> attributesList, List<string> data)
        {
            // TODO: ADD try/catch()
            // We create data table because in the furure we may want to add it to the database.
            DataSet dataSet = new("dataSet")
            {
                Namespace = "NetFrameWork"
            };
            DataTable table = new()
            {
                TableName = "table1" // TODO: We need to have a meaningful name, somehow - and save it to data???
            };
            // create the columns
            foreach (var s in attributesList)
            {
                DataColumn itemColumn;
                itemColumn = new DataColumn(s.DataAttributes_name, typeof(string));
                table.Columns.Add(itemColumn);

            }//foreach
            dataSet.Tables.Add(table);
            // fillup the table with rows
            foreach (var s in data)
            {
                var ss = s.Split(","); //TODO: Special function to split for items with "" containing ,
                if (ss.Length != table.Columns.Count)
                {
                    return Tuple.Create("", " Error! : the columns between datatable and string don't match");
                }
                DataRow newRow = table.NewRow();
                for (int i = 0; i < ss.Length; i++)
                    newRow[i] = ss[i].Replace("\"", "");
                table.Rows.Add(newRow);

            }
            dataSet.AcceptChanges();
            string json = JsonConvert.SerializeObject(table, Formatting.None);
            dataSet.Dispose();
            return Tuple.Create(json, "");
        }
        /// <summary>
        /// Gets the errors from the functions
        /// </summary>
        /// <param name="s"></param>
        public static void LogError(String s, ApplicationDBContext db)
        {          
            var a = new Logging()
            {
                ErrorMessage = s
            };
            db.Add(a);
            db.SaveChanges();
        }
    
        /// <summary>
        /// Check if both numbers and floats have values - if they  both have values then something is wrong!
        /// return true if everything is OK.
        /// </summary>
        public static bool CheckIntAndFloat( int imin, int imax, float fmin, float fmax )
        {
            int i = 0;
            if (imin != int.MaxValue || imax != int.MinValue)
                i++;
            if (fmin != float.MaxValue || fmax != float.MinValue)
                i++;
            return i != 2;
        }
    }//class
}//namespace
