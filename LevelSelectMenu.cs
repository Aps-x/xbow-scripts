using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectMenu : MonoBehaviour
{
	[SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip UIUpDown;
	public void PlaySound()
	{
		audiosource.PlayOneShot(UIUpDown);
	}
	public void LevelSelect(string levelName)
	{
		SceneManager.LoadScene(levelName);
	}
	public void BackToMainMenu()
	{
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
		{
			Destroy(player);
		}
		SceneManager.LoadScene(0);
	}
}