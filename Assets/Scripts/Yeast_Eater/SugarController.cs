﻿using System.Collections;
using UnityEngine;

public class SugarController : MonoBehaviour
{
    YeastGameController controller;

    void Start()
    {
        controller = GameObject.Find("YeastGameController").GetComponent<YeastGameController>();
    }

    public void Shrink()
    {
        GetComponent<BoxCollider>().enabled = false;
        controller.game_score++;

        StartCoroutine(ShrinkCoroutine());
    }

    IEnumerator ShrinkCoroutine()
    {
        while (transform.localScale.x > 0.1)
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, transform.localScale.z - 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(this.gameObject);
    }
}
