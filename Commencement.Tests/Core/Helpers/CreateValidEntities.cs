
using System;
using System.Collections.Generic;
using Commencement.Core.Domain;

namespace Commencement.Tests.Core.Helpers
{
    public static class CreateValidEntities
    {


        //public static Unit Unit(int? counter)
        //{
        //    var rtValue = new Unit();
        //    rtValue.FullName = "FullName" + counter.Extra();
        //    rtValue.ShortName = "ShortName";

        //    return rtValue;
        //}

        #region Helper Extension

        private static string Extra(this int? counter)
        {
            var extraString = "";
            if (counter != null)
            {
                extraString = counter.ToString();
            }
            return extraString;
        }

        #endregion Helper Extension

        
    }
}
