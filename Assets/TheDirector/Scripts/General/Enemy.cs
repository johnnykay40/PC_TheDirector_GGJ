using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform groundDetected;
    [SerializeField] private float distance;
    [SerializeField] private bool rightMovement;
    [SerializeField] private float infecionRate = 0.1f;
    private Rigidbody2D rigidBody;


    public float InfectionRate => infecionRate;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Patrol();
    }

    private void Patrol()
    {
        RaycastHit2D groundDetection = Physics2D.Raycast(groundDetected.position, Vector2.down, distance);
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

        if (groundDetection == false)
        {
            Rotation();
        }
    }

    private void Rotation() 
    {
        rightMovement = !rightMovement;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        speed *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundDetected.transform.position, groundDetected.transform.position + Vector3.down * speed);
    }
}
