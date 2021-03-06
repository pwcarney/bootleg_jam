﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public bool debug = false;
    public int current_difficulty = 0;

    [HideInInspector]
    public float main_time = 0f;

    [HideInInspector]
    public bool sanitization_done, corn_done, yeast_done, boil_done, results_done, town_done;

    [HideInInspector]
    public int sanitization_score, corn_score, yeast_score, boil_score, amount_produced, total_amount;

    [HideInInspector]
    public bool sanitization_upgrade, corn_upgrade, yeast_upgrade, boil_upgrade;

    public GameObject barn;
    public GameObject main_camera;
    public GameObject title_screen;
    public GameObject exposition_screen;
    public GameObject results_screen;
    public GameObject event_system;
    public ResultsController results_controller;

    AudioSource music_source;
    public AudioSource cricket_source;

    void ResetFields()
    {
        sanitization_done = false;
        corn_done = false;
        yeast_done = false;
        boil_done = false;
        results_done = false;
        town_done = false;

        sanitization_score = 0;
        corn_score = 0;
        yeast_score = 0;
        boil_score = 0;
        amount_produced = 0;
    }

    void Start()
    {
        music_source = GetComponent<AudioSource>();
    }

    void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        yield return new WaitForSeconds(GetComponent<Fading>().BeginFade(1));

        total_amount = 0;

        event_system.SetActive(false);
        title_screen.SetActive(false);
        main_camera.transform.position = new Vector3(1.17f, 2.21f, 1.71f);
        main_camera.transform.Rotate(new Vector3(15f, 0f, 0f));

        sanitization_upgrade = false;
        corn_upgrade = false;
        yeast_upgrade = false;
        boil_upgrade = false;

        if (!debug)
        {
            exposition_screen.SetActive(true);
        }
        else
        {
            RunNext();
        }
    }

    public void RunNext()
    {
        if (!sanitization_done)
        {
            cricket_source.Stop();

            ResetFields();
            barn.SetActive(false);
            current_difficulty++;
            Debug.Log("Day " + current_difficulty.ToString());

            if (SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync("Town");
            }
            GetComponent<Fading>().enabled = false;
            SceneManager.LoadScene("Sanitization", LoadSceneMode.Additive);
            music_source.Play();
            GetComponent<AudioListener>().enabled = false;
            exposition_screen.SetActive(false);
        }
        else if (!corn_done)
        {
            SceneManager.LoadScene("Corn_Sort", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Sanitization");
        }
        else if (!yeast_done)
        {
            SceneManager.LoadScene("Yeast_Eater", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Corn_Sort");
        }
        else if (!boil_done)
        {
            SceneManager.LoadScene("Boil", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Yeast_Eater");
        }
        else if (!results_done)
        {
            SceneManager.UnloadSceneAsync("Boil");

            barn.SetActive(true);
            GetComponent<AudioListener>().enabled = true;
            event_system.SetActive(true);
            results_screen.SetActive(true);
            amount_produced = (int)((sanitization_score + corn_score + yeast_score + boil_score) / 13.3333333333333333333f);
            results_controller.ExecuteResults();
            total_amount += amount_produced;
            results_done = true;
        }
        else if (!town_done)
        {
            barn.SetActive(false);
            GetComponent<AudioListener>().enabled = false;
            event_system.SetActive(false);
            results_screen.SetActive(false);

            music_source.Stop();

            SceneManager.LoadScene("Town", LoadSceneMode.Additive);
        }
        else
        {
            ResetFields();
            RunNext();
        }
    }

    void Update()
    {
        main_time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

public enum Upgrade
{
    Sanitization,
    Sort,
    Yeast,
    Boil,
};