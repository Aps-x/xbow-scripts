using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class TeamsGameManager : GameManager
{
    int redTeamScore = 0;
    int blueTeamScore = 0;
    int redTeamRoundScore = 0;
    int blueTeamRoundScore = 0;
    public override void Awake()
    {
        // Find players and then add them to the list of players
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerList.Add(player.GetComponent<Player>());
        }
        // Dependency Injection
        foreach (Player player in playerList)
        {
            player.Initialize(this);
        }
        // Scoreboard
        redDinoScoreCard.SetActive(true);
        blueDinoScoreCard.SetActive(true);
        greenDinoScoreCard.SetActive(false);
        yellowDinoScoreCard.SetActive(false);
        // Startup
        StartCoroutine("RoundStart");
    }
    IEnumerator RoundStart()
    {
        // Reset round score tracking
        redTeamRoundScore = 0;
        blueTeamRoundScore = 0;
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
    public override void PlayerKnockout(int killerID, int playerID)
    {
        // Update score
        switch (killerID)
        {
            case 0:
                // Red Team scored a point
                redTeamScore++;
                redTeamRoundScore++;
                for (int i = 0; i < redTeamScore; i++)
                {
                    if (i < redDinoPoints.Length)
                    {
                        redDinoPoints[i].sprite = redEgg;
                    }
                    else
                    {
                        overflowCounterRed = redTeamScore - 10;
                        overflowTextRed.text = "+" + overflowCounterRed.ToString();
                    }
                }
                break;
            case 1:
                // Blue Team scored a point
                blueTeamScore++;
                blueTeamRoundScore++;
                for (int i = 0; i < blueTeamScore; i++)
                {
                    if (i < blueDinoPoints.Length)
                    {
                        blueDinoPoints[i].sprite = blueEgg;
                    }
                    else
                    {
                        overflowCounterBlue = blueTeamScore - 10;
                        overflowTextBlue.text = "+" + overflowCounterBlue.ToString();
                    }
                }
                break;
            case 4:
                // Player died to trap
                switch(playerID)
				{
                    case 0:
                        blueTeamRoundScore++;
                        break;
                    case 1:
                        redTeamRoundScore++;
                        break;
				}
                break;
        }
        // Check if game is over
        if (redTeamRoundScore == 2 || blueTeamRoundScore == 2 && !resetInProgress)
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
        if (redTeamScore >= maxScore || blueTeamScore >= maxScore)
		{
            if (redTeamScore > blueTeamScore)
			{
                redDinoAvatar.SetActive(true);
                endScreen.SetActive(true);
                Cursor.visible = true;
            }
            else if (blueTeamScore > redTeamScore)
			{
                blueDinoAvatar.SetActive(true);
                endScreen.SetActive(true);
                Cursor.visible = true;
            }
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
}