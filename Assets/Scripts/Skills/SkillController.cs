﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    public Animator animator;

    private float speed = 12f;

    private Vector3 startPos;

    private static GameObject[] rina;

    private static GameObject[] spell = new GameObject[2];

    public static bool[] activateSpellBool = { false, false };
    static bool continueBool = false;
    static bool[] right = { true, true };
    IEnumerator enumerator1 = null;
    IEnumerator enumerator2 = null;

    // Use this for initialization
    void Start()
    {
        defaultSkills();
    }

    void defaultSkills()
    {
        //Debug.Log("Selecting default spell");
        selectSpell(0, "Light");
        selectSpell(1, "Cosmic");
    }

    public void changeSkills(string spell1, string spell2)
    {
        Destroy(GameObject.Find(spell[0].name));
        Destroy(GameObject.Find(spell[1].name));

        Debug.Log("Changing spell");
        selectSpell(0, spell1);
        selectSpell(1, spell2);
    }

    public void selectSpell(int a, string spellString)
    {
        Debug.Log("Selecting spell: " + spellString);
        rina = GameObject.FindGameObjectsWithTag("Player");
        spell[a] = Instantiate(Resources.Load(spellString)) as GameObject;
        spell[a].transform.SetParent(rina[0].transform);
        spell[a].GetComponentInChildren<ParticleSystemRenderer>().enabled = false;
        spell[a].GetComponentInChildren<ParticleSystemRenderer>().sortingOrder = 5;
        var coll = spell[a].GetComponentInChildren<ParticleSystem>().collision;
        coll.enabled = false;
        spell[a].name = spell[a].name.Replace("(Clone)", "");
        spell[a].transform.position = new Vector3(rina[0].transform.position.x + 1.5f, rina[0].transform.position.y, rina[0].transform.position.z);
    }

    public void resetSpell(string spellString) 
    {
        Debug.Log("Reseting spell: " + spellString);
        for (int a = 0; a < 2; a++)
        {
            if (spell[a] != null && spell[a].name == spellString)
            {
                var coll = spell[a].GetComponentInChildren<ParticleSystem>().collision;
                coll.enabled = false;
                spell[a].transform.SetParent(rina[0].transform);
                activateSpellBool[a] = false;
                if(a == 0)
                {
                    if (enumerator1 != null)
                        StopCoroutine(enumerator1);
                }
                else
                {
                    if (enumerator2 != null)
                        StopCoroutine(enumerator2);
                }
                StartCoroutine(resetSpellCoroutine(a));
                animator.SetBool("IsCasting", false);
                break;
            }
        }
    }

    public void activateSpell(string spellString) 
    {
        Debug.Log("Activating spell");
        animator.SetBool("IsCasting", true);
        for (int a = 0; a < 2; a++)
        {
            if (spell[a] != null && spell[a].name == spellString)
            {
                SoundManager.PlaySound("Skill");
                var coll = spell[a].GetComponentInChildren<ParticleSystem>().collision;
                coll.enabled = true;
                right[a] = CharacterController2D.m_FacingRight;
                if (right[a])
                    spell[a].transform.position = new Vector3(rina[0].transform.position.x + 1.5f, rina[0].transform.position.y, rina[0].transform.position.z);
                else
                    spell[a].transform.position = new Vector3(rina[0].transform.position.x - 1.5f, rina[0].transform.position.y, rina[0].transform.position.z);
                spell[a].transform.parent = null;
                activateSpellBool[a] = true;
                if (a == 0)
                {
                    enumerator1 = resetSpellCoroutineWithTime(spellString);
                    StartCoroutine(enumerator1);
                }
                else
                {
                    enumerator2 = resetSpellCoroutineWithTime(spellString);
                    StartCoroutine(enumerator2);
                }
                
                StartCoroutine(activateSpellCoroutine(a));
            }
        }
    }

    IEnumerator activateSpellCoroutine(int a)
    {
        Debug.Log("activateSpellCoroutine");
        while (activateSpellBool[a])
        {
            yield return null;
            spell[a].GetComponentInChildren<ParticleSystemRenderer>().enabled = true;
            if (right[a])
                spell[a].transform.Translate(speed * Time.deltaTime, 0, 0);
            else
                spell[a].transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        continueBool = true;
        yield return 0;
    }

    IEnumerator resetSpellCoroutine(int a)
    {
        yield return new WaitUntil(() => continueBool == true);
        spell[a].GetComponentInChildren<ParticleSystemRenderer>().enabled = false;
        spell[a].transform.position = new Vector3(rina[0].transform.position.x+1, rina[0].transform.position.y, rina[0].transform.position.z);
    }

    IEnumerator resetSpellCoroutineWithTime(string spellString) // If spell didnt collide with anything for 3 seconds, reset the spell
    {
        Debug.Log("resetSpellCoroutineWithTime:" + spellString);
        yield return new WaitForSeconds(1.5f);
        resetSpell(spellString);
    }
    
}
