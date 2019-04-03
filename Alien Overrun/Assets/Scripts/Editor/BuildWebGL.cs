// Based on: https://www.gamasutra.com/blogs/EnriqueJGil/20160808/278440/Unity_Builds_Scripting_Basic_and_advanced_possibilities.php

using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class BuildWebGL : MonoBehaviour
{
	static private string zipExe = @"C:\Program Files\7-Zip\7z.exe";
	static private string destinationFolder = @"C:\!\Unity\Alien Overrun Build";
	static private string destinationFile = @"C:\!\Unity\Alien Overrun.zip";
	static private string[] levels = new string[]
	{
		"Assets/Scenes/Main Menu.unity",
		"Assets/Scenes/Main.unity",
	};

	[MenuItem( "Builds/Build WebGL and zip it" )]
	public static void MakeBuild( )
	{
		bool buildSuccessful = Build( );

		if ( buildSuccessful )
		{
#if UNITY_EDITOR_WIN
			ZpiWindows( );
			CleanUp( );
#elif UNITY_EDITOR_OSX
			//ZipOSX( );
			//CleanUp( );
#else
			//ZipLinux( );
			//CleanUp( );
#endif
		}
	}

	private static bool Build( )
	{
		var returnMsg = BuildPipeline.BuildPlayer
		(
			levels,
			destinationFolder,
			BuildTarget.WebGL,
			BuildOptions.ShowBuiltPlayer
		);

		if ( returnMsg.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded )
		{
			UnityEngine.Debug.Log( "WebGL build complete..." );
			return true;
		}
		else
		{
			UnityEngine.Debug.LogError( "Error building WebGL:\n" + returnMsg );
			return false;
		}
	}

	private static void ZpiWindows( )
	{
		StringBuilder outputBuilder = new StringBuilder( );
		Process process = new Process( );
		process.StartInfo.FileName = zipExe;
		process.StartInfo.Arguments = " a \"" + destinationFile + "\" \"" + destinationFolder + @"\" + "\"*";

		// Used to capture 7zip output
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.OutputDataReceived += new DataReceivedEventHandler
		(
			delegate ( object sender, DataReceivedEventArgs e )
			{
				outputBuilder.AppendLine( e.Data );
			}
		);

		process.Start( );
		process.BeginOutputReadLine( );
		process.WaitForExit( );
		process.CancelOutputRead( );

		UnityEngine.Debug.Log( outputBuilder.ToString( ) );
	}

	private static void CleanUp( )
	{
		Directory.Delete( destinationFolder, true );
	}
}
