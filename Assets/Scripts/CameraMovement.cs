using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float max = 1;
    [SerializeField] float speed = 5;
    bool goUp = true;
    [SerializeField] CinemachineVirtualCamera cinemachine;
    void Update()
    {
        if (goUp)
            cinemachine.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition += speed / 100 * Time.deltaTime;
    }
}
