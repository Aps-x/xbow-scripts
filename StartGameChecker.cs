using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class StartGameChecker : MonoBehaviour
{
	[SerializeField] Text countdownText;
	float timeToStart = 3f;
	float countdownTimer;
	int playersInZone;
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player"))
		{
			playersInZone++;
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playersInZone--;
		}
	}
	void Update()
	{
		if (GameSettings.GameMode == 1)
		{
			if (playersInZone == 4)
			{
				countdownText.gameObject.SetActive(true);
				countdownTimer -= Time.deltaTime;
				countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
				if (countdownTimer <= 0)
				{
					SceneManager.LoadScene(GameSettings.LevelToLoad);
				}
			}
			else
			{
				countdownText.gameObject.SetActive(false);
				countdownTimer = timeToStart;
			}
		}
		else
		{
			if (playersInZone > 1 && playersInZone == PlayerInputManager.instance.playerCount)
			{
				countdownText.gameObject.SetActive(true);
				countdownTimer -= Time.deltaTime;
				countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
				if (countdownTimer <= 0)
				{
					SceneManager.LoadScene(GameSettings.LevelToLoad);
				}
			}
			else
			{
				countdownText.gameObject.SetActive(false);
				countdownTimer = timeToStart;
			}
		}
	}
}