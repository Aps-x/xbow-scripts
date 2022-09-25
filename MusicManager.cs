using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
	private static MusicManager instance;
	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip musicClip0;
	[SerializeField] AudioClip musicClip1;
	[SerializeField] AudioClip musicClip2;
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
	void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
		{
			if (!audioSource.isPlaying)
			{
				int randumNum = Random.Range(0,3);
				switch (randumNum)
				{
					case 0:
						audioSource.clip = musicClip0;
						break;
					case 1:
						audioSource.clip = musicClip1;
						break;
					case 2:
						audioSource.clip = musicClip2;
						break;
				}
				audioSource.Play();
			}
		}
		else
		{
			if (audioSource.isPlaying)
			{
				audioSource.Stop();
			}
		}
    }
}