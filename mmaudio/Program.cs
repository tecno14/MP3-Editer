using MetroFramework.Components;
using mmaudio.Clss;
using mmaudio.Frms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mmaudio
{
    static class Program
    {
        public static MyEditer Editer;
        public static MetroStyleManager MSM;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [Obsolete]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Mainfrm());
            //Application.Run(new Frms.Frm());
        }
        public static string DictionaryToString(Dictionary<string, string> dictionary)
        {
            string dictionaryString = "";
            foreach (KeyValuePair<string, string> keyValues in dictionary)
            {
                dictionaryString += keyValues.Key + " : " + keyValues.Value + ",\n ";
            }
            return dictionaryString.TrimEnd(',', ' ') ;
        }

    }
}
