using UnityEngine;

public class FloorTransparencyManager : MonoBehaviour
{
    public Material floor;
    public Material floorTransparent;
    Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (transform.position.y > 0)
            renderer.material = floorTransparent;
        else
            renderer.material = floor;
    }
}
