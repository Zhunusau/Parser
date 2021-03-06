﻿using System;
using System.Collections.Generic;
using System.Linq;
using KufarParser.Kufar;

namespace KufarParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var parser = new ParserWorker<List<Lot>>(new Kufar.KufarParser());

                parser.OnCompleted += Parser_OnCompleted;
                parser.OnNewData += Parser_OnNewData;

                Console.WriteLine("Enter the filter");
                string filter = Console.ReadLine();

                parser.Settings = new KufarSettings(filter);
                parser.Start();

                //if(Console.ReadKey().Equals("Q")) parser.Abort();

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void Parser_OnCompleted(object obj)
        {
            Console.WriteLine("\n\n\n<<<========== All work is done! ==========>>>");
        }

        private static void Parser_OnNewData(int page, List<Lot> lots)
        {
            Console.WriteLine($"\n\n\n<<<========== Page number {page} ==========>>>\n");
            foreach (var lot in lots)
            {
                Console.WriteLine($"> {lot.Name} {lot.Location} {lot.DateOfUpdate} {lot.Price}\n" +
                                  $"  {lot.Link}\n  {lot.Image}\n");
            }
        }
    }
}