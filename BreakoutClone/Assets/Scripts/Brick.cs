using UnityEngine;

public class Brick : MonoBehaviour 
{
    private new Renderer renderer;
    private Material previousMaterial;
    public int Hits = 2;
    public int Score = 100;
    public Vector3 Rotator;
    public Material HitMaterial;

    private void Start() 
    {
        this.transform.Rotate(this.Rotator * (this.transform.position.x + this.transform.position.y) * 0.1f);
        this.renderer = GetComponent<Renderer>();
        this.previousMaterial = this.renderer.sharedMaterial;
    }

    private void Update() 
    {
        this.transform.Rotate(this.Rotator * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.Hits--;
        if (this.Hits <= 0)
        {
            GameManager.Instance.Score += this.Score;
            Destroy(this.gameObject);
        }
        this.renderer.sharedMaterial = this.HitMaterial;
        this.Invoke(nameof(RestoreMaterial), 0.05f);
    }

    private void RestoreMaterial()
    {
        this.renderer.sharedMaterial = this.previousMaterial;
    }
}
