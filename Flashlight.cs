using UnityEngine;
public class Flashlight : MonoBehaviour
{
    [Range(0.05f, 0.2f)]
    public float flickTime;

    [Range(0.02f, 0.09f)]
    public float addSize;

    float timer = 0;

    bool alternate = true;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > flickTime)
		{
            if (alternate)
			{
                transform.localScale = new Vector3(transform.localScale.x + addSize, transform.localScale.y + addSize, transform.localScale.z);
            }
			else
			{
                transform.localScale = new Vector3(transform.localScale.x - addSize, transform.localScale.y - addSize, transform.localScale.z);
            }

            timer = 0;
            alternate =! alternate;
        }
    }
}