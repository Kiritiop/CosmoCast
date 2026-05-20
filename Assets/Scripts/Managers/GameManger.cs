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

        SaveData data = SaveManager.Instance.Load();

        // Apply settings immediately (no player needed)
        ApplySettings(data);

        // Apply player data once the player exists
        var player = FindFirstObjectByType<PlayerMovement>();
        if (player != null)
            player.ApplySaveData(data);
    }

    public static void ApplySettings(SaveData data)
    {
        QualitySettings.SetQualityLevel(data.graphicsQuality, true);
        Screen.fullScreen = data.fullscreen;
        AudioListener.volume = data.masterVolume;
    }
}
