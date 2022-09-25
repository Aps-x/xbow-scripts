using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    #region Header
    protected List<Player> playerList = new List<Player>();
    [Header("References")]
    [SerializeField] protected Transform[] spawnPoints;
    [SerializeField] protected Image readyImage;
    [SerializeField] protected Image goImage;
    [SerializeField] protected GameObject scoreboard;
    [SerializeField] protected GameObject endScreen;
    [SerializeField] protected AudioSource audioSource;
    [Header("Player ScoreCards")]
    [SerializeField] protected GameObject redDinoScoreCard;
    [SerializeField] protected GameObject blueDinoScoreCard;
    [SerializeField] protected GameObject greenDinoScoreCard;
    [SerializeField] protected GameObject yellowDinoScoreCard;
    [Header("Player Points[]")]
    [SerializeField] protected Image[] redDinoPoints;
    [SerializeField] protected Image[] blueDinoPoints;
    [SerializeField] protected Image[] greenDinoPoints;
    [SerializeField] protected Image[] yellowDinoPoints;
    [Header("Overflow Points")]
    [SerializeField] protected Text overflowTextRed;
    [SerializeField] protected Text overflowTextBlue;
    [SerializeField] protected Text overflowTextGreen;
    [SerializeField] protected Text overflowTextYellow;
    [Header("Egg sprites")]
    [SerializeField] protected Sprite redEgg;
    [SerializeField] protected Sprite blueEgg;
    [SerializeField] protected Sprite greenEgg;
    [SerializeField] protected Sprite yellowEgg;
    [Header("Player Avatars")]
    [SerializeField] protected GameObject redDinoAvatar;
    [SerializeField] protected GameObject blueDinoAvatar;
    [SerializeField] protected GameObject greenDinoAvatar;
    [SerializeField] protected GameObject yellowDinoAvatar;
    [Header("Teams Game Manager")]
    [SerializeField] GameObject teamsGameManagerObject;
    // Variables
    protected int playerCount;
    protected bool suddenDeath;
    protected int maxScore = 10;
    protected bool resetInProgress;
    protected int numActivePlayers;
    protected int overflowCounterRed;
    protected int overflowCounterBlue;
    protected int overflowCounterGreen;
    protected int overflowCounterYellow;
    #endregion
    public virtual void Awake()
    {
        if (GameSettings.GameMode == 1)
		{
            // Find players and then add them to the list of players
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                playerList.Add(player.GetComponent<Player>());
            }
            teamsGameManagerObject.SetActive(true);
            return;
        }
		else
		{
            // Find players and then add them to the list of players
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                playerList.Add(player.GetComponent<Player>());
            }
            // Sort by playerID
            playerList.Sort((p1, p2) => p1.playerID.CompareTo(p2.playerID));
            // Dependency Injection
            foreach (Player player in playerList)
            {
                player.Initialize(this);
            }
            // Scoreboard size
            switch (playerList.Count)
            {
                case 2:
                    redDinoScoreCard.SetActive(true);
                    blueDinoScoreCard.SetActive(true);
                    greenDinoScoreCard.SetActive(false);
                    yellowDinoScoreCard.SetActive(false);
                    break;
                case 3:
                    redDinoScoreCard.SetActive(true);
                    blueDinoScoreCard.SetActive(true);
                    greenDinoScoreCard.SetActive(true);
                    yellowDinoScoreCard.SetActive(false);
                    break;
                case 4:
                    redDinoScoreCard.SetActive(true);
                    blueDinoScoreCard.SetActive(true);
                    greenDinoScoreCard.SetActive(true);
                    yellowDinoScoreCard.SetActive(true);
                    break;
            }
            // Startup
            playerCount = playerList.Count;
            StartCoroutine("RoundStart");
        }
    }
    IEnumerator RoundStart()
    {
        numActivePlayers = playerCount;
        // Player Spawning
        foreach (Player player in playerList)
        {
            if (!player.outOfRotation)
            {
                // Initial state
                player.enabled = true;
                player.inputDisabled = true;
                player.invulnerable = true;
                player.isDead = true;
                // Spawn position
                switch (player.spawnTicket)
                {
                    case 0:
                        player.transform.position = spawnPoints[0].position;
                        player.SpawnRotation(0, false);
                        break;
                    case 1:
                        player.transform.position = spawnPoints[1].position;
                        player.SpawnRotation(-90, true);
                        break;
                    case 2:
                        player.transform.position = spawnPoints[2].position;
                        player.SpawnRotation(180, true);
                        break;
                    case 3:
                        player.transform.position = spawnPoints[3].position;
                        player.SpawnRotation(90, false);
                        break;
                }
                // Update spawn ticket
                player.spawnTicket++;
                if (player.spawnTicket == 4) { player.spawnTicket = 0; }
            }
        }
        // Pre-round effects
        yield return new WaitForSeconds(1);
        readyImage.gameObject.SetActive(true);// READY?
        yield return new WaitForSeconds(1);
        readyImage.gameObject.SetActive(false);
        goImage.gameObject.SetActive(true);// GO!
        // Players can now move
        foreach (Player player in playerList)
        {
            if (!player.outOfRotation)
            {
                player.inputDisabled = false;
                player.invulnerable = false;
                player.isDead = false;
            }
        }
        yield return new WaitForSeconds(0.5f);
        // Screen clear
        readyImage.gameObject.SetActive(false);
        goImage.gameObject.SetActive(false);
    }
    public virtual void PlayerKnockout(int killerID, int playerID)
    {
        // Reduce number of active players
        numActivePlayers--;
        // Update score
        switch (killerID)
        {
            case 0:
                // RedDino/Player 1 update and display score
                playerList[0].playerScore++;
                for (int i = 0; i < playerList[0].playerScore; i++)
                {
                    if (i < redDinoPoints.Length)
                    {
                        redDinoPoints[i].sprite = redEgg;
                    }
                    else
                    {
                        overflowCounterRed = playerList[0].playerScore - 10;
                        overflowTextRed.text = "+" + overflowCounterRed.ToString();
                    }
                }
                break;
            case 1:
                // BlueDino/Player 2 update and display score
                playerList[1].playerScore++;
                for (int i = 0; i < playerList[1].playerScore; i++)
                {
                    if (i < blueDinoPoints.Length)
                    {
                        blueDinoPoints[i].sprite = blueEgg;
                    }
                    else
                    {
                        overflowCounterBlue = playerList[1].playerScore - 10;
                        overflowTextBlue.text = "+" + overflowCounterBlue.ToString();
                    }
                }
                break;
            case 2:
                // GreenDino/Player 3 update and display score
                playerList[2].playerScore++;
                for (int i = 0; i < playerList[2].playerScore; i++)
                {
                    if (i < greenDinoPoints.Length)
                    {
                        greenDinoPoints[i].sprite = greenEgg;
                    }
                    else
                    {
                        overflowCounterGreen = playerList[2].playerScore - 10;
                        overflowTextGreen.text = "+" + overflowCounterGreen.ToString();
                    }
                }
                break;
            case 3:
                // YellowDino/Player 4 update and display score
                playerList[3].playerScore++;
                for (int i = 0; i < playerList[3].playerScore; i++)
                {
                    if (i < yellowDinoPoints.Length)
                    {
                        yellowDinoPoints[i].sprite = yellowEgg;
                    }
                    else
                    {
                        overflowCounterYellow = playerList[3].playerScore - 10;
                        overflowTextYellow.text = "+" + overflowCounterYellow.ToString();
                    }
                }
                break;
            case 4:
                // Death to arrow trap, do nothing
                break;
        }
        // Check if game is over
        if (numActivePlayers <= 1 && !resetInProgress)
        {
            resetInProgress = true;
            StartCoroutine("RoundOver");
        }
    }
    IEnumerator RoundOver()
    {
        // Delay
        yield return new WaitForSeconds(1.5f);
        // Disable player movement
        foreach (Player player in playerList)
        {
            if (!player.outOfRotation)
            {
                player.inputDisabled = true;
                player.invulnerable = true;
                player.isDead = true;
            }
        }
        // Check if max score is reached
        foreach (Player player in playerList)
		{
            if (player.playerScore >= maxScore)
			{
                maxScore = player.playerScore;
                suddenDeath = true;
			}
		}
        // Player elimination
        if (suddenDeath)
		{
            foreach (Player player in playerList)
			{
                if (player.playerScore < maxScore)
				{
                    if (player.outOfRotation == false)
					{
                        player.outOfRotation = true;
                        player.DisablePlayer();
                        playerCount--;
					}
				}
			}
		}
        // Show end screen if game is over
        if (playerCount == 1)
        {
            playerList.Sort((p1, p2) => p2.playerScore.CompareTo(p1.playerScore));
            switch (playerList[0].playerID)
            {
                case 0:
                    redDinoAvatar.SetActive(true);
                    break;
                case 1:
                    blueDinoAvatar.SetActive(true);
                    break;
                case 2:
                    greenDinoAvatar.SetActive(true);
                    break;
                case 3:
                    yellowDinoAvatar.SetActive(true);
                    break;
            }
            endScreen.SetActive(true);
            Cursor.visible = true;
        }
        // Show scoreboard and then continue game
        else
        {
            // Show scoreboard
            scoreboard.gameObject.SetActive(true);
            // Delay
            yield return new WaitForSeconds(3);
            // Hide scoreboard
            scoreboard.gameObject.SetActive(false);
            // Start new round
            resetInProgress = false;
            StartCoroutine("RoundStart");
        }
    }
    public void PlaySound()
	{
        audioSource.Play();
    }
    public void Rematch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LevelSelect()
	{
        SceneManager.LoadScene(2);
	}
    public void BackToMenu()
    {
        foreach(Player player in playerList)
		{
            Destroy(player.gameObject);
		}
        SceneManager.LoadScene(0);
    }
}