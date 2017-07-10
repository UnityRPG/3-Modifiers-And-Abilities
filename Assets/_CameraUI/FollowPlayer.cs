using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Characters;

namespace RPG.CameraUI
{
    public class FollowPlayer : MonoBehaviour
    {
        PlayerControl player;

        // Use this for initialization
        void Start()
        {
            player = FindObjectOfType<PlayerControl>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = player.transform.position;
        }
    }
}