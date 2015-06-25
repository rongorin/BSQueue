using System;
using System.IO;
using System.Text;

namespace BSQueue
{
    public class Program
    {
        

        static void Main(string[] args)
        {
            //we need to check if there are any xml file to put on the queue and move them to a working folder.
            try
            {
                Worker myObj = new Worker();
                myObj.CheckForFiles();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            

            
        }
    }
}
