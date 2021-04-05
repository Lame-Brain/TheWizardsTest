using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombSkyboxRotator : MonoBehaviour
{
    private float timer, flashTime;

    private void Start()
    {
        timer = Random.Range(1f, 100f);
        flashTime = Random.Range(0.3f, 1.5f);
    }
    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time*3);

        timer--;
        if(timer < 0) StartCoroutine(flash());
    }

    IEnumerator flash()
    {
        float amount = Random.Range(0.5f, 2f);
        RenderSettings.skybox.SetFloat("_Exposure", 1 + amount);
        yield return new WaitForSeconds(flashTime);
        RenderSettings.skybox.SetFloat("_Exposure", 1f);
        timer = Random.Range(1f, 100f);
        flashTime = Random.Range(0.3f, 1.5f);
    }
}
