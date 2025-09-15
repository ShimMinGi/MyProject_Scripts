using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider volumeSlider;    
    public AudioSource audioSource;

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            audioSource.loop = true;
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

    }

    void OnVolumeChanged(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value;
        }
    }

    public void UpdateVolumeSlider()
    {
        if (volumeSlider != null && audioSource != null)
        {
            volumeSlider.value = audioSource.volume;
        }
    }
}
