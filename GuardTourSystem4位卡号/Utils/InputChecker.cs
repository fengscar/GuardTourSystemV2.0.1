using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GuardTourSystem.Utils
{
    class InputChecker
    {
        //  将 card 转换成 合法的 大写16进制
        public static void CheckRfidCard(ref string card, int length = 4)
        {
            if (string.IsNullOrWhiteSpace(card))
            {
                return;
            }
            string str = (card.Clone() as string).ToUpper();

            card = "";
            string pattern = @"(^[0-9a-fA-F]$)";
            foreach (var item in str)
            {
                if (Regex.IsMatch(item.ToString(), pattern))
                {
                    card += item;
                    if (card.Length == length)
                    {
                        return;
                    }
                }
            }
        }

        public static void CheckEmployeeNumber(ref string card, int length = 6)
        {
            if (string.IsNullOrWhiteSpace(card))
            {
                return;
            }
            card = card.Trim();
            string str = card.Clone() as string;

            card = "";
            string pattern = @"(^[0-9a-zA-Z]$)";
            foreach (var item in str)
            {
                if (Regex.IsMatch(item.ToString(), pattern))
                {
                    card += item;
                    if (card.Length == length)
                    {
                        return;
                    }
                }
            }
        }

    }
}
