using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private const float maxAudioVolume = 0.75f;

    void Awake()
    {
        AudioClip sceneClip = Resources.Load<AudioClip>(string.Format("Musics/{0}", Application.loadedLevelName));

        audio.clip = sceneClip;
        audio.volume = 1;
        audio.loop = true;
        audio.Play();

        StartCoroutine(IncreaseVolume());
    }

    IEnumerator IncreaseVolume()
    {
        while (audio.volume < maxAudioVolume)
        {
            audio.volume += Time.deltaTime;

            yield return 0;
        }

        audio.volume = Mathf.Clamp(audio.volume, 0, maxAudioVolume);
    }
}
