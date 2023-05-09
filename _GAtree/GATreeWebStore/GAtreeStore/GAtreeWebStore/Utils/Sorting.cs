using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAtreeWebStore.Models;
using Newtonsoft.Json;
using GAtreeWebStore.ViewModels;
/// <summary>
///  Sorting classes - used in Utils
/// </summary>

// classes for sorting
namespace GAtreeWebStore
{
    //used in sorting the list DataAttributesWebApiModel
    class SortDataAttributesWebApiModel : IComparer<DataAttributesWebApiModel>
    {
        public int Compare(DataAttributesWebApiModel x, DataAttributesWebApiModel y)
        {
            if (x.sn == 0 || y.sn == 0)
            {
                return 0;
            }
            return x.sn.CompareTo(y.sn);

        }
    }
    /// <summary>
    /// Sorting a list of DataAttributes
    /// </summary>
    class SortDataAttributes : IComparer<DataAttributes>
    {
        public int Compare(DataAttributes x, DataAttributes y)
        {
            /// TODO maybe is wrong?
            if (x.DataAttributes_sn == 0 || y.DataAttributes_sn == 0)
            {
                return 0;
            }
            return x.DataAttributes_sn.CompareTo(y.DataAttributes_sn);
        }
    }
    class SortTestDataRunLog : IComparer<TestDataRunLog>
    {
        public int Compare(TestDataRunLog x, TestDataRunLog y)
        {
            /// TODO maybe is wrong?
            if (x.Test_data_run_id == 0 || y.Test_data_run_id == 0)
            {
                return 0;
            }
            return x.Test_data_run_id.CompareTo(y.Test_data_run_id);
        }
    }
    class SortJsonTree : IComparer<JsonTree>
    {
        public int Compare(JsonTree x, JsonTree y)
        {
            double xx = Double.Parse(x.evaluation);
            double yy = Double.Parse(y.evaluation);
            return xx.CompareTo(yy); // xx-yy?
        }
    }


}
