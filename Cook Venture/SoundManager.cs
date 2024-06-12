using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip buttonPressSound;
    public AudioClip powerButtonPressSound;
    public AudioClip coinsSound;
    public AudioClip orderTakenAudio;
    public AudioClip crateOpenAudio;
    public List<AudioClip> carHornSounds = new List<AudioClip>();
    public List<AudioClip> carEngineSounds = new List<AudioClip>();
    public List<AudioClip> customerSound = new List<AudioClip>();
    public AudioClip thankYouSound;
    public List<AudioClip> cashierSounds = new List<AudioClip>();
    
    

    public AudioSource audioSource;

    public AudioSource bgAudioSource;


    public bool stopVibration;
    public bool stopSound;
    public bool stopMusic;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySelectionVibration()
    {
        if (stopVibration) return;
        MMVibrationManager.Haptic(HapticTypes.Selection);
    }

    public void PlayMediumVibration()
    {
        if (stopVibration) return;
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }

    public void PlayStrongvibration()
    {
        if (stopVibration) return;
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
    }

    public void OnButtonDownSound() 
    {
        if (stopSound) return;
        audioSource.PlayOneShot(buttonPressSound);
    }

    public void OnPowerDownButton() 
    {
        if (stopSound) return;
        audioSource.PlayOneShot(powerButtonPressSound);
    }

    public void OnCoinGained() 
    {
        if (stopSound) return;
        audioSource.PlayOneShot(coinsSound);
    }
    public void OnOrderTaken() 
    {
        if (stopSound) return;
        audioSource.PlayOneShot(orderTakenAudio);
    }

    public void OnCrateOpenSound() 
    {
        if (stopSound) return;
        audioSource.PlayOneShot(crateOpenAudio);
    } 

    public void StopBgSound()
    {

        bgAudioSource.Stop();
    }

    public void PlayBgSound()
    {
        bgAudioSource.Play();
    }

    public void PlayCarHornSounds()
    {
        StartCoroutine(PlayCarHornSoundsWithDelay());
    }

    IEnumerator PlayCarHornSoundsWithDelay()
    {
        yield return new WaitForSeconds(2.5f);
        int x = Random.Range(0, carHornSounds.Count);
        AudioClip newCarAudioClip = carHornSounds[x];
        audioSource.PlayOneShot(newCarAudioClip);
    }

    public void PlayCarEngineSound()
    {
        StartCoroutine(PlayCarEngineSoundWithDelay());
    }

    IEnumerator PlayCarEngineSoundWithDelay()
    {
        yield return null;
        int x = Random.Range(0, carEngineSounds.Count);
        AudioClip newCarAudioClip = carEngineSounds[x];
        audioSource.PlayOneShot(newCarAudioClip);
    }

    public void PlayThankYouSound()
    {
        int x = Random.Range(0, 3);
        if(x == 1)
        {
            audioSource.PlayOneShot(thankYouSound);
        }
    }

    public void PlayCustomerSound()
    {
        StartCoroutine (PlayCustomerSoundWithDelay());
    }

    IEnumerator PlayCustomerSoundWithDelay()
    {
        yield return new WaitForSeconds(2f);
        int x = Random.Range(0, 2);
        if (x == 1)
        {
            int z  = Random.Range(0,customerSound.Count);
            AudioClip newAudioClip = customerSound[z];
            audioSource.PlayOneShot(newAudioClip);
        }
    }

    public void PlayCashierSound()
    {
        StartCoroutine(PlayCashierSoundWithDelay());
    }
    IEnumerator PlayCashierSoundWithDelay()
    {
        yield return new WaitForSeconds(2f);
        int x = Random.Range(0, 3);
        if (x == 1)
        {
            int z = Random.Range(0, cashierSounds.Count);
            AudioClip newAudioClip = cashierSounds[z];
            audioSource.PlayOneShot(newAudioClip);
        }
    }
}
