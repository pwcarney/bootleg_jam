﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TownsPersonController : MonoBehaviour
{
    MainController main_controller;

    public List<int> dialogue_id;
    [HideInInspector]
    public List<Dialogue> dialogue_lines;
    int dialogue_ind = 0;

    float next_talk = 0f;
    float talk_cooldown = 0.5f;
    string slow_string;

    AudioSource source;
    public AudioClip speaking_clip;

    void Start()
    {
        source = GetComponent<AudioSource>();

        if (SceneManager.sceneCount > 1)
        {
            GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
            for (int i = 0; i < main_objects.Length; i++)
            {
                if (main_objects[i].name == "MainController")
                {
                    main_controller = main_objects[i].GetComponent<MainController>();
                }
            }

            switch (main_controller.current_difficulty)
            {
                case 1:
                    dialogue_id.Add(10);
                    break;
                case 2:
                    dialogue_id.Add(11);
                    break;
                case 3:
                    dialogue_id.Add(12);
                    break;
                case 4:
                    dialogue_id.Add(13);
                    break;
                case 5:
                    dialogue_id.Add(14);
                    break;
                case 6:
                    dialogue_id.Add(15);
                    break;
                case 7:
                    dialogue_id.Add(16);
                    break;
                case 8:
                    dialogue_id.Add(17);
                    break;
                case 9:
                    dialogue_id.Add(18);
                    break;
                case 10:
                    dialogue_id.Add(19);
                    break;
            }
        }

        for (int i = 0; i < dialogue_id.Count; i++)
        {
            dialogue_lines.Add(DialogueManager.current.GetDialogue(dialogue_id[i]));
        }

        next_talk = Time.timeSinceLevelLoad + talk_cooldown;
        if (SceneManager.sceneCount > 1)
        {
            AnimateText(dialogue_lines[dialogue_ind].line);
        }
    }

    public void AnimateText(string str)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateTextRoutine(str));
    }

    IEnumerator AnimateTextRoutine(string strComplete)
    {
        slow_string = "";
        int i = 0;
        while (i < strComplete.Length)
        {
            source.clip = speaking_clip;
            source.Play();

            slow_string += strComplete[i++];
            GetComponentInChildren<Text>().text = slow_string;
            yield return new WaitForSeconds(0.03f);
        }
    }

    public void NextText()
    {
        if (dialogue_ind < dialogue_lines.Count)
        {
            dialogue_ind++;
            AnimateText(dialogue_lines[dialogue_ind].line);
        }
    }


}