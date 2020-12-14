using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballManadger
{
    class EditingInfo
    {
        public static int ConvertNumber(string textToConvert)
        {
            bool check = false;
            int number = 0;
            check = int.TryParse(textToConvert, out number);
            if (check)
            {
                return number;
            }
        
            return number;
        }
        public static double ConvertNumberToDouble(string textToConvert)
        {
            bool check = false;
            double number = 0;
            check = double.TryParse(textToConvert, out number);
            if (check)
            {
                return number;
            }

            return number;
        }

        public EditingInfo()
        {
                
        }
    }
}
