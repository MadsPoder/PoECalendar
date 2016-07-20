//The script has no error handling, so it shouldn't really be used as is for other purposes. 
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace PoECalendar
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter fs = new System.IO.StreamWriter("PoEMedallionRaceSeason.ics");
            fs.WriteLine("BEGIN:VCALENDAR\nPRODID:-//PoECalender//www.pathofexile.com/seasons/index/season/Medallion//EN\nVERSION:2.0\nCALSCALE:GREGORIAN\nMETHOD:PUBLISH");
            string jsonData;
            WebRequest rawJsonData = WebRequest.Create("http://api.pathofexile.com/leagues?type=event&compact=1");
            WebResponse response = rawJsonData.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                jsonData = reader.ReadToEnd();
            }
            JArray jsonVal = JArray.Parse(jsonData) as JArray;

            foreach (dynamic race in jsonVal)
            {

                DateTime dtStart = Convert.ToDateTime(race.startAt);
                
                string newStartDate = dtStart.ToString("yyyyMMdd");
                string newStartTime = dtStart.ToString("HHmmss");
                string newStart = newStartDate + "T" + newStartTime +"Z";

                DateTime dtEnd = Convert.ToDateTime(race.endAt);
                string newEndDate = dtEnd.ToString("yyyyMMdd");
                string newEndTime = dtEnd.ToString("HHmmss");
                string newEnd = newEndDate + "T" + newEndTime + "Z";

                string uid = race.url;
                if((uid != null))
                    uid = uid.Substring(uid.LastIndexOf("/") + 1, (uid.Length - uid.LastIndexOf("/")) - 1);

                fs.WriteLine("\nBEGIN:VEVENT\nDTSTART:" + newStart + "\nDTEND:" + newEnd + "\nLOCATION:Wraeclast\nSUMMARY:" + race.id + "\nDESCRIPTION:" + race.url + "\nUID:" + uid + newStart + newEnd + "\nTRANSP:OPAQUE\nACTION:DISPLAY\nPRIORITY:0\nCLASS:PRIVATE\nEND:VEVENT");
            }
            fs.Write("\nEND:VCALENDAR");
            fs.Close();
        }
    }
}