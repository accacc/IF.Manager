using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen
{
    public class ObjectNamerHelper
    {
        public static string RemoveLastWord(string name, string word)
        {

            name = name.Trim();

            string lastFiveChar = name.Substring(name.Length - word.Length);

            if (lastFiveChar == word)
            {
                return name.Remove(name.Length - word.Length);
            }

            return name;

        }


        public static string AddAsLastWord(string name, string word)
        {
            name = name.Trim();

            if (name.Length < word.Length + 1)
            {
                name = name + word;
            }

            string lastFiveChar = name.Substring(name.Length - word.Length);

            if (lastFiveChar != word)
            {
                name = name + word;
            }

            return name;
        }
    }
}
