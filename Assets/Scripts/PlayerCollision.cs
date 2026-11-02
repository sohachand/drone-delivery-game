using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public GameObject sparklePrefab;
    private AudioSource audioSource;
    private int score = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Package"))
        {
            audioSource.Play();

            GameObject sparkle = Instantiate(sparklePrefab, collision.transform.position, Quaternion.identity);
            Destroy(sparkle, 2f);

            Destroy(collision.gameObject);

            score++;
            Debug.Log("Score: " + score);

            if (score >= 10)
            {
                PlayerPrefs.SetInt("FinalScore", score);
                SceneManager.LoadScene("Victory");
                
            }
        }
        else if (collision.gameObject.CompareTag("RogueDrone"))
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}