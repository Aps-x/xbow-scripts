using UnityEngine;
using System.Collections.Generic;
public class FlashlightManager : MonoBehaviour
{
    List<GameObject> playerList = new List<GameObject>();
    [SerializeField] FollowPlayer[] flashLights;
    void Start()
    {
        // Find players and add them to list
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerList.Add(player);
        }
        // Attach flashlight to corresponding player
		for (int i = 0; i < playerList.Count; i++)
		{
            flashLights[i].playerToFollow = playerList[i].transform;
		}
	}
}