using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lenght, startpos, startposy;
    public GameObject cam;
    public float parallaxEffect;
    void Start()
    {
        startpos = transform.position.x;
        startposy = transform.position.y;

        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update() {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + distance, startposy, transform.position.z);

        if (temp > startpos + lenght) 
            { startpos += lenght;}
        else if (temp < startpos - lenght) {startpos -= lenght;}
    }
}
