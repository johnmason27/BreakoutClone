using UnityEngine;

public class Player : MonoBehaviour 
{
    private new Rigidbody rigidbody;

    private void Start() 
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate() 
    {
        this.rigidbody.MovePosition(new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x, -17, 0)); // Specify the Z as 50 because the camera is 50 on Z, Keep the same distance away from object being controlled
    }
}
