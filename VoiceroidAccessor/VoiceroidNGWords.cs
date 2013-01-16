using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.azworks.VoiceroidAccess.messages
{
    class NGWords
    {
        public static string CreateSafdWord(string testWord)
        {
            string ngFound = FindNGWord(testWord);
            if (!string.IsNullOrEmpty(ngFound))
            {
                return CreateSafdWord(testWord.Replace(ngFound, " ピー "));
            }
            return testWord;
        }

        public static string FindNGWord(string testString)
        {
            foreach (string key in CreateNGWordList())
            {
                if (testString.Contains(key))
                    return key;
            }

            return null;
        }

        private static string[] CreateNGWordList()
        {
            return new string[]
	        {
		        "NG words",
				"message"
	        };
        }
    }
}
