﻿﻿using UnityEngine;

namespace RPG.Characters
{
    public class FaceCamera : MonoBehaviour
    {
        Camera cameraToLookAt;
          
        void Start()
        {
            cameraToLookAt = Camera.main;
        }

         
        void LateUpdate()
        {
            transform.LookAt(cameraToLookAt.transform);
        }
    }
}