using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string SaveFileName = "save.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Save(SaveData data)
    {
        data.saveDate = DateTime.UtcNow.ToString("o");
        File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
        Debug.Log($"Game saved to {SavePath}");
    }

    public SaveData Load()
    {
        if (!File.Exists(SavePath)) return new SaveData();
        return JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
    }

    public bool HasSave() => File.Exists(SavePath);

    public void DeleteSave()
    {
        if (File.Exists(SavePath)) File.Delete(SavePath);
    }
}

[Serializable]
public class SaveData
{
    public string saveDate;

    // Player position
    public float posX, posY, posZ;
    public float rotY;
    public string sceneName;

    // Character customization
    public string characterName = "Player";
    public int bodyTypeIndex;
    public int hairStyleIndex;
    public int hairColorIndex;
    public int skinColorIndex;
    public int outfitIndex;

    // Skill tree
    public string[] unlockedSkillIds = Array.Empty<string>();
    public int skillPoints;

    // Inventory
    public InventoryItemSave[] items = Array.Empty<InventoryItemSave>();
    public int currency;

    // Settings
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public int graphicsQuality = 2;         // 0=Low 1=Med 2=High 3=Ultra
    public float mouseSensitivity = 0.15f;
    public bool fullscreen = true;
}

[Serializable]
public class InventoryItemSave
{
    public string itemId;
    public int quantity;
}
