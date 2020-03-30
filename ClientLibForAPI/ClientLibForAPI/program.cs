using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using DatabaseEntries;
using Newtonsoft.Json;

namespace ClientLibForAPI
{
    class program
    {
       
        private static void Main() 
        {
            Console.WriteLine("Hello world");
            Groups tester = new Groups();
            var allGroups = tester.GetGroup();
            foreach(var i in allGroups) 
            {
                Console.WriteLine($"{i.GroupID}, {i.Name}");
            }
        }

    }
}
