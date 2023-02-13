using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] SceneLoader sceneLoader;
    private void Awake()
    {
        sceneLoader = GetComponentInParent<SceneLoader>();
    }
    private void Update()
    {
        if (Input.anyKeyDown)
            sceneLoader.LoadScene(1);
    }
}
