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
			var neurover = "1.2";
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
			Cosmos.Core.Memory.Heap.Collect();
			Console.CursorVisible = true;
			Console.Write("NeuroOS#> ");
			string input = "";
			input = Console.ReadLine();
			string[] args;
			args = input.Split(' ');
			HandleThisCommand(args);
		}

		private static void HandleThisCommand(string[] args)
		{
			if (args[0] == "help")
			{
				Console.WriteLine("NOTE: The * symbol means the command has not been implemented yet.");
				Console.WriteLine("NeuroOS help:");
				Console.WriteLine("help -- shows this list of commands");
				Console.WriteLine("cellfetch -- shows system information");
				Console.WriteLine("build -- shows information about this build of NeuroOS");
				Console.WriteLine("touch -- create a file");
				Console.WriteLine("read -- read a file's contents");
				Console.WriteLine("list -- list the contents of a directory");
				Console.WriteLine("mkfolder -- make a directory");
				Console.WriteLine("del -- delete a file or directory");
				Console.WriteLine("curl* -- use the internet to get information");
				Console.WriteLine("chsize -- change screen resolution");
				Console.WriteLine("beep -- makes a pc speaker beep");
				Console.WriteLine("play -- play a song that totally isn't from any major game company");
				Console.WriteLine("clear -- removes all text from the screen");
				Console.WriteLine("shutdown -- powers off your machine");
				Console.WriteLine("reboot -- reboots your machine");
				Console.WriteLine("calculator* -- do some math");
				Console.WriteLine("And for fun, throwexception -- crashes the operating system");
			}
			else if (args[0] == "cellfetch")
			{
				var usedRAM = Cosmos.Core.GCImplementation.GetUsedRAM() / 1024;
				var uptimeSpan = Cosmos.Core.CPU.GetCPUUptime();
				var RAMinMB = Cosmos.Core.CPU.GetAmountOfRAM();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("                    .(/    ,//                      OS: NeuroOS 1.2 x86");
				Console.WriteLine("               ,(*              ,(*                 Kernel: 1.2-build8-4-22");
				Console.WriteLine("          ,(*                        ./*            Uptime: " + uptimeSpan);
				Console.WriteLine("     .(*                                  .//       Packages: 20 (cosmos)");
				Console.WriteLine(" #,                     ./                      /#  Shell: nucleus");
				Console.WriteLine("#                  ,(#########/                     Resolution: " + Console.WindowWidth + "x" + Console.WindowHeight);
				Console.WriteLine("#             ,(###################/                Terminal: console");
				Console.WriteLine("#            #########################              CPU: " + Cosmos.Core.CPU.GetCPUVendorName());
				Console.WriteLine("#            #########################              Memory: " + usedRAM + "/" + RAMinMB + "MB");
				Console.WriteLine("#            #########################            ");
				Console.WriteLine("#            #########################            ");
				Console.WriteLine("#            #########################            ");
				Console.WriteLine("#             ,(###################/              ");
				Console.WriteLine("#                  ,(#########/                   ");
				Console.WriteLine(" #,                     ./                      /#");
				Console.WriteLine("     .(*                                  .//       Created by EnterTheVoid-x86.");
				Console.WriteLine("          ,(*                        ./*            Contributors:");
				Console.WriteLine("               ,(*              ,(*                 Nex389");
				Console.WriteLine("                    .(/    ,//                    ");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else if (args[0] == "touch")
			{
				try
				{
					var file_stream = File.Create("0:\\" + args[1]);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
			}
			else if (args[0] == "read")
			{
				try
				{
					Console.WriteLine(File.ReadAllText("0:\\" + args[1]));
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
			}
			else if (args[0] == "contents")
			{
				try
				{
					try
					{
						var directory_list = VFSManager.GetDirectoryListing("0:\\" + args[1]);
						foreach (var directoryEntry in directory_list)
						{
							Console.WriteLine(directoryEntry.mName);
						}
					}
					catch (Exception)
					{
						Console.WriteLine("0:\\");
						var directory_list = VFSManager.GetDirectoryListing("0:\\");
						foreach (var directoryEntry in directory_list)
						{
							Console.WriteLine(directoryEntry.mName);
						}
					}
				}
				catch (Exception)
				{
					Console.Beep();
					Console.WriteLine("NeuroOS error: Couldn't list the directory!");
				}
			}
			else if (args[0] == "beep")
			{
				Console.Beep();
			}
			else if (args[0] == "chsize")
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
			else if (args[0] == "play")
			{
				Console.WriteLine("Song name: At Heck's Gate");
				Console.WriteLine("Loading...");
				System.Threading.Thread.Sleep(5000);
				Console.WriteLine("Loaded 40 E notes, 4 D notes, 6 C notes, and 4 ASharp notes.");
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
			else if (args[0] == "shutdown")
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
			else if (args[0] == "reboot")
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
			else if (args[0] == "clear")
			{
				Console.Clear();
			}
			else if (args[0] == "throwexception")
			{
				throw new Exception("User initiated crash.");
			}
			else if (args[0] == "calculator")
			{
				Console.WriteLine("Not implemented yet.");
			}
			else if (args[0] == "build")
			{
				if (Environment.Is64BitProcess)
				{
					Console.WriteLine("This build of NeuroOS is running on a 64-bit CPU");
				}
				else
				{
					Console.WriteLine("This build of NeuroOS is running on a 32-bit CPU");
				}
			}
			else if (args[0] == "mkfolder")
			{
				try
				{
					Console.WriteLine("Creating folder named " + args[1]);
					VFSManager.CreateDirectory("0:\\" + args[1]);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
			else if (args[0] == "del")
			{
				try
				{
					if (VFSManager.FileExists("0:\\" + args[1]))
					{
						Console.WriteLine("Deleting file named " + args[1]);
						VFSManager.DeleteFile("0:\\" + args[1]);
					}
					else if (VFSManager.DirectoryExists("0:\\" + args[1]))
                    {
						Console.WriteLine("Deleting folder named " + args[1]);
						VFSManager.DeleteDirectory("0:\\" + args[1], true);
					}
				}
				catch (Exception e)
                {
					Console.WriteLine(e);
                }
			}
			else
			{
				Console.WriteLine("NeuroOS error: Unknown Command " + args[0]);
			}
		}
	}
}
