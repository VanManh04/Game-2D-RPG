using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;
    private float xPosition;

    private float length;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    void Update()
    {
        float distabceToMove = cam.transform.position.x * (1 - parallaxEffect);
        float distanceMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distabceToMove, transform.position.y);

        if (distanceMove > xPosition + length)
            xPosition += length;
        else if(distanceMove < xPosition - length)
            xPosition -= length;
    }
}
