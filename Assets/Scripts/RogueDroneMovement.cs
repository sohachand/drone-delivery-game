using UnityEngine;

public class RogueDroneMovement : MonoBehaviour
{
    public float speed = 5f;
    public float stunDuration = 3f;
    public float twistDistance = 5f;

    public float bobSpeed = 2f;
    public float bobAmount = 0.5f;
    public float minHeight = 1f;
    public float maxHeight = 100f;
    public float verticalChaseSpeed = 3f;

    public float separationRadius = 3f;
    public float separationStrength = 2f;

    public Color stunnedColor = Color.cyan;
    private Color originalColor;
    private Renderer[] renderers;

    private bool isStunned = false;
    public bool isDestroyed = false;
    public GameObject packagePrefab;

    private Transform player;
    private Rigidbody rb;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            originalColor = renderers[0].material.color;
        }

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isDestroyed) return;

        if (isStunned)
        {
            CheckForTwist();
        }
    }

    void FixedUpdate()
    {
        if (isDestroyed || isStunned) return;
        ChasePlayer();
    }

    void ChasePlayer()
    {
        if (player == null) return;

        Vector3 chaseDirection = (player.position - transform.position).normalized;

        Vector3 separation = Vector3.zero;
        RogueDroneMovement[] allDrones = FindObjectsByType<RogueDroneMovement>(FindObjectsSortMode.None);
        foreach (RogueDroneMovement other in allDrones)
        {
            if (other == this || other.isDestroyed) continue;

            float dist = Vector3.Distance(transform.position, other.transform.position);
            if (dist < separationRadius && dist > 0.01f)
            {
                Vector3 away = (transform.position - other.transform.position).normalized;
                separation += away * (separationRadius - dist);
            }
        }

        Vector3 finalDirection = (chaseDirection + separation * separationStrength).normalized;
        finalDirection.y = 0;

        if (finalDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 5f * Time.fixedDeltaTime));
        }

        Vector3 moveStep = transform.forward * speed * Time.fixedDeltaTime;

        float desiredHeight = Mathf.Clamp(player.position.y, minHeight, maxHeight);
        float bob = Mathf.Sin(Time.time * bobSpeed) * bobAmount;

        Vector3 newPos = rb.position + moveStep;
        newPos.y = Mathf.Lerp(rb.position.y, desiredHeight + bob, verticalChaseSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPos);
    }

    void CheckForTwist()
    {
        Debug.Log(gameObject.name + " checking for twist, isStunned=" + isStunned);

        RogueDroneMovement[] allDrones = FindObjectsByType<RogueDroneMovement>(FindObjectsSortMode.None);

        foreach (RogueDroneMovement other in allDrones)
        {
            if (other == this || other.isDestroyed) continue;

            if (other.IsStunned())
            {
                float dist = Vector3.Distance(transform.position, other.transform.position);
                Debug.Log("Distance to " + other.gameObject.name + " = " + dist + " (need <= " + twistDistance + ")");

                if (dist <= twistDistance)
                {
                    Debug.Log("TWIST TRIGGERED between " + gameObject.name + " and " + other.gameObject.name);

                    this.isDestroyed = true;
                    other.isDestroyed = true;

                    Instantiate(packagePrefab, transform.position, Quaternion.identity);

                    Destroy(this.gameObject);
                    Destroy(other.gameObject);
                    return;
                }
            }
        }
    }

    public void Stun()
    {
        if (!isStunned)
        {
            Debug.Log(gameObject.name + " has been stunned!");
            StartCoroutine(StunRoutine());
        }
    }

    private System.Collections.IEnumerator StunRoutine()
    {
        isStunned = true;

        float flashTimer = 0f;
        while (flashTimer < stunDuration)
        {
            bool useFlash = Mathf.FloorToInt(flashTimer * 6f) % 2 == 0;
            SetColor(useFlash ? stunnedColor : originalColor);

            transform.Rotate(Vector3.up, 200f * Time.deltaTime);

            flashTimer += Time.deltaTime;
            yield return null;
        }

        SetColor(originalColor);
        isStunned = false;
    }

    void SetColor(Color c)
    {
        foreach (Renderer r in renderers)
        {
            r.material.color = c;
        }
    }

    public bool IsStunned()
    {
        return isStunned;
    }
}