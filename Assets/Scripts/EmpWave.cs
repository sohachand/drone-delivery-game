using UnityEngine;

public class EMPWave : MonoBehaviour
{
    public float growDuration = 2f;
    public float maxScale = 5f;

    private float timer = 0f;
    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / growDuration;
        transform.localScale = Vector3.Lerp(startScale, Vector3.one * maxScale, t);

        if (timer >= growDuration)
        {
            Destroy(gameObject);
        }
    }

   void OnTriggerEnter(Collider other)
{
    Debug.Log("EMP touched: " + other.gameObject.name);
    
    RogueDroneMovement rogue = other.GetComponentInParent<RogueDroneMovement>();
    if (rogue != null)
    {
        Debug.Log("Found RogueDroneMovement on: " + rogue.gameObject.name);
        rogue.Stun();
    }
    else
    {
        Debug.Log("No RogueDroneMovement found on this object or its parents");
    }
}
}