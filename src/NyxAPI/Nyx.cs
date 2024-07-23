using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NyxAPI;

public class Nyx
{
	public struct STARTUPINFO
	{
		public uint cb;

		public string lpReserved;

		public string lpDesktop;

		public string lpTitle;

		public uint dwX;

		public uint dwY;

		public uint dwXSize;

		public uint dwYSize;

		public uint dwXCountChars;

		public uint dwYCountChars;

		public uint dwFillAttribute;

		public uint dwFlags;

		public short wShowWindow;

		public short cbReserved2;

		public IntPtr lpReserved2;

		public IntPtr hStdInput;

		public IntPtr hStdOutput;

		public IntPtr hStdError;
	}

	public struct PROCESS_INFORMATION
	{
		public IntPtr hProcess;

		public IntPtr hThread;

		public uint dwProcessId;

		public uint dwThreadId;
	}

	public static string Exploit = "NyxAPI";

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

	private static bool IsProcessRunning(string processName)
	{
		Process[] processesByName = Process.GetProcessesByName(processName);
		return processesByName.Length != 0;
	}

	private static void CloseProcess(string processName)
	{
		Process[] processesByName = Process.GetProcessesByName(processName);
		Process[] array = processesByName;
		foreach (Process process in array)
		{
			try
			{
				process.Kill();
			}
			catch (Exception)
			{
			}
		}
	}

	private static async void AutoKillBackgroundNiggers()
	{
		string rbx = "RobloxPlayerBeta";
		string nyx = "nyxbeta";
		while (true)
		{
			if (!IsProcessRunning(rbx))
			{
				CloseProcess(nyx);
			}
			await Task.Delay(1);
		}
	}

	public void Attach()
	{
		AutoKillBackgroundNiggers();
		Directory.CreateDirectory("workspace");
		File.WriteAllText("workspace\\DONOTTOUCH.lua", "");
		if (File.Exists("nyxbeta.exe"))
		{
			if (IsRobloxOpen())
			{
				if (!IsAttached())
				{
					STARTUPINFO lpStartupInfo = default(STARTUPINFO);
					lpStartupInfo.cb = (uint)Marshal.SizeOf(lpStartupInfo);
					PROCESS_INFORMATION lpProcessInformation = default(PROCESS_INFORMATION);
					try
					{
						if (CreateProcess(null, "nyxbeta.exe", IntPtr.Zero, IntPtr.Zero, bInheritHandles: false, 0u, IntPtr.Zero, null, ref lpStartupInfo, out lpProcessInformation))
						{
							Message(2, "Attach successful");
						}
						else
						{
							Message(1, "Exception caught for attaching! " + Marshal.GetLastWin32Error());
						}
						return;
					}
					catch (Exception ex)
					{
						Message(1, "Attach(); exception -> " + ex.Message);
						return;
					}
				}
				Message(2, Exploit + " is already attached");
			}
			else
			{
				Message(1, "Roblox isnt open");
			}
		}
		else
		{
			Message(3, "Could not find nyxbeta.exe\nDid you turn off antivirus?\n\nIf you are this exploits developer, call Update(); somewhere in your code");
		}
	}



    public void Execute(string source)
	{
		if (File.Exists("nyxbeta.exe"))
		{
			if (IsAttached())
			{
				Pipe.NamedPipeSendData(source);
			}
			else
			{
				Message(2, "Attach to execute scripts");
			}
		}
		else
		{
			Message(3, "Could not find nyxbeta.exe\nScript failed to execute");
		}
	}

	public bool IsAttached()
	{
		if (File.Exists("\\\\.\\pipe\\STOPSKIDDINGMYPIPE"))
		{
			return true;
		}
		return false;
	}

	public void Update()
	{
		WebClient webClient = new WebClient();
		if (File.Exists("nyxbeta.exe"))
		{
			File.Delete("nyxbeta.exe");
			webClient.DownloadFile(webClient.DownloadString("https://raw.githubusercontent.com/speedstarkawaii/nyx/main/nyxbeta.link"), "nyxbeta.exe");
			string text = webClient.DownloadString("https://raw.githubusercontent.com/speedstarkawaii/nyx/main/version.doc");
			Message(2, "Updated the API to version " + text.ToString());
		}
		else
		{
			webClient.DownloadFile(webClient.DownloadString("https://raw.githubusercontent.com/speedstarkawaii/nyx/main/nyxbeta.link"), "nyxbeta.exe");
		}
	}

	public bool IsRobloxOpen()
	{
		Process[] processesByName = Process.GetProcessesByName("RobloxPlayerBeta");
		return processesByName.Length != 0;
	}

	public void Message(int shitcase, string message)
	{
		string exploit = Exploit;
		MessageBoxIcon icon;
		switch (shitcase)
		{
		case 1:
			icon = MessageBoxIcon.Exclamation;
			exploit = Exploit + " Warning";
			break;
		case 2:
			icon = MessageBoxIcon.Asterisk;
			exploit = Exploit + " Information";
			break;
		case 3:
			icon = MessageBoxIcon.Hand;
			exploit = Exploit + " Error";
			break;
		default:
			icon = MessageBoxIcon.None;
			exploit = Exploit + " Message";
			break;
		}
		MessageBox.Show(message, exploit, MessageBoxButtons.OK, icon);
	}
}
