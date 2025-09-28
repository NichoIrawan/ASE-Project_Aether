using UnityEngine;

public class LevelSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    private void Start()
    {
        player.transform.position = new Vector3(0, 0, 0);
    }
}
