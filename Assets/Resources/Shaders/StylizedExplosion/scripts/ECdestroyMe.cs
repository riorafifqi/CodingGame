using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;

public class ECdestroyMe : MonoBehaviour{

    float timer;
    public float deathtimer = 10;
	public GameObject smoke;

	// Use this for initialization
	void Awake() {
		//StartCoroutine(FindObjectOfType<CameraController>().ShakeCam());
		smoke.SetActive(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

		float tempSize = Mathf.Lerp(0.3f, 0, timer / 5);

		VisualEffect smokevfx = smoke.GetComponent<VisualEffect>();
		smokevfx.SetFloat("Size", tempSize);


		if (timer >= deathtimer)
        {
            Destroy(gameObject);
        }
	
	}
}
