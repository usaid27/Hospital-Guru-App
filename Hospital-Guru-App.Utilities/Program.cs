﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("ts7964454@gmail.com", "tqyo tfou jvle bagc"),
                EnableSsl = true
            };
            client.UseDefaultCredentials = false;
            client.Send("ts7964454@gmail.com", "cs.credent@gmail.com", "test", "testbody");
            Console.WriteLine("Sent");
            Console.ReadLine();
        }
    }
}