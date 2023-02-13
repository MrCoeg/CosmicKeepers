using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Parallax : MonoBehaviour
{
    Transform cameraTransform;
    [SerializeField] float parallaxSpeed;
    [SerializeField] float oldCameraPos;
    [SerializeField] float newCameraPos;
    [SerializeField] List<Transform> transfroms = new List<Transform>();

    private float biggestZIndex;
    private void Awake()
    {
        transfroms.ForEach(x => InstantiatePosition(x.GetComponentsInChildren<Transform>().ToList()));
        cameraTransform = Camera.main.transform;
        oldCameraPos = cameraTransform.position.x;
        biggestZIndex = transfroms[0].position.x;
        transfroms.ForEach(x => FindBiggestZIndex(x.position.z));
    }

    private void Update()
    {
        if (oldCameraPos == cameraTransform.position.x) return;
        else
        {
            newCameraPos = cameraTransform.position.x;
            transfroms.ForEach(x => MoveParallax(x));
            oldCameraPos = newCameraPos;

        }

    }

    private void InstantiatePosition(List<Transform> listTransform)
    {
        float pos = -19.2f*2;
        listTransform.ForEach(x => x.position = new Vector3(pos += 19.2f, x.position.y, x.position.z));
    }

    private void FindBiggestZIndex(float zIndex)
    {
        if (biggestZIndex < zIndex) biggestZIndex = zIndex;
    }

    private void MoveParallax(Transform transformToMove)
    {
        float parallaxMove = (newCameraPos - oldCameraPos) * (biggestZIndex - transformToMove.position.z) * parallaxSpeed;
        transformToMove.gameObject.transform.position += new Vector3(-parallaxMove, 0) * Time.deltaTime;

    }

}
