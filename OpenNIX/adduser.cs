using Cosmos.System.FileSystem.VFS;
using System;

public class adduser
{
	public void useradd(string[] args)
	{
		if (args[1].Contains(" "))
        {
			Console.WriteLine("Username cannot contain spaces!");
        }
		else
        {
			Console.WriteLine("Creating new user named " + args[1]);
			VFSManager.CreateDirectory("0:\\" + "Users\\" + args[1] +"\\Documents");
			VFSManager.CreateDirectory("0:\\" + "Users\\" + args[1] + "\\Pictures");
			VFSManager.CreateDirectory("0:\\" + "Users\\" + args[1] + "\\Sounds");
		}
	}
}
