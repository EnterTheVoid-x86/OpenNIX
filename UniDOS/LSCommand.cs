using Cosmos.System.FileSystem.VFS;
using NeuroOS;
using System;
using System.IO;

public class LSCommand
{
    public LSCommand()
    {
    }

    public void LS(string[] args)
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
					var directory_list = VFSManager.GetDirectoryListing("0:\\" + Directory.GetCurrentDirectory());
					foreach (var directoryEntry in directory_list)
					{
						Console.WriteLine(directoryEntry.mName);
					}
				}
			}
			catch (Exception)
			{
				Console.Beep();
				Console.WriteLine("[0.0005 COMMAND ERROR] Failed to list directories. Maybe the filesystem wasn't initalized at boot?");
			}
	}
}
