using NUnit.Framework;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private AudioSource outdoorBgMusic;
    [SerializeField] private AudioSource museumBgMusic;

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

    private void OnEnable()
    {
        EventManager.Instance.OnSceneChanged += ManageSceneMusic;
    }

    private void Start()
    {
        if (outdoorBgMusic != null)
            Debug.Log("[MusicManager] Il manque la 'outdoorBgMusic'");

        if (museumBgMusic != null)
            Debug.Log("[MusicManager] Il manque la 'museumBgMusic'");

        PlayMusic(outdoorBgMusic);

    }

    private void ManageSceneMusic(string nextScene)
    {
        if (nextScene == "Renaissance")
        {
            outdoorBgMusic.Stop();
            PlayMusic(museumBgMusic);
        }
        if (nextScene == "Start")
        {
            museumBgMusic.Stop();
            PlayMusic(outdoorBgMusic);
        }
    }

    private void PlayMusic(AudioSource music)
    {
        music.loop = true;
        music.volume = 0.5f;
        music.Play();
    }

    private void OnDisable()
    {
        EventManager.Instance.OnSceneChanged -= ManageSceneMusic;
    }
}
