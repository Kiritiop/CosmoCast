using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    private bool _isPaused;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null) return;
        if (!Keyboard.current.escapeKey.wasPressedThisFrame) return;

        // Settings panel takes priority — back out of it first
        if (settingsPanel.activeSelf) { BackFromSettings(); return; }

        // Pause menu is open — resume
        if (_isPaused) { Resume(); return; }

        // Pause always takes priority over battle
        Pause();
    }

    public bool IsPaused => _isPaused;

    public void TogglePause()
    {
        if (_isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _isPaused = true;
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;

        if (BattleManager.Instance != null && BattleManager.Instance.IsInBattle)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackFromSettings()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        GameManger.Instance?.SaveGame();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Call these from your settings panel sliders / dropdowns
    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;
        PatchSetting(d => d.masterVolume = value);
    }

    public void SetMusicVolume(float value)
    {
        BGMManager.Instance?.SetVolume(value);
        PatchSetting(d => d.musicVolume = value);
    }
    public void SetSfxVolume(float value)    => PatchSetting(d => d.sfxVolume = value);

    public void SetGraphicsQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PatchSetting(d => d.graphicsQuality = index);
    }

    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
        PatchSetting(d => d.fullscreen = value);
    }

    public void SetMouseSensitivity(float value)
    {
        FindFirstObjectByType<PlayerMovement>()?.ApplySaveSensitivity(value);
        PatchSetting(d => d.mouseSensitivity = value);
    }

    private void PatchSetting(System.Action<SaveData> patch)
    {
        if (SaveManager.Instance == null) return;
        SaveData data = SaveManager.Instance.HasSave()
            ? SaveManager.Instance.Load()
            : new SaveData();
        patch(data);
        SaveManager.Instance.Save(data);
    }
}
