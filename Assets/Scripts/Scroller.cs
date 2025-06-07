using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public PlayerController playerController;
    public float Speed;

    private float offset;
    private Material mat;



    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }


    void Update()
    {
        float inertia = playerController.GetInertiaForce();
        float speed = Speed + (-1* inertia * Speed);

        offset += (Time.deltaTime * speed) / 10;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}