using UnityEngine;
public class FollowPlayer : MonoBehaviour
{
    public Transform playerToFollow;
    void Update()
    {
        if (playerToFollow != null)
		{
            transform.position = playerToFollow.position;
        }
    }
}