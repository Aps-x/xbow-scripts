using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
	[SerializeField] AudioSource audiosource;
	[SerializeField] AudioClip UIUpDown;
	[SerializeField] Text buildVersion;
	Vector2 cursorOffset = new Vector2(36, 36);
	void Awake()
	{
		buildVersion.text = Application.version;
		Cursor.SetCursor((Texture2D)Resources.Load("Cursors/WhiteCursor"), cursorOffset, CursorMode.Auto);
		Cursor.visible = true;
	}
	public void GameModeSelect(int modeNum)
	{
		GameSettings.GameMode = modeNum;
	}
	public void LevelSelect(string levelName)
	{
		GameSettings.LevelToLoad = levelName;
		PlayGame();
	}
	public void PlayGame()
	{
		SceneManager.LoadScene(1);
	}
	public void QuitGame()
	{
		Application.Quit();
	}
	public void PlaySound()
	{
		audiosource.PlayOneShot(UIUpDown);
	}
}