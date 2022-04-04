using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneHandler : MonoBehaviour
{
    public bool playOnStart = true;
    public bool isEnd;
    public int cutsceneIndex;
    public string[] dialog;

    GameManager gm;
    SpriteRenderer spr;
    CameraFollow cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraFollow>();
        gm = FindObjectOfType<GameManager>();
        spr = GetComponentInChildren<SpriteRenderer>();

        if (playOnStart)
        {
            cam.target = transform;
            StartCoroutine(DisplayCutsceneText());
        }
    }

    private void Update()
    {
        if (!gm.gm_gameSaveData.playedCutscenes[cutsceneIndex])
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            {
                StopAllCoroutines();
                gm.SetCutsceneDialog("");
                LoadNextLevel();
            }
        }
    }

    public void DisplayCutscene()
    {
        StartCoroutine(DisplayCutsceneText());
    }

    public void LoadNextLevel()
    {
        if(isEnd)
            gm.LoadLevel(2);
        else
            gm.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator DisplayCutsceneText()
    {
        if (!isEnd) // TEMP
        {
            if (!gm.gm_gameSaveData.playedCutscenes[cutsceneIndex])
            {
                gm.StopMusic();
                yield return new WaitForSeconds(2);
                spr.color = Color.white;

                for (int i = 0; i < dialog.Length; i++)
                {
                    gm.PlaySFX(gm.gm_gameSfx.uiSfx[Random.Range(3, 6)]);
                    gm.SetCutsceneDialog(InsertLineBreaks(dialog[i]));
                    yield return new WaitForSeconds(2.75f);
                    gm.SetCutsceneDialog("");
                    yield return new WaitForSeconds(0.5f);
                }

                //gm.gm_gameSaveData.playedCutscenes[cutsceneIndex] = true;
            }
        }

        while (gm.gm_gameVars.isLoadingLevel)
            yield return null;

        LoadNextLevel();
    }

    string InsertLineBreaks(string s)
    {
        string t = "";

        for (int i = 0; i < s.Length; i++)
        {
            // Add line break
            if (s[i] == '\\')
                t += "\n";
            else
                t += s[i];
        }
        return t;
    }
}