using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyClipBoardList.Util {
    internal static class AppUtil {

        /// <summary>
        /// set clipboard
        /// </summary>
        /// <param name="baseText"></param>
        public static void SetClipBoard(string baseText) {
            var text = String.Format(baseText, Clipboard.GetText(TextDataFormat.UnicodeText));
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
        }
    }
}
