using UnityEngine;
using System.Collections;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager Instance;

    public AudioClip sfxPlayerHit;
    public AudioClip sfxPlayerOuch;
    public AudioClip sfxWaterSplash;
    public AudioClip sfxPlayerCollision;
    public AudioClip sfxWaterDive;
    public AudioClip sfxAlligatorBite;
    public AudioClip sfxBonus;
    public AudioClip sfxFail;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySfxFail(float waitTime)
    {
        StartCoroutine(CallSfx(waitTime, sfxFail, 0.25f));
    }

    public void PlaySfxBonus(float waitTime)
    {
        StartCoroutine(CallSfx(waitTime, sfxBonus, 0.25f));
    }

    public void PlaySfxPlayerHit(float waitTime)
    {
        StartCoroutine(CallSfx(waitTime, sfxPlayerHit));
    }

    public void PlaySfxPlayerOuch(float waitTime)
    {
        StartCoroutine(CallSfx(waitTime, sfxPlayerOuch));
    }

    public void PlaySfxWaterSplash(float waitTime)
    {
        StartCoroutine(CallSfx(waitTime, sfxWaterSplash));
    }

    public void PlaySfxPlayerCollision(float waitTime)
    {
        StartCoroutine(CallSfx(waitTime, sfxPlayerCollision));
    }

    public void PlaySfxWaterDive(float waitTime)
    {
        StartCoroutine(CallSfx(waitTime, sfxWaterDive));
    }

    public void PlaySfxAlligatorBite(float waitTime)
    {
        StartCoroutine(CallSfx(waitTime, sfxAlligatorBite));
    }

    private IEnumerator CallSfx(float waitTime, AudioClip clip, float volume = 1)
    {
        yield return new WaitForSeconds(waitTime);

        PlayAudioEffects(clip, volume);
    }

    private void PlayAudioEffects(AudioClip clipToPlay, float volume)
    {
        AudioSource.PlayClipAtPoint(clipToPlay, transform.position, volume);
    }
}
