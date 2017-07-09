using UnityEngine;

// Add a UI Socket transform to your enemy
// Attach this script to the socket
// Link to a canvas prefab
namespace RPG.Characters
{
    public class FaceCamera : MonoBehaviour
    {
        Camera cameraToLookAt;

        // Use this for initialization 
        void Start()
        {
            cameraToLookAt = Camera.main;
        }

        // Update is called once per frame 
        void LateUpdate()
        {
            transform.LookAt(cameraToLookAt.transform);
        }
    }
}