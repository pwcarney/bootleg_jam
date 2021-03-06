﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerTownController : MonoBehaviour
{
    MainController main_controller;
    public TownsPersonController text_controller;
    public GameObject win_game_screen;
    public GameObject lose_game_screen;

    Vector3 dest;

    AudioSource player_source;
    public AudioClip coin_clip;

    public AudioSource town_camera;
    public AudioClip win_clip;

    public Button food_button;
    public Button upgrade1;
    public Button upgrade2;
    public Button upgrade3;
    public Button upgrade4;
    public Button continue_button;
    public Text month_text;
    public Text gallons_text;

    bool fed = false;
    int food_cost = 10;
    int sanitization_cost = 50;
    int corn_cost = 50;
    int yeast_cost = 50;
    int boil_cost = 50;

	void Start ()
    {
        dest = new Vector3(-1.74f, 0f, -7.32f);

        if (SceneManager.sceneCount > 1)
        {
            GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
            for (int i = 0; i < main_objects.Length; i++)
            {
                if (main_objects[i].name == "MainController")
                {
                    main_controller = main_objects[i].GetComponent<MainController>();

                    main_controller.gameObject.GetComponent<AudioListener>().enabled = false;
                    main_controller.event_system.SetActive(false);
                    main_controller.results_screen.SetActive(false);
                }
            }
        }

        player_source = GetComponent<AudioSource>();

        if (main_controller.current_difficulty == 10)
        {
            town_camera.clip = win_clip;
            town_camera.Play();
        }

        gallons_text.text = "Gallons of Booze:\n" + main_controller.total_amount;
        month_text.text = "Month " + main_controller.current_difficulty;
    }

    public void SanitizationUpgrade()
    {
        if (main_controller.total_amount >= sanitization_cost && !main_controller.sanitization_upgrade)
        {
            AnimateNumber(main_controller.total_amount, main_controller.total_amount - sanitization_cost);
            main_controller.total_amount -= sanitization_cost;
            main_controller.sanitization_upgrade = true;
        }
    }

    public void CornUpgrade()
    {
        if (main_controller.total_amount >= corn_cost && !main_controller.corn_upgrade)
        {
            AnimateNumber(main_controller.total_amount, main_controller.total_amount - corn_cost);
            main_controller.total_amount -= corn_cost;
            main_controller.corn_upgrade = true;
        }
    }

    public void YeastUpgrade()
    {
        if (main_controller.total_amount >= yeast_cost && !main_controller.yeast_upgrade)
        {
            AnimateNumber(main_controller.total_amount, main_controller.total_amount - yeast_cost);
            main_controller.total_amount -= yeast_cost;
            main_controller.yeast_upgrade = true;
        }
    }

    public void BoilUpgrade()
    {
        if (main_controller.total_amount >= boil_cost && !main_controller.boil_upgrade)
        {
            AnimateNumber(main_controller.total_amount, main_controller.total_amount - boil_cost);
            main_controller.total_amount -= boil_cost;
            main_controller.boil_upgrade = true;
        }
    }

    public void Continue()
    {
        // Check win and lose conditions
        if (!fed)
        {
            town_camera.Stop();
            player_source.Stop();
            lose_game_screen.SetActive(true);
            return;
        }
        else if (fed && main_controller.current_difficulty == 10)
        {
            player_source.Stop();
            win_game_screen.SetActive(true);
            return;
        }

        main_controller.town_done = true;
        main_controller.RunNext();
    }

    public void BuyFood()
    {
        if (main_controller.total_amount > food_cost && !fed)
        {
            AnimateNumber(main_controller.total_amount, main_controller.total_amount - food_cost);
            main_controller.total_amount -= food_cost;
            fed = true;
        }
        else if (!fed)
        {
            text_controller.AnimateText("I can't afford food this month. Not looking forward to watching my family slowly starve to death.");
        }
    }

    void AnimateNumber(int previous_amount, int new_amount)
    {
        StartCoroutine(AnimateNumberRoutine(previous_amount, new_amount));
    }

    IEnumerator AnimateNumberRoutine(int previous_amount, int new_amount)
    {
        int i = previous_amount;
        while (i > new_amount)
        {
            player_source.clip = coin_clip;
            player_source.Play();

            i--;
            gallons_text.text = "Gallons of Booze:\n" + i.ToString();
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Update()
    {
        SetButtonsInteractible();
        
        if (!GetComponentInChildren<Animation>().isPlaying)
        {
            GetComponentInChildren<Animation>().Play();
        }

        // Rotate to face destination
        if (Vector3.Distance(dest, transform.position) > 1)
        {
            transform.rotation = Quaternion.LookRotation(dest - transform.position, new Vector3(0, 1, 0));
        }

        // Player travel
        transform.position = Vector3.MoveTowards(transform.position, dest, 1f * Time.deltaTime);
    }

    void SetButtonsInteractible()
    {
        // Food button greyed out if food bought
        if (fed)
        {
            food_button.interactable = false;
        }
        else
        {
            food_button.interactable = true;
        }

        // Continue button greyed out if haven't bought food and can.
        if (main_controller.total_amount >= food_cost && !fed)
        {
            continue_button.interactable = false;
        }
        else
        {
            continue_button.interactable = true;
        }

        if ((main_controller.total_amount < (food_cost + sanitization_cost) && !fed) || (main_controller.total_amount < sanitization_cost) || (main_controller.sanitization_upgrade))
        {
            upgrade1.interactable = false;
        }
        else
        {
            upgrade1.interactable = true;
        }

        if ((main_controller.total_amount < (food_cost + sanitization_cost) && !fed) || (main_controller.total_amount < sanitization_cost) || (main_controller.corn_upgrade))
        {
            upgrade2.interactable = false;
        }
        else
        {
            upgrade2.interactable = true;
        }

        if ((main_controller.total_amount < (food_cost + sanitization_cost) && !fed) || (main_controller.total_amount < sanitization_cost) || (main_controller.yeast_upgrade))
        {
            upgrade3.interactable = false;
        }
        else
        {
            upgrade3.interactable = true;
        }

        if ((main_controller.total_amount < (food_cost + sanitization_cost) && !fed) || (main_controller.total_amount < sanitization_cost) || (main_controller.boil_upgrade))
        {
            upgrade4.interactable = false;
        }
        else
        {
            upgrade4.interactable = true;
        }
    }

    void QuitGame()
    {
        main_controller.QuitGame();
    }
}
