//using System.Collections;
//using UnityEngine;

//public class VoiceVoxTest : MonoBehaviour
//{
//    [SerializeField]
//    private AudioSource _audioSource;

//    void Start()
//    {
//        StartCoroutine(SpeakTest("????????I?????Unity??VOICEVOX???g?????I"));
//    }

//    IEnumerator SpeakTest(string text)
//    {
//        // VOICEVOX??REST-API?N???C?A???g
//        VoiceVoxApiClient client = new VoiceVoxApiClient();

//        // ?e?L?X?g????AudioClip????i?b???u3:???????v?j
//        yield return client.TextToAudioClip(3, text);

//        if (client.AudioClip != null)
//        {
//            // AudioClip???��???AAudioSource??A?^?b?`
//            _audioSource.clip = client.AudioClip;
//            // AudioSource????
//            _audioSource.Play();
//        }
//    }
//}
