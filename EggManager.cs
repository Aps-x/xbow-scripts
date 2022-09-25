using UnityEngine;
using UnityEngine.InputSystem;
public class EggManager : MonoBehaviour
{
    [SerializeField] GameObject[] eggs;
	void Awake()
	{
		if (GameSettings.GameMode == 1)
		{
            eggs[2].gameObject.SetActive(false);
            eggs[3].gameObject.SetActive(false);
            eggs[4].gameObject.SetActive(true);
            eggs[5].gameObject.SetActive(true);
        }
	}
	void OnPlayerJoined(PlayerInput playerInput)
    {
        switch (playerInput.playerIndex)
        {
            case 0:
                eggs[0].gameObject.SetActive(false);
                break;
            case 1:
                eggs[1].gameObject.SetActive(false);
                break;
            case 2:
                eggs[2].gameObject.SetActive(false);
                eggs[4].gameObject.SetActive(false);
                break;
            case 3:
                eggs[3].gameObject.SetActive(false);
                eggs[5].gameObject.SetActive(false);
                break;
        }
    }
    void OnPlayerLeft(PlayerInput playerInput)
	{
        switch (playerInput.playerIndex)
        {
            case 0:
                eggs[0].gameObject.SetActive(true);
                break;
            case 1:
                eggs[1].gameObject.SetActive(true);
                break;
            case 2:
                if (GameSettings.GameMode == 1)
				{
                    eggs[4].gameObject.SetActive(true);
                }
                else
				{
                    eggs[2].gameObject.SetActive(true);
                }
                break;
            case 3:
                if (GameSettings.GameMode == 1)
                {
                    eggs[5].gameObject.SetActive(true);
                }
                else
                {
                    eggs[3].gameObject.SetActive(true);
                }
                break;
        }
    }
}