using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] CinemachineVirtualCamera cinemachine;
    void Start()
    {
        cinemachine.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;
    }
    void Update()
    {
        cinemachine.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition += speed / 100 * Time.deltaTime;
    }
}
