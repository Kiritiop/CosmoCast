using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }


    
}
