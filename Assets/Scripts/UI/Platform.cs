using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> tiles;
    [SerializeField]
    Transform oxygen;

    public List<MeshRenderer> m_render = new List<MeshRenderer>();
    float m_counterTime = -1.5f;
    List<int> indexTiles = new List<int>();
    List<int> indexCover = new List<int>();

    int lenDisplay = 0;
    int lenTiles = 16;
    bool unhide = true;
    bool oxygenDistributed = true;
    TVScreen screen;

    // Start is called before the first frame update
    void Start()
    {
        GameObject tv = GameObject.Find("Screen");
        screen = tv.GetComponent<TVScreen>();
        lenDisplay = screen.m_display.Count;
        lenTiles = tiles.Count;
        for (int i = 0; i < lenTiles; i++)
        {
            m_render.Add(tiles[i].GetComponent<MeshRenderer>());
            indexTiles.Add(Random.RandomRange(0, lenDisplay - 1));
        }
        InitCoverList();
    }

    // Update is called once per frame
    void Update()
    {
        Display();
    }

    void Display()      // display on tiles
    {
        m_counterTime += Time.deltaTime;
        if (m_counterTime < 10.0f && m_counterTime >= 0.0f)       // memorize for 10.0f
        {
            for (int i = 0; i < 4; i++)
            {
                Uncover(i);
                if (m_counterTime >= 2.5f * i && m_counterTime <= 2.5f * i + 2.0f)
                {
                    Uncover(i);
                    break;
                }
                else
                {
                    Cover();
                }
            }
        }
        else if (m_counterTime < 15.0f)     // move and determine final position for 5.0f
        {
            Cover();
            if (oxygenDistributed && m_counterTime > 0.0f)
            {
                CreateOxygen();
                oxygenDistributed = false;
            }
        }
        else if (m_counterTime < 17.5f)
        {
            Uncover(4);
        }
        else if (m_counterTime < 20.0f && unhide)
        {
            Hide();
            unhide = false;
        }
        else if (m_counterTime > 20.0f)
        {
            m_counterTime = -1.5f;
            unhide = true;
            oxygenDistributed = true;
            Recover();
            for(int i = 0; i < lenTiles; i++)
            {
                indexTiles[i] = Random.RandomRange(0, lenDisplay - 1);
            }
        }
    }

    void Uncover(int type)
    {
        int s = 0;
        int e = 0;
        if (type == 4)      // uncover all
        {
            for (int i = 0; i < lenTiles; i++)
            {
                m_render[i].material = screen.m_display[indexTiles[i]];
            }
            return;
        }
        if (type == 0)
        {
            s = 0;
            e = 5;
        }
        if (type == 1)
        {
            s = 5;
            e = 11;
        }
        if (type == 2)
        {
            s = 11;
            e = 17;
        }
        if (type == 3)
        {
            s = 17;
            e = indexCover.Count;
        }
        for (; s < e; s++)
        {
            m_render[indexCover[s]].material = screen.m_display[indexTiles[indexCover[s]]];
        }

    }

    void InitCoverList()
    {
        for(int i = 0; i < lenTiles; i++)       // init
        {
            indexCover.Add(i);
        }
        for(int j = 0; j < 3; j++)
        {
            for (int i = 0; i < lenTiles; i++)       //shuffle
            {
                int des = Random.RandomRange(0, lenTiles);
                int temp = indexCover[i];
                indexCover[i] = indexCover[des];
                indexCover[des] = temp;
            }
        }
        List<int> extra = new List<int>();
        extra.Add(1);
        extra.Add(2);
        extra.Add(2);
        extra.Add(3);
        int pos = 0;
        for(int e = 0; e < 4; e++)
        {
            for(int i = 0; i < extra[e]; i++)
            {
                indexCover.Insert(pos, Random.RandomRange(0, lenTiles));
            }
            pos += extra[e] + 4;
        }
    }

    public void Cover()
    {
        for (int i = 0; i < lenTiles; i++)
        {
            m_render[i].material = screen.m_display[lenDisplay - 1];
        }
    }

    void Hide()
    {
        int indexer = screen.indexDisplay;
        for (int i = 0; i < lenTiles; i++)
        {
            if(indexTiles[i] != indexer)
            {
                tiles[i].SetActive(false);
            }
        }
    }

    void Recover()
    {
        for (int i = 0; i < lenTiles; i++)
        {
            tiles[i].SetActive(true);
        }
    }

    void CreateOxygen()
    {
        int n = Random.RandomRange(1, 4);
        for(int i =0; i < n; i++)
        {
            int temp = Random.RandomRange(0, lenTiles);
            Instantiate(oxygen, new Vector3(tiles[temp].transform.position.x, tiles[temp].transform.position.y + 1.3f, tiles[temp].transform.position.z), Quaternion.Euler(0f,0f,-45.0f));
        }
    }
}
