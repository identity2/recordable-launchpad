using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;

public class SetXCodePlist
{
	[PostProcessBuild]
	public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
	{
		if (buildTarget == BuildTarget.iOS) {
			//get plist
			string plistPath = pathToBuiltProject + "/Info.plist";
			PlistDocument plist = new PlistDocument ();
			plist.ReadFromString (File.ReadAllText (plistPath));

			//get root
			PlistElementDict rootDict = plist.root;

			rootDict.CreateDict ("NSMicrophoneUsageDescription");
			rootDict.SetString ("NSMicrophoneUsageDescription", "Record you own voice as the beat");

			File.WriteAllText (plistPath, plist.WriteToString ());
		}
	}
}