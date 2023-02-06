using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Loading transitioningUtility;
    [SerializeField] float transitionTime;
    [SerializeField] int newSceneIndex;

    private void Start()
    {
        transitioningUtility = GetComponentInChildren<Loading>();
        StartCoroutine(transitioningUtility.LoadNewScene(transitionTime));
    }

    public void LoadScene(int oldSceneIndex)
    {
        newSceneIndex = oldSceneIndex;
        StartCoroutine(transitioningUtility.StartSceneTansition(transitionTime, 
            () => { SceneManager.LoadScene(newSceneIndex); })
            );
    }


}
