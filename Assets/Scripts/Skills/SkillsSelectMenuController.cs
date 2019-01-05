﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsSelectMenuController : MonoBehaviour {


    private Animator animator;

    private List<GameObject> selectedSkills;

    private Color32 oldColor = new Color32(0,252,255,255);
    private Color32 newColor = new Color32(255, 91, 0, 255);
    private Color32 selectedColor = new Color32(251, 255, 0, 255);


    // Use this for initialization
    void Start () {

        animator = GameObject.Find("SkillsMenu").GetComponent<Animator>();
        selectedSkills = new List<GameObject>();

        for(int i=0; i< AllSpellsController.allSpells.Length; i++)
        {
            if (AllSpellsController.allSpells[i] == false)
            {
                GameObject.Find("Skill" + (i+1)).GetComponent<Image>().color = newColor;
            }
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void skillButtonClicked(GameObject gameObject)
    {
        
        Image SkillImage = gameObject.transform.Find("Mask").GetChild(0).GetComponent<Image>();

        checkTheList(gameObject);
    }
    
    public void AcceptButtonClicked() {

        Image selected1 = GameObject.Find("selectedSkillImage1").GetComponent<Image>();
        Image selected2 = GameObject.Find("selectedSkillImage2").GetComponent<Image>();

        if (selectedSkills.Count == 2)
        {
            selected1.transform.parent.parent.name = selectedSkills[0].transform.Find("Mask").GetChild(0).GetComponent<Image>().name;
            selected1.transform.parent.parent.tag = selectedSkills[0].transform.Find("Mask").GetChild(0).GetComponent<Image>().name;
            selected2.transform.parent.parent.name = selectedSkills[1].transform.Find("Mask").GetChild(0).GetComponent<Image>().name;
            selected2.transform.parent.parent.tag = selectedSkills[1].transform.Find("Mask").GetChild(0).GetComponent<Image>().name;
            GameObject.FindGameObjectWithTag("Player").GetComponent<SkillController>().changeSkills(selected1.transform.parent.parent.name, selected2.transform.parent.parent.name);

            selected1.sprite = selectedSkills[0].transform.Find("Mask").GetChild(0).GetComponent<Image>().sprite;
            selected2.sprite = selectedSkills[1].transform.Find("Mask").GetChild(0).GetComponent<Image>().sprite;
        }

        closeSkillMenu();
        
    }

    public void closeSkillMenu()
    {
        animator.SetBool("isOpen", false);
    }

    public void checkTheList(GameObject gameObject)
    {
        string st = gameObject.transform.parent.name;
        int a = int.Parse(st.Substring(st.Length - 1, 1));

        if (AllSpellsController.allSpells[a - 1]) { 

            if (!selectedSkills.Contains(gameObject))
            {
                selectedSkills.Add(gameObject);
                gameObject.GetComponent<Image>().color = selectedColor;

                if(selectedSkills.Count > 2)
                {
                    selectedSkills[0].GetComponent<Image>().color = oldColor;
                    selectedSkills.Remove(selectedSkills[0]);
                }
            
            }
            else
            {
                selectedSkills.Remove(gameObject);
                gameObject.GetComponent<Image>().color = oldColor;


            }
        }
    }
}
