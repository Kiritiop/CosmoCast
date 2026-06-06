using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class SpellBookManager : MonoBehaviour
{
    public static SpellBookManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
