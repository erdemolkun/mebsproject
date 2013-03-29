using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mebs_Envanter
{
    public class LoggerMebs
    {
        static DateTime nowTime;
        static LoggerMebs() {
            nowTime = DateTime.Now;
        }
        public static void WriteToFile(String msg) {

            try
            {
                if (!Directory.Exists("log"))
                {
                    Directory.CreateDirectory("log");
                }
                String fileName = nowTime.ToShortDateString() + " " + nowTime.ToShortTimeString() + ".txt";
                fileName = fileName.Replace(':', '_');


                DateTime dt = DateTime.Now;
                String headerTime = dt.ToLongTimeString() + dt.ToString("':'fff") + "";
                StringBuilder sb = new StringBuilder();
                sb.Append(headerTime);
                sb.Append(System.Environment.NewLine);
                sb.Append(System.Environment.NewLine);
                sb.Append(msg); //Add tabulation                

                String content = sb.ToString(); // error + "\rHata tipi : " + hataTipi;            

                using (StreamWriter sw = File.AppendText("log\\" + fileName))
                {
                    sw.Write(content);
                }
            }
            catch (Exception)
            {
            }
        
        }
    }
}
