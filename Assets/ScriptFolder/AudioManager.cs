using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music Sources")]
    public AudioSource musicSource;
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("SFX Source")]
    public AudioSource sfxSource;

    [Header("SFX Clips")]
    public AudioClip walkStep;
    public AudioClip flyingWind;
    public AudioClip coinPickup;
    public AudioClip totemSplash;
    public AudioClip hoopSuccess;
    public AudioClip volcanoRumble;
    public AudioClip meteorImpact;
    public AudioClip winSound;
    public AudioClip loseSound;

    private void Awake()
    {
        // Singleton Pattern: Ensure only one Audio Manager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive when switching scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Play correct music based on scene
        CheckMusic(SceneManager.GetActiveScene().buildIndex);
    }

    // Call this whenever a scene loads
    public void CheckMusic(int sceneIndex)
    {
        if (sceneIndex == 0) // Main Menu
        {
            PlayMusic(menuMusic);
        }
        else // Game Scene
        {
            PlayMusic(gameMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return; // Don't restart if already playing

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
}