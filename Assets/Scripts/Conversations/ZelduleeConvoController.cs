using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZelduleeConvoController : MonoBehaviour
{
    public GameObject ref_Introtext, ref_WhoAreYou_reply, ref_What_Want_reply, ref_What_Realm_reply, ref_What_Happening_reply, ref_What_Artifact_reply, ref_Die_reply;
    public GameObject ref_WhoAreYou, ref_What_Want, ref_What_Realm, ref_What_Happening, ref_What_Artifact, ref_Die;
    public GameObject ref_Zeldulee;
    public GameObject[] ref_Guards;

    private AudioSource[] conversations;
    private AudioSource _as;

    private void Start()
    {
        conversations = new AudioSource[transform.Find("Audio").childCount];
        for (int _i = 0; _i < transform.Find("Audio").childCount; _i++)
            conversations[_i] = transform.Find("Audio").GetChild(_i).GetComponent<AudioSource>();
        _as = transform.Find("Audio").Find("Intro").GetComponent<AudioSource>();
        StartCoroutine(PlayDialogue());
    }

    private void ClearAll()
    {
        ref_Introtext.SetActive(false); ref_WhoAreYou_reply.SetActive(false); ref_What_Want_reply.SetActive(false); ref_What_Realm_reply.SetActive(false); ref_What_Happening_reply.SetActive(false); ref_What_Artifact_reply.SetActive(false); ref_Die_reply.SetActive(false);
        ref_Introtext.SetActive(false); ref_WhoAreYou.SetActive(false); ref_What_Want.SetActive(false); ref_What_Realm.SetActive(false); ref_What_Happening.SetActive(false); ref_What_Artifact.SetActive(false); ref_Die.SetActive(false);
    }
    public void Who_Are_You()
    {
        {
            ClearAll();
            if (_as.isPlaying) _as.Stop();
            _as = transform.Find("Audio").Find("Who_Are_You").GetComponent<AudioSource>();
            StartCoroutine(PlayDialogue());
            ref_WhoAreYou_reply.SetActive(true);
            ref_What_Want.SetActive(true);
            ref_What_Happening.SetActive(true);
            ref_Die.SetActive(true);
        }
    }
    public void What_Do_You_Want()
    {
        {
            ClearAll();
            if (_as.isPlaying) _as.Stop();
            _as = transform.Find("Audio").Find("What_do_You_want").GetComponent<AudioSource>();
            StartCoroutine(PlayDialogue());
            ref_What_Want_reply.SetActive(true);
            ref_WhoAreYou.SetActive(true);
            ref_What_Realm.SetActive(true);
            ref_Die.SetActive(true);
        }
    }
    public void What_Realm()
    {
        {
            ClearAll();
            if (_as.isPlaying) _as.Stop();
            _as = transform.Find("Audio").Find("What_Realm").GetComponent<AudioSource>();
            StartCoroutine(PlayDialogue());
            ref_What_Realm_reply.SetActive(true);
            ref_WhoAreYou.SetActive(true);
            ref_What_Happening.SetActive(true);
            ref_Die.SetActive(true);
        }
    }
    public void What_Is_Going_On()
    {
        {
            ClearAll();
            if (_as.isPlaying) _as.Stop();
            StartCoroutine(What_Happening_Expos());
        }
    }
    public void What_is_Artifact()
    {
        {
            ClearAll();
            if (_as.isPlaying) _as.Stop();
            StartCoroutine(ArtifactExpos());
        }
    }
    public void Die_Foul_Fiend()
    {
        {
            ClearAll();
            if (_as.isPlaying) _as.Stop();
            ref_Die_reply.SetActive(true);

            if (_as.isPlaying) _as.Stop();
            _as = transform.Find("Audio").Find("die_foul_fiend").GetComponent<AudioSource>();
            StartCoroutine(StartFight());
        }
    }

    IEnumerator PlayDialogue()
    {
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + .5f));
    }

    IEnumerator What_Happening_Expos()
    {
        AudioSource _as; float wait = .5f;
        ref_What_Happening_reply.SetActive(true);
        ref_What_Happening_reply.GetComponent<Text>().text = "Is this the part where I divulge my machinations to you? Monologue the whole ''Evil Plot''? ";
        _as = transform.Find("Audio").Find("What_is_going_on1").GetComponent<AudioSource>();
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + wait));

        ref_What_Happening_reply.GetComponent<Text>().text = "Very well...\nI have revealed a certain artifact to Melech, the mortal wizard who lives above my realm.It has ensnared his mind and he will do anything to possess it... ";
        _as = transform.Find("Audio").Find("What_is_going_on2").GetComponent<AudioSource>();
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + wait));

        ref_What_Happening_reply.GetComponent<Text>().text = "but the artifact belongs to that buffoon ''Tromuu, the Great Serpent'' who cannot actually DO anything with it. Fool.";
        _as = transform.Find("Audio").Find("What_is_going_on3").GetComponent<AudioSource>();
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + wait));

        ref_What_Happening_reply.GetComponent<Text>().text = "Anyway, because of certain... obligations of magic, Melech cannot actually TAKE the artifact... he has to have someone get it for him. So he sends adventurers!";
        _as = transform.Find("Audio").Find("What_is_going_on4").GetComponent<AudioSource>();
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + wait));

        ref_What_Happening_reply.GetComponent<Text>().text = "Meanwhile, Tromuu, who is greatly concerned about someone trying to steal the artifact, has manifested here, in my Realm! And because of my influence, he also cannot simply take it!";
        _as = transform.Find("Audio").Find("What_is_going_on5").GetComponent<AudioSource>();
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + wait));

        ref_What_Happening_reply.GetComponent<Text>().text = "Do you see? The wonderful conflict this creates? How I have so perfectly set into motion a machine that sends a constant flow of souls down to me in the form of adventurers... like you?";
        _as = transform.Find("Audio").Find("What_is_going_on6").GetComponent<AudioSource>();
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + wait));

        ref_What_Happening_reply.GetComponent<Text>().text = "HA HA HA!\nYou are not the first to come here, and you will not be the last. You are DOOMED!\nHA HA HAAA!";
        _as = transform.Find("Audio").Find("What_is_going_on7").GetComponent<AudioSource>();
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + wait));

        ref_What_Realm.SetActive(true);
        ref_What_Artifact.SetActive(true);
        ref_Die.SetActive(true);
    }

    IEnumerator ArtifactExpos()
    {
        AudioSource _as; float wait = .5f;
        ref_What_Artifact_reply.SetActive(true);
        ref_What_Happening_reply.GetComponent<Text>().text = "The artifact? Why it is the Serpent's Totem of course! Without it, Tromuu will never be whole,";
        _as = transform.Find("Audio").Find("What_is_the_Artifact1").GetComponent<AudioSource>();
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + wait));

        ref_What_Happening_reply.GetComponent<Text>().text = "but with it Melech takes a step further along the road to becoming more like me! ";
        _as = transform.Find("Audio").Find("What_is_the_Artifact2").GetComponent<AudioSource>();
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + wait));

        ref_What_Happening_reply.GetComponent<Text>().text = "It is just there, behind me... as if that could possibly matter now...";
        _as = transform.Find("Audio").Find("What_is_the_Artifact3").GetComponent<AudioSource>();
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + wait));
        ref_WhoAreYou_reply.SetActive(true);
        ref_What_Happening.SetActive(true);
        ref_Die.SetActive(true);
    }

    IEnumerator StartFight()
    {
        _as.Play();
        yield return new WaitForSeconds((_as.clip.length + .5f));

        GameManager.EXPLORE.OpenBattleScreen();
        GameManager.EXPLORE.current_Battle_Screen.GetComponent<BattleScreenController>().enemy.Add(ref_Zeldulee);
        if (ref_Guards.Length > 0)
        {
            for (int _i = 0; _i < ref_Guards.Length; _i++)
            {
                GameManager.EXPLORE.current_Battle_Screen.GetComponent<BattleScreenController>().enemy.Add(ref_Guards[_i]);
            }
        }

        Destroy(gameObject);
    }
}
