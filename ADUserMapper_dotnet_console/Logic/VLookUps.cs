using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADUserMapper_dotnet_console.Logic
{
    public static class VLookUps
    {
        public static string StateLookUp(string abbreviation)
        {
            switch (abbreviation)
            {
                case "AL":
                    return "Alaska";
                case "CA":
                    return "California";
                case "CO":
                    return "Colorado";
                case "FL":
                    return "Florida";
                case "MI":
                    return "Michigan";
                default:
                    return "Not Know";
            }
        }










    }
}
