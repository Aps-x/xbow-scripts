using UnityEngine;
public class Trap : MonoBehaviour
{
	[SerializeField] BoxCollider2D hitbox;
	[SerializeField] Animator animator;
	[SerializeField] GameObject flashlight;
	public int trapID = 4;
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (other.GetComponent<Player>().playerID != trapID)
			{
				other.GetComponent<Player>().OnPlayerDeath(trapID);
			}
		}
	}
	public void EnableDamage()
	{
		hitbox.enabled = true;
		flashlight.SetActive(true);
	}
	public void DisableDamage()
	{
		hitbox.enabled = false;
		flashlight.SetActive(false);
		trapID = 4;
	}
	public void FireTrapReset()
	{
		animator.SetBool("FireTrapActive", false);
	}
}