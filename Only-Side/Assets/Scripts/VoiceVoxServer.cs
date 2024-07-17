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

        // Windows�̏ꍇ
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C /Voicevox/voicevox_server.bat.lnk";

        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;

        try
        {
            Process process = Process.Start(startInfo);
            UnityEngine.Debug.Log("Voicevox�T�[�o�[���N�����܂����B");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Voicevox�T�[�o�[�̋N���Ɏ��s���܂���: " + e.Message);
        }
    }
}
