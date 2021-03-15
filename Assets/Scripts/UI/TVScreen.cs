using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVScreen : MonoBehaviour
{
    public MeshRenderer m_render;
    
    [SerializeField]
    public  List<Material> m_counter;
    [SerializeField]
    public List<Material> m_display;

    float m_counterTime = -1.5f;
    int lenDisplay = 0;
    public int indexDisplay = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_render = GetComponent<MeshRenderer>();
        lenDisplay = m_display.Count;
        indexDisplay = Random.Range(0, lenDisplay);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Display();      // decide what to be showed on TV screen
    }

    void Display()
    {
        m_counterTime += Time.deltaTime;
        if (m_counterTime <= 5.75f && m_counterTime >= 5f)
        {
            m_render.material = m_counter[0];   // 5s
        }
        else if (m_counterTime <= 6.75f && m_counterTime >= 6f)
        {
            m_render.material = m_counter[1];   // 4s
        }
        else if (m_counterTime <= 7.75f && m_counterTime >= 7f)
        {
            m_render.material = m_counter[2];   // 3s
        }
        else if (m_counterTime <= 8.75f && m_counterTime >= 8f)
        {
            m_render.material = m_counter[3];   // 2s
        }
        else if (m_counterTime <= 9.75f && m_counterTime >= 9f)
        {
            m_render.material = m_counter[4];   // 1s
        }
        else if (m_counterTime < 10.0f)     // final 0.25f to decide the index
        {
            indexDisplay = Random.Range(0, lenDisplay - 1);
            m_render.material = m_counter[5];
        }
        else if (m_counterTime >= 10.0f && m_counterTime <= 20.0f)
        {
            m_render.material = m_display[indexDisplay];
        }
        else if (m_counterTime >= 20.0f)
        {
            m_render.material = m_counter[5];
            m_counterTime = -1.5f;
        }
        else
        {
            m_render.material = m_counter[5];
        }
    }
}
