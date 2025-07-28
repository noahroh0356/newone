using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class RubyTest
{
    [MenuItem("Tools/Check Ruby in Unity")]
    public static void CheckRuby()
    {
        var psi = new ProcessStartInfo();
        psi.FileName = "/bin/bash";
        psi.Arguments = "-c \"export PATH=/Users/nobyeongjun/.rbenv/versions/3.1.4/bin:$PATH && ruby -v\"";
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;

        var proc = new Process();
        proc.StartInfo = psi;
        proc.Start();

        string output = proc.StandardOutput.ReadToEnd();
        string err = proc.StandardError.ReadToEnd();
        proc.WaitForExit();

        UnityEngine.Debug.Log("✅ Ruby Version Output:\n" + output);
        if (!string.IsNullOrEmpty(err))
        {
            UnityEngine.Debug.LogError("❌ Ruby Error:\n" + err);
        }
    }
}
