using System.Activities.Tracking;
using System;
using System.IO;
namespace FileTrackingParticipant {
    public class FileTrackingParticipant:TrackingParticipant {
        string fileName;
        protected override void Track(TrackingRecord record, 
                                      TimeSpan timeout) {
            fileName = @"D:\" + record.InstanceId + ".tracking";
            using (StreamWriter sw = File.AppendText(fileName)) {
                sw.WriteLine("----------Tracking Started-----------");
                sw.WriteLine(record.ToString());
                sw.WriteLine("----------Tracking End---------------");
            }  
        }
    }
}