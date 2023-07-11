using Cosmos.System.FileSystem.VFS;
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
					if (Directory.Exists(directoryEntry.mFullPath))
					{
						Console.WriteLine(directoryEntry.mName + " [Folder]");
					}
					else if (File.Exists(directoryEntry.mFullPath))
					{
						Console.WriteLine(directoryEntry.mName + " [File]");
					}
					}
				}
				catch (Exception)
				{
					var directory_list = VFSManager.GetDirectoryListing("0:\\" + Directory.GetCurrentDirectory());
					foreach (var directoryEntry in directory_list)
					{
						if (Directory.Exists(directoryEntry.mFullPath))
						{
							Console.WriteLine(directoryEntry.mName + " [Folder]");
						}
						else if (File.Exists(directoryEntry.mFullPath))
						{
						Console.WriteLine(directoryEntry.mName + " [File]");
						}
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
