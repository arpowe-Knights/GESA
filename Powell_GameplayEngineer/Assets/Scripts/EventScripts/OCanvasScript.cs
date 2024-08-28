using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OCanvasScript : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;

    private bool enabled;

    private void Awake()
    {
        // Disable by default
        objectToDisable.SetActive(enabled);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            Toggle();
    }

    private void Toggle()
    {
        Debug.Log("TEST3");

        enabled = !enabled;
        objectToDisable.SetActive(enabled);
    }
}
