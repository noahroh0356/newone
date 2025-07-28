using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

public class PodTools
{
    private const string RubyPath = "/Users/nobyeongjun/.rbenv/versions/3.1.4/bin";

    [MenuItem("Tools/Pod/Check Pod Version")]
    public static void CheckPodVersion()
    {
        var psi = new ProcessStartInfo();
        psi.FileName = "/bin/bash";

        string rubyBinPath = "/Users/nobyeongjun/.rbenv/versions/3.1.4/bin";
        string gemPath = "/Users/nobyeongjun/.gem/ruby/3.1.0/bin";

        psi.Arguments = $"-c \"export PATH={rubyBinPath}:{gemPath}:$PATH && pod --version\"";
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

        UnityEngine.Debug.Log($"? [Pod Version] Output:\n{output}");
        if (!string.IsNullOrEmpty(err))
            UnityEngine.Debug.LogError($"? [Pod Version] Error:\n{err}");
    }
    [MenuItem("Tools/Pod/Install CocoaPods")]
    public static void InstallCocoaPods()
    {
        var psi = new ProcessStartInfo();
        psi.FileName = "/bin/bash";

        // ?? ruby ??, gem ?? ??
        string rubyBinPath = "/Users/nobyeongjun/.rbenv/versions/3.1.4/bin";
        string gemPath = "/Users/nobyeongjun/.gem/ruby/3.1.0/bin";

        psi.Arguments = $"-c \"export PATH={rubyBinPath}:{gemPath}:$PATH && gem install cocoapods --user-install\"";
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

        UnityEngine.Debug.Log($"? [CocoaPods Install] Output:\n{output}");
        if (!string.IsNullOrEmpty(err))
            UnityEngine.Debug.LogError($"? [CocoaPods Install] Error:\n{err}");
    }
}
