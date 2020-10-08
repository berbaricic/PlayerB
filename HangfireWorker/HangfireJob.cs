using System;
using System.Collections.Generic;
using System.Text;

namespace HangfireWorker 
{
    public class HangfireJob
    {
        public void SendEMail(string name)
        {
            Console.WriteLine("Sending e-mail with name of Youtube video. Name: " + name);
        }

        public void SendNotice(string name)
        {
            Console.WriteLine("Hey you. Your video {0} has been watched. Be proud!", name);
        }

        public void DoSomeWorkAfterSendingEMail()
        {
            Console.WriteLine("Some work after sending email to client.");
        }
    }
}
