using UnityEngine;
using UnityEditor;
using System.Diagnostics;

[InitializeOnLoad]
public class VoicevoxServer
{
    static VoicevoxServer()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            StartVoicevoxServer();
        }
    }

    private static void StartVoicevoxServer()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();

        // Windowsの場合
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C /Voicevox/voicevox_server.bat.lnk";

        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;

        try
        {
            Process process = Process.Start(startInfo);
            UnityEngine.Debug.Log("Voicevoxサーバーを起動しました。");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Voicevoxサーバーの起動に失敗しました: " + e.Message);
        }
    }
}
