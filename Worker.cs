using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Messaging;
using System.Media;

namespace BSQueue
{
    public class Worker
    {
        private string Folder_Source = "C:\\Users\\Greg\\Documents\\Charles River Implementations\\Taquanta\\Development\\Testing\\Source\\";
        private string Folder_Work = "C:\\Users\\Greg\\Documents\\Charles River Implementations\\Taquanta\\Development\\Testing\\Work\\";
        private string Folder_Old = "C:\\Users\\Greg\\Documents\\Charles River Implementations\\Taquanta\\Development\\Testing\\Old\\";
        //private string msg_QueueIN = "FormatName:DIRECT=OS:10.0.98.54\\private$\\blue_stream_in";
        private string msg_QueueIN = "FormatName:Direct=OS:paulglaptop\\private$\\Request"; // "FormatName:DIRECT=OS:192.168.0.2\\private$\\Taquanta";
        private string msg_QueueOUT = "FormatName:Direct=OS:paulglaptop\\private$\\Reply"; // "FormatName:DIRECT=OS:192.168.0.2\\private$\\Taquanta";

        public void CheckForFiles()
        {
            string msgReply = "";
            try
            {
                //for testing only **********************
                File.Copy(Path.Combine(Folder_Old, "Test_800.xml"), Path.Combine(Folder_Source, "Test_800.xml"));
                File.Delete(Path.Combine(Folder_Work, "Test_800.xml"));
                //*******************************

                if (Directory.GetFiles(Folder_Source, "*.xml").Length == 0) return; // no files

                //Message queue
 
                MessageQueue myMsgQ = new MessageQueue(@msg_QueueIN, false, true);

                ((XmlMessageFormatter)myMsgQ.Formatter).TargetTypeNames = new string[] { "System.Xml" };// std  

                string myFileContent = "";

                foreach (var file in Directory.GetFiles(Folder_Source, "*.xml"))
                {
                    string FileName = Path.GetFileName(file);
                    File.Move(file, Path.Combine(Folder_Work, FileName));
                }

                string[] Myfiles = Directory.GetFiles(Folder_Work, "*.xml");

                for (int i = 0; i <= Myfiles.Length - 1; i++)
                {
//get paul to give a legit order punch in to dummy xml.
// ask him to create the 2 charles river quueues for sole use of CRIMS and let me know the names and IP of the Server and the queue

 
                    myFileContent = File.ReadAllText(Myfiles[i]);
                    // code here to create output xml message as done before.
                    Message myMsg = new Message(myFileContent, new ActiveXMessageFormatter());
                    try
                    {
                        myMsgQ.Send(myMsg);
                    }
                    catch (MessageQueueException msgEx)
                    {
                        throw msgEx;
                    }
                    try
                    {
                        //receiving the reply from fpm.  add in a time loop to keep looking for a reply, after x time generate email and fail
                        MessageQueue myMsgQOUT = new MessageQueue(@msg_QueueOUT, false, true);
                        myMsgQOUT.Formatter = new XmlMessageFormatter();
                        msgReply = ((string)myMsgQOUT.Receive().Body); //murst interogate msgReply to load the ACK and NACKS into CRIMS
                    }
                    catch (MessageQueueException msgEx)
                    {
                        throw msgEx;
                    }
                    
                    FileInfo myFileDetail = new FileInfo(Myfiles[i]);
                    File.Move(Myfiles[i], Path.Combine(Folder_Old, myFileDetail.Name.Substring(0, myFileDetail.Name.Length - 4) + DateTime.Now.ToString("yyyyMMddHHmmssms") + ".xml"));
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void ReadQueue()
        {
            try
            {
                
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

    }
}
