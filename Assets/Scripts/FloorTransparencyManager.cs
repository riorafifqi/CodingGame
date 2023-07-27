using System.Collections;
using UnityEngine;

public class FloorTransparencyManager : MonoBehaviour
{
    public Material floor;
    public Material floorTransparent;
    public Color emissionColorInContact;
    public Color emissionColorNoContact;
    public float emissionDropOffDuration = 1.0f;

    private Renderer rendererM;
    private bool inContactWithPlayer;

    void Start()
    {
        rendererM = GetComponent<Renderer>();
        if (transform.position.y > 0)
            rendererM.material = floorTransparent;
        else
            rendererM.material = floor;
    }

    void OnCollisionStay(Collision collision)
    {
        // Assuming the player has a specific tag like "Player".
        if (collision.gameObject.CompareTag("Player"))
        {
            // Update the emission color based on player contact.
            if (!inContactWithPlayer)
            {
                SetEmissionColor(emissionColorInContact * 5);
                inContactWithPlayer = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Assuming the player has a specific tag like "Player".
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(EmissionDropOff());
        }
    }

    void SetEmissionColor(Color color)
    {
        // Create a Material Property Block.
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        // Set the emission color property in the Material Property Block.
        mpb.SetColor("_EmissionColor", color);

        // Apply the Material Property Block to the renderer.
        rendererM.SetPropertyBlock(mpb);
    }

    IEnumerator EmissionDropOff()
    {
        float startTime = Time.time;
        float startIntensity = 5;
        Color startColor = emissionColorInContact * startIntensity;

        while (Time.time < startTime + emissionDropOffDuration)
        {
            float elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / emissionDropOffDuration);

            float currentIntensity = Mathf.Lerp(startIntensity, 0f, t);
            SetEmissionColor(startColor * currentIntensity);

            yield return null;
        }

        SetEmissionColor(emissionColorNoContact * 1.8f);
        inContactWithPlayer = false;
    }
}
