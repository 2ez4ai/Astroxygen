using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapDIY : MonoBehaviour
{
    MeshRenderer m_render;
    [SerializeField]
    List<Material> capM;

    // Start is called before the first frame update
    void Start()
    {
        m_render = GetComponent<MeshRenderer>();
        AssignFace();
    }

    void AssignFace()
    {
        int n = capM.Count;
        m_render.material = capM[Random.RandomRange(0, n)];
    }
}
