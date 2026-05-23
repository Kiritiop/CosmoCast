using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance { get; private set; }

    [Header("Auto-Save")]
    [SerializeField] private float autoSaveInterval = 60f;

    private float _autoSaveTimer;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadGame();
    }

    void Update()
    {
        _autoSaveTimer += Time.deltaTime;
        if (_autoSaveTimer >= autoSaveInterval)
        {
            _autoSaveTimer = 0f;
            SaveGame();
        }
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        if (SaveManager.Instance == null) return;

        var data = SaveManager.Instance.HasSave()
            ? SaveManager.Instance.Load()
            : new SaveData();

        // Player position
        var player = FindFirstObjectByType<PlayerMovement>();
        if (player != null)
            player.PopulateSaveData(data);

        data.sceneName = SceneManager.GetActiveScene().name;

        SaveManager.Instance.Save(data);
    }

    public void LoadGame()
    {
        if (SaveManager.Instance == null || !SaveManager.Instance.HasSave()) return;

        // Player loads its own position in Start() after _controller is ready
        ApplySettings(SaveManager.Instance.Load());
    }

    public static void ApplySettings(SaveData data)
    {
        QualitySettings.SetQualityLevel(data.graphicsQuality, true);
        Screen.fullScreen = data.fullscreen;
        AudioListener.volume = data.masterVolume;
        BGMManager.Instance?.SetVolume(data.musicVolume);
    }
}
