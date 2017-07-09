using UnityEngine;

// Add a UI Socket transform to your enemy
// Attach this script to the socket
// Link to a canvas prefab
namespace RPG.Characters
{
    public class EnemyUI : MonoBehaviour
    {
        [Tooltip("The UI canvas prefab")]
        [SerializeField] GameObject enemyCanvasPrefab = null;

        Camera cameraToLookAt;

        // Use this for initialization 
        void Start()
        {
            cameraToLookAt = Camera.main;
            Instantiate(enemyCanvasPrefab, transform.position, Quaternion.identity, transform);
        }

        // Update is called once per frame 
        void LateUpdate()
        {
            transform.LookAt(cameraToLookAt.transform);
        }
    }
}