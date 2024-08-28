using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCanvasScript : MonoBehaviour
{
    private PlayerControls _playerControls;

    [SerializeField] private GameObject objectToDisable;

    private bool enabled;

    private void Awake()
    {
        _playerControls = new PlayerControls();

        _playerControls.Enable();


        // Disable by default
        objectToDisable.SetActive(enabled);
    }

    private void DoThing(InputAction.CallbackContext test)
    {
        // Toggle
        enabled = !enabled;

        objectToDisable.SetActive(enabled);

        Debug.Log("TEST");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Toggle();
    }

    private void Toggle()
    {
        Debug.Log("TEST2");

        enabled = !enabled;
        objectToDisable.SetActive(enabled);
    }
}