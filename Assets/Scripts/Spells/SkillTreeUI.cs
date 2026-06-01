using UnityEngine;
using UnityEngine.InputSystem;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] GameObject SpellBook_Bg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpellBook_Bg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    public void GetInput()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame &&  SpellBook_Bg.activeSelf == false)
        {
            OpenSpellBook();
        }
        else if (Keyboard.current.qKey.wasPressedThisFrame && SpellBook_Bg.activeSelf)
        {
            CloseSpellBook();
        }
    }
    private void OpenSpellBook()
    {
        //PlayerMovement.IsSpellBookOpen = true;
        Time.timeScale = 0f;
        SpellBook_Bg.SetActive (true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    private void CloseSpellBook()
    {
        //PlayerMovement.IsSpellBookOpen = false;
        Time.timeScale = 1f;
        SpellBook_Bg.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
