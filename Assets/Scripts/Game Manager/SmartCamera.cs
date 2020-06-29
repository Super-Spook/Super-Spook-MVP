using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    CinemachineVirtualCamera DynamicCamera;
    public GameObject Avatar;
    public bool FlippingCamera;
    public bool ShakingCamera;


    void Start()
    {
        DynamicCamera = GetComponent<CinemachineVirtualCamera>();
        FlippingCamera = false;
        ShakingCamera = false;
    }

    
    void Update()
    {
        if (FlippingCamera)
        {
            FlipCamera();
        }
        if (ShakingCamera)
        {
            ShakeCamera();
        }
    }

    public void FlipCamera()
    {
        if (Avatar.GetComponent<AvatarMovement>().isFacingRight) 
        {            
            DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.10f;
            
        }
        else if (!Avatar.GetComponent<AvatarMovement>().isFacingRight) 
        {
            DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.75f;                    
        }
    }   
    public void ShakeCamera()
    {
        if (Avatar.GetComponent<DashMove>().dashDirection < 0)
        {
            DynamicCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.7f;
            DynamicCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.7f;
        }
        else if (Avatar.GetComponent<DashMove>().dashDirection == 0)
        {
            DynamicCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            DynamicCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
            ShakingCamera = false;
        }
    }
    public void IncreaseLookAhead()
    {
        if (Avatar.GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = 0.6f;
        }
        else
        {
            DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = 0.1f;
        }
    }
}
