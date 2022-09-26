using Cosmos.Core.Memory;
using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.ExtendedASCII;
using Cosmos.HAL.Network;
using Cosmos.HAL;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        private static int _frames = 0;
        public static int FPS = 0;
        public static int DeltaTime = 0;
        public static int FreeCount = 0;
        public string user = "user";
        public string hostname = "OpenNIX";
        public static void Update()
        {
            if (DeltaTime != RTC.Second)
            {
                FPS = _frames;
                _frames = 0;
                DeltaTime = RTC.Second;
            }

            _frames++;
        }
        protected override void BeforeRun()
        {
            
            Console.Clear();
            Console.WriteLine("              ***===+++ IN DEVELOPMENT +++===***");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("                     ------======------");
            Console.WriteLine("                         OpenNIX 10");
            Console.WriteLine("                     ------======------");
            Console.WriteLine("                   Initalizing variables...");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Copyright (C) 2022, Callux");
            long TimeOnBoot = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            Graphics.Canvas.DrawCursor();
            Graphics.Canvas.StartupSound();
        }

        protected override void Run()
        {
            Update();
            Graphics.Canvas.UpdateCursor();
        }
    }
}
