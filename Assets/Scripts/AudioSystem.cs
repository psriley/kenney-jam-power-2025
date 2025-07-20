using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioSystem : MonoBehaviour
{
    [SerializeField] private AudioSource soundEffectsAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource alarmAudioSource;

    [SerializeField] private List<AudioClip> mineSounds;
    [SerializeField] private List<AudioClip> placeSounds;
    [SerializeField] private List<AudioClip> crankSounds;
    [SerializeField] private List<AudioClip> alarmSounds;
    [SerializeField] private AudioClip uiSound;

    private Coroutine alarmCoroutine;
    private bool alarmActive = false;

    private AudioClip RandomSound(List<AudioClip> sounds)
    {
        if (sounds.Count == 0)
        {
            return null;
        }

        System.Random r = new System.Random();
        int index = r.Next(0, sounds.Count);
        return sounds[index];
    }

    public void PlayMineSound()
    {
        soundEffectsAudioSource.PlayOneShot(RandomSound(mineSounds));
    }

    public void PlayPlaceSound()
    {
        soundEffectsAudioSource.PlayOneShot(RandomSound(placeSounds));
    }

    public void PlayCrankSound()
    {
        soundEffectsAudioSource.PlayOneShot(RandomSound(crankSounds));
    }

    public void PlayAlarm()
    {
        if (alarmCoroutine == null)
        {
            alarmActive = true;
            alarmCoroutine = StartCoroutine(AlarmSound());
        }
    }

    public void StopAlarm()
    {
        if (alarmCoroutine != null)
        {
            alarmActive = false;
            StopCoroutine(alarmCoroutine);
            alarmCoroutine = null;
            alarmAudioSource.Stop();
        }
    }

    private IEnumerator AlarmSound()
    {
        int currentIndex = 0;

        while (alarmActive)
        {
            alarmAudioSource.clip = alarmSounds[currentIndex];
            alarmAudioSource.Play();

            yield return new WaitForSeconds(alarmAudioSource.clip.length);

            if (currentIndex < alarmSounds.Count - 1)
            {
                currentIndex++;
            } else
            {
                currentIndex = 0;
            }
        }
    }
}
