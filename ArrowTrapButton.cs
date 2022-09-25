using UnityEngine;
public class ArrowTrapButton : MonoBehaviour
{
	[SerializeField] Sprite targetActivated;
	[SerializeField] Sprite targetDeactivated;
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] ArrowTrap[] arrowTraps;
	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip reloadSound;
	float timeSinceLastActivation = 0;
	float activationInterval = 5;
	bool canPlaySound = false;
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Arrow") && Time.time > (timeSinceLastActivation + activationInterval))
		{
			timeSinceLastActivation = Time.time;
			int playerIDCopy = other.GetComponent<Arrow>().playerIDCopy;
			for (int i = 0; i < arrowTraps.Length; i++)
			{
				arrowTraps[i].FireArrow(playerIDCopy);
			}
			spriteRenderer.sprite = targetDeactivated;
			canPlaySound = true;
		}
	}
	void Update()
	{
		if (Time.time > (timeSinceLastActivation + activationInterval) && canPlaySound)
		{
			spriteRenderer.sprite = targetActivated;
			canPlaySound = false;
			//audioSource.PlayOneShot(reloadSound);
		}
	}
}