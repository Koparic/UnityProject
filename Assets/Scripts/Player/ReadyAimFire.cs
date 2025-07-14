using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAimFire : MonoBehaviour
{
    public GameObject bullet;
    public GameObject dot;
    public ParticleSystem explosion;
    public bool dotDisplay = false;
    public float dotFreq = 0.1f;
    public static int dotNum = 15;
    public float speed = 25f;
    public float gravity = 9f;
    private GameObject[] dots = new GameObject[dotNum];

    private void Start()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i] = GameObject.Instantiate(dot);
            dots[i].SetActive(dotDisplay);
        }
    }

    private void Update()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(dotDisplay);
        }
    }

    public void Aim()
    {
        float radianAngle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        float vx = speed * Mathf.Cos(radianAngle);
        float vy = speed * Mathf.Sin(radianAngle);
        for (int i = 1; i <= dots.Length; i++)
        {
            dots[i-1].transform.position = transform.position + new Vector3(vx * i * dotFreq, (vy - 0.5f * gravity * i * dotFreq) * i * dotFreq, -2);
        }
    }

    public Transform Fire(Transform player)
    {
        GameObject b = GameObject.Instantiate(bullet);
        b.transform.position = gameObject.transform.position;
        b.transform.rotation = gameObject.transform.rotation;
        Bullet comp = b.AddComponent<Bullet>();
        float radianAngle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        comp.speedX = speed * Mathf.Cos(radianAngle);
        comp.speedY = speed * Mathf.Sin(radianAngle);
        comp.startAngle = transform.rotation.eulerAngles.z;
        comp.gravity = gravity;
        comp.player = player;
        comp.explosion = explosion;
        return b.transform;
    }

}
