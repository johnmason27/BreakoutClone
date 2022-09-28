using UnityEngine;

public class Ball : MonoBehaviour 
{
    private float speed = 20f; // 20 fps
    private new Rigidbody rigidbody;
    private Vector3 velocity;
    private Renderer ballRenderer;

    private void Start() 
    {
        this.rigidbody = GetComponent<Rigidbody>();
        this.ballRenderer = GetComponent<Renderer>();
        this.Invoke(nameof(this.Launch), 0.5f);
    }

    private void Launch()
    {
        this.rigidbody.velocity = Vector3.up * this.speed;
    }

    private void FixedUpdate() 
    {
        this.rigidbody.velocity = this.rigidbody.velocity.normalized * this.speed;
        this.velocity = this.rigidbody.velocity; // Set the volicity of the ball as it updates

        if (!this.ballRenderer.isVisible)
        {
            GameManager.Instance.Balls--;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        this.rigidbody.velocity = Vector3.Reflect(this.velocity, collision.contacts[0].normal); // Perpendicular angle of the ball and reflecting the direction of the ball
    }
}
