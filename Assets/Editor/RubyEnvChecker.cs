using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class RubyEnvChecker
{
    [MenuItem("Tools/Check Ruby Version and PATH")]
    public static void CheckRubyEnv()
    {
        var process = new Process();
        process.StartInfo.FileName = "ruby";
        process.StartInfo.Arguments = "-v";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();
        string rubyVersion = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        UnityEngine.Debug.Log("Ruby Version: " + rubyVersion);

        // PATH 확인
        var envProcess = new Process();
        envProcess.StartInfo.FileName = "env";
        envProcess.StartInfo.RedirectStandardOutput = true;
        envProcess.StartInfo.UseShellExecute = false;
        envProcess.StartInfo.CreateNoWindow = true;

        envProcess.Start();
        string envOutput = envProcess.StandardOutput.ReadToEnd();
        envProcess.WaitForExit();

        UnityEngine.Debug.Log("Environment variables:\n" + envOutput);
    }
}
