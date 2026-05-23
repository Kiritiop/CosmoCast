using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    private AudioSource _audioSource;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.HasSave())
            SetVolume(SaveManager.Instance.Load().musicVolume);
    }

    public void SetVolume(float value)
    {
        _audioSource.volume = Mathf.Clamp01(value);
    }
}