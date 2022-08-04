using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using System.IO;
using Cosmos.System.Graphics;
using System.Drawing;
using Point = Cosmos.System.Graphics.Point;
using Cosmos.System.Network;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;
using Cosmos.System.Network.IPv4.TCP.FTP;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.HAL;
using Cosmos.System.ExtendedASCII;

namespace NeuroOS
{
	public class Kernel : Sys.Kernel
	{
		CosmosVFS fs = new CosmosVFS();
		protected override void BeforeRun()
		{
			var neurover = "1.0";
			Console.WriteLine("Changing resolution...");
			Console.SetWindowSize(90, 30);
			Console.WriteLine("NeuroOS starting...");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Initalizing Filesystem...");
			try
			{
				VFSManager.RegisterVFS(fs);
				var available_space = fs.GetAvailableFreeSpace(@"0:\");
				Console.WriteLine("Available Free Space: " + available_space);
				var fs_type = fs.GetFileSystemType(@"0:\");
				Console.WriteLine("File System Type: " + fs_type);
			}
			catch (Exception e)
            {
				Console.WriteLine("Filesystem couldn't be initalized.");
				Console.WriteLine("Continuing without Filesystem...");
            }
			Console.WriteLine("Initalizing Network...");
			try
			{
				using (var xClient = new DHCPClient())
				{
					/** Send a DHCP Discover packet **/
					//This will automatically set the IP config after DHCP response
					xClient.SendDiscoverPacket();
				}
				DHCPClient netctl = new DHCPClient();
			}
			catch (Exception)
            {
				Console.WriteLine("NeuroOS error: Couldn't initalize network!");
				Console.WriteLine("Continuing without networking...");
            }
			Console.WriteLine("Initalizing PC Speaker...");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("                    .(/    ,//                    ");
			Console.WriteLine("               ,(*              ,(*               ");
			Console.WriteLine("          ,(*                        ./*          ");
			Console.WriteLine("     .(*                                  .//     ");
			Console.WriteLine(" #,                     ./                      /#");
			Console.WriteLine("#                  ,(#########/                   ");
			Console.WriteLine("#             ,(###################/              ");
			Console.WriteLine("#            #########################            ");
			Console.WriteLine("#            #########################            ");
			Console.WriteLine("#            #########################            ");
			Console.WriteLine("#            #########################            ");
			Console.WriteLine("#            #########################            ");
			Console.WriteLine("#             ,(###################/              ");
			Console.WriteLine("#                  ,(#########/                   ");
			Console.WriteLine(" #,                     ./                      /#");
			Console.WriteLine("     .(*                                  .//     ");
			Console.WriteLine("          ,(*                        ./*          ");
			Console.WriteLine("               ,(*              ,(*               ");
			Console.WriteLine("                    .(/    ,//                    ");
			Sys.PCSpeaker.Beep(Sys.Notes.AS3, Sys.Durations.Sixteenth);
			Sys.PCSpeaker.Beep(Sys.Notes.D4, Sys.Durations.Sixteenth);
			Sys.PCSpeaker.Beep(Sys.Notes.F4, Sys.Durations.Sixteenth);
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Welcome to NeuroOS version " + neurover + ".");
			Console.WriteLine("Type 'help' for help on commands.");
		}

		protected override void Run()
		{
			string input = "";
			Console.CursorVisible = true;
			Console.Write("NeuroOS#> ");
			input = Console.ReadLine();
            HandleThisCommand(input);
		}

		private static void HandleThisCommand(string input)
        {
			if (input == "help")
			{
				Console.WriteLine("NOTE: The * symbol means the command has not been implemented yet.");
				Console.WriteLine("NeuroOS help:");
				Console.WriteLine("help -- shows this list of commands");
				Console.WriteLine("sysinfo -- shows system information");
				Console.WriteLine("build -- shows information about this build of NeuroOS");
				Console.WriteLine("touch -- create a file");
				Console.WriteLine("read -- read a file's contents");
				Console.WriteLine("contents -- list the contents of the current directory");
				Console.WriteLine("curl* -- use the internet to get information");
				Console.WriteLine("chsize -- change screen resolution");
				Console.WriteLine("beep -- makes a pc speaker beep");
				Console.WriteLine("play -- play a song that totally isn't from any major game company");
				Console.WriteLine("clear -- removes all text from the screen");
				Console.WriteLine("shutdown -- powers off your machine");
				Console.WriteLine("reboot -- reboots your machine");
			}
			else if (input == "sysinfo")
			{
				Console.WriteLine("Amount of used RAM:");
				Console.WriteLine(Cosmos.Core.GCImplementation.GetUsedRAM());
				Console.WriteLine("Amount of available RAM:");
				Console.WriteLine(Cosmos.Core.GCImplementation.GetAvailableRAM());
			}
			else if (input == "touch")
			{
				string touchname = "";
				Console.WriteLine("File name: ");
				touchname = Console.ReadLine();
				try
				{
					var file_stream = File.Create(@"0:\" + touchname);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
			}
			else if (input == "read")
			{
				string readfile = "";
				Console.WriteLine("File name: ");
				readfile = Console.ReadLine();
				try
				{
					Console.WriteLine(File.ReadAllText(@"0:\" + readfile));
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
			}
			else if (input == "contents")
			{
				try
				{
					Console.WriteLine("PhysicalDrive0:");
					var directory_list = VFSManager.GetDirectoryListing("0:\\");
					foreach (var directoryEntry in directory_list)
					{
						Console.WriteLine(directoryEntry.mName);
					}
				}
				catch (Exception)
				{
					Console.Beep();
					Console.WriteLine("NeuroOS error: Couldn't list the directory!");
				}
			}
			else if (input == "beep")
			{
				Console.Beep();
			}
			else if (input == "chsize")
			{
				Console.WriteLine("Enter new resolution:");
				Console.WriteLine("1. 40x25");
				Console.WriteLine("2. 40x50");
				Console.WriteLine("3. 80x25");
				Console.WriteLine("4. 80x50");
				Console.WriteLine("5. 90x30 (Default Resolution)");
				Console.WriteLine("6. 90x60");
				string chsizeinput = "";
				chsizeinput = Console.ReadLine();
				if (chsizeinput == "1")
				{
					Console.SetWindowSize(40, 25);

				}
				else if (chsizeinput == "2")
				{
					Console.SetWindowSize(40, 50);
				}
				else if (chsizeinput == "3")
				{
					Console.SetWindowSize(80, 25);
				}
				else if (chsizeinput == "4")
				{
					Console.SetWindowSize(80, 50);
				}
				else if (chsizeinput == "5")
				{
					Console.SetWindowSize(90, 30);
				}
				else if (chsizeinput == "6")
				{
					Console.SetWindowSize(90, 60);
				}
				else
				{
					Console.WriteLine("Invalid input.");
				}
			}
			else if (input == "play")
			{
				Console.WriteLine("Song name: At Heck's Gate");
				Console.WriteLine("Loading...");
				System.Threading.Thread.Sleep(5000);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.D5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.C5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.AS4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.B4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.C5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.D5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.C5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.AS4, Sys.Durations.Half);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.D5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.C5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.AS4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.B4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.C5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.D5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.C5, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.E4, Sys.Durations.Sixteenth);
				Sys.PCSpeaker.Beep(Sys.Notes.AS4, Sys.Durations.Half);
			}
			else if (input == "shutdown")
			{
				Console.WriteLine("Shutting down in 5 seconds!");
				System.Threading.Thread.Sleep(1000);
				Console.WriteLine("Shutting down in 4 seconds!");
				System.Threading.Thread.Sleep(1000);
				Console.WriteLine("Shutting down in 3 seconds!");
				System.Threading.Thread.Sleep(1000);
				Console.WriteLine("Shutting down in 2 seconds!");
				System.Threading.Thread.Sleep(1000);
				Console.WriteLine("Shutting down in 1 second!");
				System.Threading.Thread.Sleep(1000);
				Sys.Power.Shutdown();
			}
			else if (input == "reboot")
			{
				Console.WriteLine("Rebooting in 5 seconds!");
				System.Threading.Thread.Sleep(1000);
				Console.WriteLine("Rebooting in 4 seconds!");
				System.Threading.Thread.Sleep(1000);
				Console.WriteLine("Rebooting in 3 seconds!");
				System.Threading.Thread.Sleep(1000);
				Console.WriteLine("Rebooting in 2 seconds!");
				System.Threading.Thread.Sleep(1000);
				Console.WriteLine("Rebooting in 1 second!");
				System.Threading.Thread.Sleep(1000);
				Sys.Power.Reboot();
			}
			else if (input == "clear")
			{
				Console.Clear();
			}
			else
			{
				Console.WriteLine("NeuroOS error: Unknown Command " + input);
			}
        }
	}
}
