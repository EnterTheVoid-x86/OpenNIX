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
using ONVI;

namespace OpenNIX
{
	public class Kernel : Sys.Kernel
	{
		CosmosVFS fs = new CosmosVFS();
		string username = "user";
		string openver = "1.3";
		string hostname = "OpenNIX";

		protected override void BeforeRun()
		{
			VFSManager.RegisterVFS(fs);
			try
			{
				Console.WriteLine("Cosmos bootloader init complete... Loading the kernel.");
				Console.WriteLine("[0.0001] Booting OpenNIX on physical CPU 0x0...");
				Console.WriteLine("[0.0001] Device hardware:");
				var RAMinMB = Cosmos.Core.CPU.GetAmountOfRAM();
				Console.WriteLine(Cosmos.Core.CPU.GetCPUBrandString());
				Console.WriteLine(RAMinMB + "MB of ram");
				Console.WriteLine("[0.0001] run init as init process");
				Console.WriteLine("[INIT] Started init.");
				Console.WriteLine("[INIT] Changing resolution...");
				Console.SetWindowSize(90, 30);
				Console.WriteLine("Cosmos bootloader init complete... Loading the kernel.");
				Console.WriteLine("[0.0001] Device hardware:");
				Console.WriteLine(Cosmos.Core.CPU.GetCPUBrandString());
				Console.WriteLine(RAMinMB + "MB of ram");
				Console.WriteLine("[0.0001] run init as init process");
				Console.WriteLine("[INIT] Started init.");
				Console.WriteLine("[INIT] Changing resolution...");
				Console.WriteLine("[INIT] Initalizing Filesystem...");
				try
				{
					var available_space = fs.GetAvailableFreeSpace(@"0:\");
					Console.WriteLine("[FILESYSTEM] Available Free Space: " + available_space);
					var fs_type = fs.GetFileSystemType(@"0:\");
					Console.WriteLine("[FILESYSTEM] File System Type: " + fs_type);
				}
				catch (Exception)
				{
					Console.BackgroundColor = ConsoleColor.DarkRed;
					Console.ForegroundColor = ConsoleColor.White;
					Console.Clear();
					Console.WriteLine("OpenNIX has encountered a Kernel Panic!");
					Console.WriteLine("");
					Console.WriteLine("-[Unable to initalize the File System!]-");
					Console.WriteLine("");
					Console.WriteLine("The system has been halted. Press any key to reboot.");
					Console.ReadKey();
					Sys.Power.Reboot();
				}
				Console.WriteLine("[INIT] Setting network IP address...");
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
					Console.WriteLine("[NETWORK ERROR] Couldn't initalize network!");
					Console.WriteLine("[NETWORK WARN] Continuing without networking...");
				}
				Console.WriteLine("[INIT] Initalizing PC Speaker...");
				Console.WriteLine("[INIT] Init complete. Dropping to login manager...");
				if (Directory.Exists("0:\\Users\\"))
				{
					Console.WriteLine("[INIT] Directory 'Users' exists.");
					if (File.Exists("0:\\System\\hostname"))
                    {
						Console.WriteLine("[INIT] Setting hostname...");
						Console.WriteLine("[INIT] Reading hostname from address 0:\\System\\hostname");
						hostname = File.ReadAllText("0:\\System\\hostname");
					}
					else
                    {
						Console.WriteLine("[INIT] Creating directory system if it does not exist");
						VFSManager.CreateDirectory("0:\\System\\");
						Console.WriteLine("[INIT] Creating file 0:\\System\\Hostname with string 'OpenNIX'");
						File.Create("0:\\System\\hostname");
						File.WriteAllText("0:\\System\\hostname", "OpenNIX");
						Console.WriteLine("[INIT] Setting hostname...");
						hostname = File.ReadAllText("0:\\System\\hostname");
					}
				}
                else
                {
					FirstTime();
				}
				Console.Beep();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("                             (          ");
				Console.WriteLine("                           &&%          ");
				Console.WriteLine("                        /&%%%&//        ");
				Console.WriteLine("                      *,,,*%%&%#        ");
				Console.WriteLine("                  .,///(&%%((#((        ");
				Console.WriteLine("                 &&%&&%#*,,*,,&&%#      ");
				Console.WriteLine("              ((((((///////&%%(((/      ");
				Console.WriteLine("            ,,,,*%%&%%&&%&&,,*,,,,      ");
				Console.WriteLine("         ,*///%&&(######//////%%&%(     ");
				Console.WriteLine("       &&%&&%#*,,*,,&&%%&%%&&%*,,,,     ");
				Console.WriteLine("    #####(/***/*/&%%#######///*/%%&#(   ");
				Console.WriteLine("  ,,,,,%%&%%&&%&&,,,,,,*%&&%%&%%*,,**   ");
				Console.WriteLine("     &&#######******%%&%##%#(******&&#/ ");
				Console.WriteLine("            %%&%%&&%,,,,,%%&&%&&%%&,,,, ");
				Console.WriteLine("                   ....*&##%##%%******* ");
				Console.WriteLine("                              %%&&%&&%(*");
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine("Callux OpenNIX 1.3-build-9-17-22 x86 (development)");
				for (; ; )
				{
					Console.Write("login: ");
					string login = "";
					login = Console.ReadLine();
					if (VFSManager.DirectoryExists("0:\\" + "Users\\" + login))
					{
						Console.Clear();
						Console.WriteLine("");
						Console.WriteLine("Welcome to Callux OpenNIX version " + openver + ".");
						Console.WriteLine("Type 'help' for help on commands.");
						username = login;
						break;
					}
					else
					{
						Console.WriteLine("login incorrect");
					}
				}
			}
			catch (Exception EX)
            {
				Console.BackgroundColor = ConsoleColor.DarkRed;
				Console.ForegroundColor = ConsoleColor.White;
				Console.Clear();
				Console.WriteLine("OpenNIX has encountered a Kernel Panic!");
				Console.WriteLine("");
				Console.WriteLine($"-[{EX}]-");
				Console.WriteLine("");
				Console.WriteLine("The system has been halted. Press any key to reboot.");
				Console.ReadKey();
				Sys.Power.Reboot();
				
			}
		}

		void DrawBar()
		{
			// This code is borrowed from Poscalin OS, we have them to thank for this top bar.
			// TITLE BAR
			// Storing Original Cursor Position
			int origX = Console.CursorLeft;
			int origY = Console.CursorTop;

			// Setting Cursor Position Top Right Of Screen
			Console.SetCursorPosition(0, 0);

			// DRAWING THE TITLE BAR
			// Coloring
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Red;

			// Time
			var hour = Cosmos.HAL.RTC.Hour;
			var minute = Cosmos.HAL.RTC.Minute;
			var strhour = hour.ToString();
			var strmin = minute.ToString();

			// Initial Bar
			Console.WriteLine("OPENNIX 1.3    KERNEL 1.3-BUILD-9-17-22    OPENNIX SHELL v1.3                        " + strhour + ":" + strmin);

			// Resetting Colors
			Console.BackgroundColor = ConsoleColor.Black;

			// Resetting Cursor Position
			Console.SetCursorPosition(origX, origY);
			Console.ForegroundColor = ConsoleColor.White;
		}

		void FirstTime()
        {
				VFSManager.CreateDirectory("0:\\System\\");
				File.Create("0:\\System\\hostname");
				File.WriteAllText("0:\\System\\hostname", "OpenNIX");
				Console.WriteLine($"Thank you for choosing to beta test OpenNIX {openver}, and Welcome!");
				Console.WriteLine("We see that it is your first time using OpenNIX.");
				Console.WriteLine("And thus, the setup will walk you through creating a user account.");
				Console.WriteLine("So, let's get started!");
				Console.WriteLine("Press any key to continue.");
				Console.ReadKey();
				Console.Write("What's your name? => ");
				string input = "";
				input = Console.ReadLine();
				Console.WriteLine("Hi, " + input + "!");
				Console.Write("What do you want your username to be? (no spaces) => ");
				string username = "";
				username = Console.ReadLine();
				Console.WriteLine("Okay. We're creating you an account right now with the username " + username + ".");
				VFSManager.CreateDirectory("0:\\" + "Users\\" + username + "\\Documents");
				VFSManager.CreateDirectory("0:\\" + "Users\\" + username + "\\Pictures");
				VFSManager.CreateDirectory("0:\\" + "Users\\" + username + "\\Sounds");
				Console.WriteLine($"Okay {input}, let's get started!");
				Console.WriteLine("You will be sent to the login manager now. Type in your username to log into your computer.");
        }
		protected override void Run()
		{
			DrawBar();
			Directory.SetCurrentDirectory($"Users\\{username}");
			string path = Directory.GetCurrentDirectory();
			if (path == username)
            {
				path = $"Users\\{username}";
            }
			Cosmos.Core.Memory.Heap.Collect();
			Console.CursorVisible = true;
			Console.Write($"{username}@{hostname}#[{path}]> ");
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
				Console.WriteLine("OpenNIX help:");
				Console.WriteLine("help -- shows this list of commands");
				Console.WriteLine("cellfetch -- shows system information");
				Console.WriteLine("build -- shows information about this build of NeuroOS");
				Console.WriteLine("echo - print what the user says");
				Console.WriteLine("touch -- create a file");
				Console.WriteLine("cat -- read a file's contents");
				Console.WriteLine("ls -- list the contents of a directory");
				Console.WriteLine("diskutil -- utils for managing hard drives");
				Console.WriteLine("mkdir -- make a directory");
				Console.WriteLine("rm -- delete a file or directory");
				Console.WriteLine("pwd -- print the directory you are in");
				Console.WriteLine("cd -- change directory");
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
				string usedRAM = (Cosmos.Core.GCImplementation.GetUsedRAM() / 1024 / 1024).ToString();
				var uptimeSpan = Cosmos.Core.CPU.GetCPUUptime();
				var RAMinMB = Cosmos.Core.CPU.GetAmountOfRAM();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("                             (          OS: OpenNIX 1.3 x86");
				Console.WriteLine("                           &&%          Kernel: 1.3-build9-17-22");
				Console.WriteLine("                        /&%%%&//        Uptime: " + uptimeSpan);
				Console.WriteLine("                      *,,,*%%&%#        Packages: 20 (cosmos)");
				Console.WriteLine("                  .,///(&%%((#((        Shell: onsh");
				Console.WriteLine("                 &&%&&%#*,,*,,&&%#      Resolution: " + Console.WindowWidth + "x" + Console.WindowHeight);
				Console.WriteLine("              ((((((///////&%%(((/      Terminal: console");
				Console.WriteLine("            ,,,,*%%&%%&&%&&,,*,,,,      CPU: " + Cosmos.Core.CPU.GetCPUBrandString());
				Console.WriteLine("         ,*///%&&(######//////%%&%(     Memory: " + usedRAM + "/" + RAMinMB + "MB");
				Console.WriteLine("       &&%&&%#*,,*,,&&%%&%%&&%*,,,,     ");
				Console.WriteLine("    #####(/***/*/&%%#######///*/%%&#(   ");
				Console.WriteLine("  ,,,,,%%&%%&&%&&,,,,,,*%&&%%&%%*,,**   ");
				Console.WriteLine("     &&#######******%%&%##%#(******&&#/ Created by EnterTheVoid-x86.");
				Console.WriteLine("            %%&%%&&%,,,,,%%&&%&&%%&,,,, Contributors:");
				Console.WriteLine("                   ....*&##%##%%******* Nex389");
				Console.WriteLine("                              %%&&%&&%(*");
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
			else if (args[0] == "cat")
			{
				try
				{
					Console.WriteLine(File.ReadAllText(args[1]));
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
			}
			else if (args[0] == "beep")
			{
				Console.Beep();
			}
			else if (args[0] == "onvi")
			{
				try
				{
					ONVI.ONVI.StartONVI(args);
				}
				catch (IndexOutOfRangeException)
                {
					Console.WriteLine("No file name given!");
				}
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
					Console.WriteLine("[0.0005 COMMAND ERROR] Invalid input.");
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
				Console.Write("Are you sure you want to shutdown? (y/n) => ");
				string input = "";
				input = Console.ReadLine();
				if (input == "y")
				{
					Sys.Power.Shutdown();
				}
				else
                {
					Console.WriteLine("Aborting shutdown.");
                }
			}
			else if (args[0] == "reboot")
			{
				Console.Write("Are you sure you want to reboot? (y/n) => ");
				string input = "";
				input = Console.ReadLine();
				if (input == "y")
				{
					Sys.Power.Reboot();
				}
				else
				{
					Console.WriteLine("Aborting reboot.");
				}
			}
			else if (args[0] == "clear")
			{
				Console.Clear();
			}
			else if (args[0] == "ls")
			{
				var command = new LSCommand();
				command.LS(args);
			}
			else if (args[0] == "adduser")
			{
				var command = new adduser();
				command.useradd(args);
			}
			else if (args[0] == "echo")
			{
				try
				{
					Console.WriteLine(args[1]);
				}
				catch (IndexOutOfRangeException)
				{
					Console.WriteLine("No input given.");
				}
				catch (Exception)
				{
					Console.WriteLine("General Error.");
				}
			}
			else if (args[0] == "throwexception")
			{
				Console.BackgroundColor = ConsoleColor.DarkRed;
				Console.ForegroundColor = ConsoleColor.White;
				Console.Clear();
				Console.WriteLine("OpenNIX has encountered a Kernel Panic!");
				Console.WriteLine("");
				Console.WriteLine($"-[User initated crash.]-");
				Console.WriteLine("");
				Console.WriteLine("The system has been halted. Press any key to reboot.");
				Console.ReadKey();
				Sys.Power.Reboot();
			}
			else if (args[0] == "calculator")
			{
				Console.WriteLine("[0.0005 INFO] Command not implemented yet.");
			}
			else if (args[0] == "diskutil")
			{
				Console.WriteLine("diskutil v0.1 alpha");
				try
				{
					if (args[1] == "list")
					{
						foreach (var disk in VFSManager.GetDisks())
						{
							Console.WriteLine(disk.ToString());
						}
					}
					else if (args[1] == "format")
					{
						Console.WriteLine("formatting disk " + args[2]);
					}
				}
				catch (Exception)
				{
					Console.WriteLine("[0.0005 COMMAND ERROR] General Error.");

				}
			}
			else if (args[0] == "build")
			{
				if (Environment.Is64BitProcess)
				{
					Console.WriteLine("This build of OpenNIX is running on a 64-bit CPU");
				}
				else
				{
					Console.WriteLine("This build of OpenNIX is running on a 32-bit CPU");
				}
			}
			else if (args[0] == "mkdir")
			{
				try
				{
					Console.WriteLine("Creating folder named " + args[1]);
					VFSManager.CreateDirectory("0:\\" + Directory.GetCurrentDirectory() + args[1]);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
			else if (args[0] == "rm")
			{
				try
				{
					if (VFSManager.FileExists("0:\\" + Directory.GetCurrentDirectory() + args[1]))
					{
						Console.WriteLine("Deleting file named " + args[1]);
						VFSManager.DeleteFile("0:\\" + Directory.GetCurrentDirectory() + args[1]);
					}
					else if (VFSManager.DirectoryExists("0:\\" + Directory.GetCurrentDirectory() + args[1]))
					{
						Console.WriteLine("Deleting folder named " + args[1]);
						VFSManager.DeleteDirectory("0:\\" + Directory.GetCurrentDirectory() + args[1], true);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
			else if (args[0] == "cd")
			{
				try
				{
					if (args[1] == "0:\\")
					{
						Directory.SetCurrentDirectory("");
					}
					else
                    {
						Directory.SetCurrentDirectory(args[1]);
					}
				}
				catch (Exception)
				{
					Console.WriteLine("[0.0005 COMMAND ERROR] General Error.");
				}
			}
			else
			{
				Console.WriteLine("onsh: " + args[0] + ": command not found");
			}
		}
	}
}
