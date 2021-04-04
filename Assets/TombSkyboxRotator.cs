using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombSkyboxRotator : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        RenderSettings.skybox.SetFloat("_Rotation", Time.time*3);
    }
}
