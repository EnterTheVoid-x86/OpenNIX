using System;
using tutelOS;
using Cosmos;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using System.IO;
using Cosmos.System.Network.IPv4.UDP.DHCP;

public class Init
{
	CosmosVFS fs = new CosmosVFS();
	string username = "user";
	string openver = "1.4";
	string hostname = "tutelOS";
	public void InitSystem()
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
}
