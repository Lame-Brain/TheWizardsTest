using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlickerController : MonoBehaviour
{
    private Light me;
    private Vector3 here;
    private float deltaX, deltaY, timer, brightness;
    public float FlickerSpeed;
    public float x, y, z;

    // Start is called before the first frame update
    void Start()
    {
        me = this.GetComponent<Light>();
        here = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        brightness = me.intensity;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer++;
        if(timer > FlickerSpeed)
        {
            timer = 0;
            this.transform.position = new Vector3(here.x + Random.Range(-.05f, .05f), here.y, here.z + Random.Range(-.005f, .005f));
            me.intensity = brightness + Random.Range(0, .25f);
            x = here.x;
            y = here.y;
            z = here.z;
        }
    }
}
