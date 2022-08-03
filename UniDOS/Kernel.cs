using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
namespace UniDOS
{
	public class Kernel : Sys.Kernel
	{

		protected override void BeforeRun()
		{
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
			else if (input == "build")
            {
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
			else
            {
				Console.WriteLine("UniDOS error: Unknown Command!");
            }
        }
	}
}
