using UnityEngine;

public class PlayerEMP : MonoBehaviour
{
    public GameObject empWavePrefab;

    void Update()
    {
        if (Input.GetAxis("Fire1") != 0)
        {
            Instantiate(empWavePrefab, transform.position, Quaternion.identity);
        }
    }
}