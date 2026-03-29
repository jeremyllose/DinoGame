using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music Source")]
    public AudioSource musicSource; // Drag the Audio Source component here!

    [Header("Music Tracks")]
    public AudioClip menuMusic;   // Scene 0
    public AudioClip level1Music; // Level 1
    public AudioClip level2Music; // Level 2
    public AudioClip level3Music; // Level 3
    public AudioClip level4Music; // Level 4

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Default check for Main Menu (Scene 0)
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayMusic(menuMusic);
        }
    }

    public void PlayLevelMusic(int levelIndex)
    {
        switch (levelIndex)
        {
            case 1: PlayMusic(level1Music); break;
            case 2: PlayMusic(level2Music); break;
            case 3: PlayMusic(level3Music); break;
            case 4: PlayMusic(level4Music); break;
            default: PlayMusic(level1Music); break; 
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null) return; // Safety Check
        if (clip == null) return;
        
        if (musicSource.clip == clip) return; // Don't restart if already playing

        musicSource.clip = clip;
        musicSource.Play();
    }

    // --- DUMMY FUNCTION ---
    // We keep this here so LevelManager doesn't crash, 
    // but it does nothing (Sound Effects are disabled).
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        // Do nothing.
    }
}