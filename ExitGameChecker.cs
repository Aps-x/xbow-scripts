using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class ExitGameChecker : MonoBehaviour
{
	List<GameObject> players = new List<GameObject>();
	[SerializeField] Text countdownText;
    float timeToStart = 3f;
    float countdownTimer;
    int playersInZone;
	bool gameOver;
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playersInZone++;
			players.Add(other.gameObject);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !gameOver)
		{
			playersInZone--;
			players.Remove(other.gameObject);
		}
	}
	void Update()
	{
		if (playersInZone > 0 && playersInZone == PlayerInputManager.instance.playerCount)
		{
			countdownText.gameObject.SetActive(true);
			countdownTimer -= Time.deltaTime;
			countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
			if (countdownTimer <= 0 && !gameOver)
			{
				gameOver = true;
				foreach(GameObject player in players)
				{
					Destroy(player);
				}
				SceneManager.LoadScene(0);
			}
		}
		else
		{
			countdownText.gameObject.SetActive(false);
			countdownTimer = timeToStart;
		}
	}
}