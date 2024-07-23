using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace NyxAPI;

internal class Pipe
{
	private const uint GENERIC_READ = 2147483648u;

	private const uint GENERIC_WRITE = 1073741824u;

	private const uint OPEN_EXISTING = 3u;

	private const int INVALID_HANDLE_VALUE = -1;

	public string name = "\\\\\\\\.\\\\pipe\\\\STOPSKIDDINGMYPIPE";

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool CloseHandle(IntPtr hObject);

	public static bool NamedPipeExists(string pipe)
	{
		IntPtr hObject = CreateFile(pipe, 3221225472u, 0u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
		if (hObject.ToInt64() != -1)
		{
			CloseHandle(hObject);
			return true;
		}
		return false;
	}

	public static void NamedPipeSendData(string script)
	{
		try
		{
			using NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", "STOPSKIDDINGMYPIPE", PipeDirection.Out);
			namedPipeClientStream.Connect();
			using StreamWriter streamWriter = new StreamWriter(namedPipeClientStream);
			streamWriter.AutoFlush = true;
			streamWriter.WriteLine(script);
		}
		catch (Exception ex)
		{
			Nyx nyx = new Nyx();
			nyx.Message(3, "Exception while executing! " + ex.Message.ToString());
			Environment.Exit(-1);
		}
	}
}
