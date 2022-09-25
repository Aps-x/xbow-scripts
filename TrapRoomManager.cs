using UnityEngine;
public class TrapRoomManager : MonoBehaviour
{
    [SerializeField] Animator campfire;
    [SerializeField] SpriteRenderer targetSR;
    [SerializeField] Sprite targetActive;
    [SerializeField] Sprite targetInactive;
    [SerializeField] Animator[] fireTrapsAnimators;
    [SerializeField] Trap[] fireTrapsScripts;
    [SerializeField] GameObject campFireLight;
    float timer;
    float nextActivation;
    float activationRate = 30;
    bool trapReady;
    void Start()
    {
        timer = Time.time;
        nextActivation = timer + activationRate;
    }
    void Update()
    {
        if (!trapReady)
		{
            timer += Time.deltaTime;
        }
        if (timer >= nextActivation && !trapReady)
		{
            trapReady = true;
            targetSR.sprite = targetActive;
            campfire.SetBool("IsBurning", true);
            campFireLight.SetActive(true);
            for (int i = 0; i < fireTrapsAnimators.Length; i++)
            {
                fireTrapsAnimators[i].SetBool("FireTrapReady", true);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arrow") && trapReady)
        {
            nextActivation = timer + activationRate;
            trapReady = false;
            targetSR.sprite = targetInactive;
            int playerIDCopy = other.GetComponent<Arrow>().playerIDCopy;
			for (int i = 0; i < fireTrapsScripts.Length; i++)
			{
                fireTrapsScripts[i].trapID = playerIDCopy;
            }
            for (int i = 0; i < fireTrapsAnimators.Length; i++)
            {
                fireTrapsAnimators[i].SetBool("FireTrapReady", false);
                fireTrapsAnimators[i].SetBool("FireTrapActive", true);
            }
            Invoke("ResetCampfire", 2.5f);
        }
    }
    void ResetCampfire()
	{
        campfire.SetBool("IsBurning", false);
        campFireLight.SetActive(false);
    }
}