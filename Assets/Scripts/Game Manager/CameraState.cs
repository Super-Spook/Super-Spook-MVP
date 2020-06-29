using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraState : MonoBehaviour
{
    public CinemachineVirtualCamera DynamicCamera;

    public float changeSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Avatar"))
        {
            if(DynamicCamera.m_Lens.OrthographicSize == 8 && DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY == 0.6f)
            {
                DynamicCamera.m_Lens.OrthographicSize = 10;
                DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.4f;
            }
            else if (DynamicCamera.m_Lens.OrthographicSize == 10 && DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY == 0.4f)
            {
                DynamicCamera.m_Lens.OrthographicSize = 8;
                DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.6f;
            }
            //if (DynamicCamera.m_Lens.OrthographicSize <= 10 && DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY >= 0.4f)
            //{
                //DynamicCamera.m_Lens.OrthographicSize += changeSpeed * Time.deltaTime;
                //DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY -= changeSpeed * Time.deltaTime;
            //}
            
        }
    }
}
