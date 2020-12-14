using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace BasketballManadger
{
    class EditingInfo
    {
        public static int ConvertNumber(string textToConvert)
        {
            bool check = false;
            int number = -1;
            check = int.TryParse(textToConvert, out number);
            if (check)
            {
                return number;
            }
            else
            {
                number = -1;
                return number;
            }
        }


        public static double ConvertNumberToDouble(string textToConvert)
        {
            bool check = false;
            double number = -1;
            check = double.TryParse(textToConvert, out number);
            if (check)
            {
                return number;
            }

            else
            {
                number = -1;
                return number;
            }
        }

        public EditingInfo()
        {
                
        }
    }
}
