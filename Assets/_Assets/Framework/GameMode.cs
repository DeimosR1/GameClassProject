using UnityEngine;

public class GameMode : MonoBehaviour
{

    [SerializeField] Player mPlayerGameObjectPrefab;

    Player mPlayerGameObject;

    void Awake()
    {
        PlayerStart playerStart = FindFirstObjectByType<PlayerStart>();
        if (!playerStart)
        {
            throw new System.Exception("Need a player start in the secene for the player Spawn Location and rotation");
        }
        mPlayerGameObject = Instantiate(mPlayerGameObjectPrefab, playerStart.transform.position, playerStart.transform.rotation);
    }
}