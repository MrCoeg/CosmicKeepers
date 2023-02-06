using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Image imageTransition;
    public delegate void LoadSceneCallback();

    private void Awake()
    {
        imageTransition =  GetComponent<Image>();
    }

    public IEnumerator StartSceneTansition(float transitionTime, LoadSceneCallback callback) {
        byte alpha = 0;
        float alphaShiftSpeed = transitionTime / 255;
        while(alpha < 255)
        {
            imageTransition.color = new Color32(0, 0, 0, alpha++);
            yield return new WaitForSeconds(alphaShiftSpeed);
        }
        callback();
    }

    public IEnumerator LoadNewScene(float transitionTime)
    {
        byte alpha = 255;
        float alphaShiftSpeed = transitionTime / 255;
        while (alpha > 0)
        {
            imageTransition.color = new Color32(0, 0, 0, alpha--);
            yield return new WaitForSeconds(alphaShiftSpeed);
        }
    }
}
