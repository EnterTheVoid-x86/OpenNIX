using Cosmos.Core.Memory;
using Cosmos.HAL.Drivers.PCI.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys = Cosmos.System;

namespace CosmosKernel1.Graphics
{
    public class Canvas
    {
        [ManifestResourceStream(ResourceName = "CosmosKernel2.Wallpaper.bmp")]
        static byte[] wallpaper;
        static Bitmap bitmap = new Bitmap(wallpaper);
        [ManifestResourceStream(ResourceName = "CosmosKernel2.Cursor.bmp")]
        static byte[] cursor;
        static Bitmap CursorBitmap = new Bitmap(cursor);
        [ManifestResourceStream(ResourceName = "CosmosKernel2.StartupSound.wav")]
        static byte[] StartupSoundWAVE;
        static Pen pen = new Pen(Color.White);
        static uint RAMinMB = Cosmos.Core.CPU.GetAmountOfRAM();
        static string CPUModel = Cosmos.Core.CPU.GetCPUBrandString();
        public static void UpdateCursor()
        {
            var FreedMem = Heap.Collect();
            try
            {

                FreedMem = Heap.Collect();
                KernelHelpers.canvas.DrawImageAlpha(CursorBitmap, (int)Cosmos.System.MouseManager.X, (int)Cosmos.System.MouseManager.Y);
                FreedMem = Heap.Collect();
                KernelHelpers.canvas.Display();
                FreedMem = Heap.Collect();
                KernelHelpers.canvas.DrawImage(bitmap, 0, 0);
                FreedMem = Heap.Collect();
                KernelHelpers.canvas.DrawString("OpenNIX 10", PCScreenFont.Default, pen, 0, 587);
                FreedMem = Heap.Collect();
                KernelHelpers.canvas.DrawString(CPUModel, PCScreenFont.Default, pen, 0, 20);
                FreedMem = Heap.Collect();
                KernelHelpers.canvas.DrawString($"*** IN DEVELOPMENT *** FPS: {Kernel.FPS} | NUMBER OF OBJECTS FREED: {FreedMem} | {RAMinMB}MB OF RAM", PCScreenFont.Default, pen, 0, 0);
                FreedMem = Heap.Collect();
            }
            catch (Exception e)
            {
                KernelHelpers.canvas.DrawString("Exception occurred: " + e.Message, PCScreenFont.Default, pen, 0, 60);
            }
        }
        public static void DrawCursor()
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
            Console.WriteLine("                   Initalizing resources...");
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
            Console.WriteLine("                       Starting Poxi...");
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
            KernelHelpers.canvas = new SVGAIICanvas(new Mode(800, 600, ColorDepth.ColorDepth32));
            KernelHelpers.canvas.Clear(Color.Blue);


            Sys.MouseManager.ScreenWidth = 800;
            Sys.MouseManager.ScreenHeight = 600;

            Sys.MouseManager.X = (uint)((int)KernelHelpers.canvas.Mode.Columns / 2);
            Sys.MouseManager.Y = (uint)((int)KernelHelpers.canvas.Mode.Rows / 2);

        }

        public static void StartupSound()
        {
            try
            {
                var mixer = new AudioMixer();
                var audioStream = MemoryAudioStream.FromWave(StartupSoundWAVE);
                var driver = AC97.Initialize(bufferSize: 4096);
                mixer.Streams.Add(audioStream);

                var audioManager = new AudioManager()
                {
                    Stream = mixer,
                    Output = driver


                };
                audioManager.Enable();
            }
            catch (Exception)
            {
                Console.Beep();
            }
        }
    }
}
