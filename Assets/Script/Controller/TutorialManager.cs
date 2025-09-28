using UnityEngine;

public class TutorialManager : MonoBehaviour
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
        player.transform.position = new Vector3(-0.5f, 4.34f, 0);
    }
}
