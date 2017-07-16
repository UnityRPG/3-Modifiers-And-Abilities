﻿﻿using UnityEngine;

using RPG.Characters;

namespace RPG.CameraUI
{
    public class FollowPlayer : MonoBehaviour
    {
        PlayerControl player;

         
        void Start()
        {
            player = FindObjectOfType<PlayerControl>();
        }

        
        void LateUpdate()
        {
            transform.position = player.transform.position;
        }
    }
}