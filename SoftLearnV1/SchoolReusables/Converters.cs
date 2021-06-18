using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.SchoolReusables
{
    public class Converters
    {
        public static EnumUtility.Gender stringToGender(string genderString)
        {
            EnumUtility.Gender enumObj = EnumUtility.Gender.Male;
            switch (genderString.Trim().ToLower())
            {
                case "f":
                    enumObj = EnumUtility.Gender.Female;
                    break;
                case "m":
                    enumObj = EnumUtility.Gender.Male;
                    break;
            }
            return enumObj;
        }        
    }
}
