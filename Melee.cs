using UnityEngine;
public class Melee : MonoBehaviour
{
	[SerializeField] Player player;
	[SerializeField] AudioSource parrySound;
	float coolDownTime;
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("MeleeSwing"))
		{
			parrySound.Play();
			coolDownTime = Time.time + 1.25f; 
		}
		if(other.CompareTag("Player"))
		{
			Player otherPlayer = other.GetComponent<Player>();
			if(otherPlayer.meleeImmune == false && Time.time > coolDownTime)
			{
				if (otherPlayer.playerID != player.playerID)
				{
					otherPlayer.OnPlayerDeath(player.playerID);
					player.meleePauseDuration = 0.25f;
				}
			}
		}
		if(other.CompareTag("Arrow"))
		{
			Arrow arrow = other.GetComponent<Arrow>();
			arrow.ArrowReflect(transform.position + transform.up * 10f, player.playerID);
		}
	}
}