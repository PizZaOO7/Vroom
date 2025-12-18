using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


using LogitechG29.Sample.Input;
public class SimpleCarRadio : MonoBehaviour
{
    [SerializeField] private InputControllerReader inputControllerReader;

    [Header("Музыка")]
    public List<AudioClip> musicTracks = new List<AudioClip>();
    public AudioSource audioSource;

    float time;

    [Header("UI")]
    public Text trackNameText;
    public GameObject radioPanel;

    private int currentTrackIndex = 0;
    private bool isPlaying = false;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        UpdateUI();
    }

    void Update()
    {
        if (inputControllerReader.EastButton && Time.time > time + 0.1f)
        {
            Debug.Log("radio igraet"); 
            ToggleRadio();
            time = Time.time;
        }

        if (inputControllerReader.SouthButton && isPlaying && Time.time > time + 0.1f)
        {
            NextTrack();
            time = Time.time;
        }

        

        // Автопереход на следующий трек
        if (isPlaying && !audioSource.isPlaying && musicTracks.Count > 0)
        {
            NextTrack();
        }
    }

    public void ToggleRadio()
    {
        if (isPlaying)
        {
            StopRadio();
        }
        else
        {
            PlayRadio();
        }
    }

    void PlayRadio()
    {
        if (musicTracks.Count == 0) return;

        isPlaying = true;

        if (audioSource.clip == null)
        {
            audioSource.clip = musicTracks[currentTrackIndex];
        }

        audioSource.Play();
        UpdateUI();
    }

    void StopRadio()
    {
        isPlaying = false;
        audioSource.Stop();
        UpdateUI();
    }

    void NextTrack()
    {
        if (musicTracks.Count == 0) return;

        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Count;
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();
        UpdateUI();
    }

    void PreviousTrack()
    {
        if (musicTracks.Count == 0) return;

        currentTrackIndex = (currentTrackIndex - 1 + musicTracks.Count) % musicTracks.Count;
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (radioPanel != null)
        {
            radioPanel.SetActive(isPlaying);
        }

        if (trackNameText != null)
        {
            if (isPlaying && musicTracks.Count > 0)
            {
                trackNameText.text = $"Сейчас играет:\n{musicTracks[currentTrackIndex].name}";
            }
            else
            {
                trackNameText.text = "РАДИО ВЫКЛ";
            }
        }
    }

    // Для UI кнопок
    public void UI_ToggleRadio()
    {
        ToggleRadio();
    }

    public void UI_NextTrack()
    {
        NextTrack();
    }

    public void UI_PreviousTrack()
    {
        PreviousTrack();
    }
}