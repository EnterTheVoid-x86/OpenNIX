using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using System.IO;
using Cosmos.System.Graphics;
namespace UniDOS
{
	public class Kernel : Sys.Kernel
	{
		CosmosVFS fs = new CosmosVFS();
		protected override void BeforeRun()
		{
			Console.WriteLine("Changing resolution...");
			Console.SetWindowSize(90, 30);
			Console.WriteLine("UniDOS starting...");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Initalizing Filesystem...");
			VFSManager.RegisterVFS(fs);
			var available_space = fs.GetAvailableFreeSpace(@"0:\");
			Console.WriteLine("Available Free Space: " + available_space);
			var fs_type = fs.GetFileSystemType(@"0:\");
			Console.WriteLine("File System Type: " + fs_type);
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Welcome to UniDOS (Unix Nano Interface & Disk Operating System).");
			Console.WriteLine("Type 'help' for help on commands.");
		}

		protected override void Run()
		{
			string input = "";
			Console.CursorVisible = true;
			Console.Write("UniDOS#> ");
			input = Console.ReadLine();
            HandleThisCommand(input);
		}

		private static void HandleThisCommand(string input)
        {
			if (input == "help")
			{
				Console.WriteLine("UniDOS help:");
				Console.WriteLine("help -- shows this list of commands");
				Console.WriteLine("sysinfo -- shows system information");
				Console.WriteLine("build -- shows information about this build of UniDOS");
				Console.WriteLine("touch -- create a file");
				Console.WriteLine("read -- read a file's contents");
				Console.WriteLine("contents -- list the contents of the current directory");
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
				var directory_list = VFSManager.GetDirectoryListing("0:\\");
				foreach (var directoryEntry in directory_list)
				{
					Console.WriteLine(directoryEntry.mName);
				}
			}
			else if (input == "chsize")
			{
				Console.WriteLine("Not implemented yet.");
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
				Console.WriteLine("UniDOS error: Unknown Command " + input);
			}
        }
	}
}
