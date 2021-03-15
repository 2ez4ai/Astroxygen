using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICamera : MonoBehaviour
{
    [SerializeField]
    Text m_remainingText;

    [SerializeField]
    Text centerText;

    [SerializeField]
    List<GameObject> fallGuys;

    [SerializeField]
    GameObject screen;

    [SerializeField]
    Platform platform;

    List<PlayerLogic> fallGuysScripts = new List<PlayerLogic>();


    int n_fallGuys = 0;
    float m_counterTime = -1.5f;

    // Start is called before the first frame update
    void Start()
    {
        // assume fallGuys[0] is the player
        n_fallGuys = fallGuys.Count;
        if (m_remainingText)
        {
            m_remainingText.text = " Remaining: " + n_fallGuys + "\n Oxygen needs:" + 0;
        }
        for(int i = 0; i < n_fallGuys; i++)
        {
            fallGuysScripts.Add(fallGuys[i].GetComponent<PlayerLogic>());
        }
    }

    void FixedUpdate()
    {
        UpdateLUText();     // keep refreshing the remaining counter
        CenterCount();      // display the counting down
        GameWin();
    }

    void UpdateLUText()
    {
        int cnt = 0;
        for(int i = 0; i < n_fallGuys; i++)
        {
            if (fallGuysScripts[i].m_alive)
            {
                cnt++;
            }
        }
        m_remainingText.text = " Remaining: " + cnt + "\n Oxygen needs:" + fallGuysScripts[0].oxygenCounter;
    }

    void CenterCount()      // 10~15
    {
        m_counterTime += Time.deltaTime;
        if (m_counterTime > 10.0f && m_counterTime < 15.0f)     // count from 10.f
        {
            float temp = 16.0f - m_counterTime;
            centerText.text = (int)temp + "";
        }
        else if (m_counterTime < 20.0f)
        {
            centerText.text = "";
        }
        else if (m_counterTime > 20.0f)
        {
            m_counterTime = -1.5f;
            centerText.text = "";
        }
    }

    void GameWin()
    {
        if (fallGuysScripts[0].oxygenCounter < 1){
            centerText.text = "You Win!";
            m_counterTime = -1.5f;
            Time.timeScale = 0.0f;
            screen.GetComponent<MeshRenderer>().material = screen.GetComponent<TVScreen>().m_counter[7];
        }
    }
}
