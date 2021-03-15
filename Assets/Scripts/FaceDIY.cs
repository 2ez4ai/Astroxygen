using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDIY : MonoBehaviour
{
    MeshRenderer m_render;
    [SerializeField]
    List<Material> faceM;

    // Start is called before the first frame update
    void Start()
    {
        m_render = GetComponent<MeshRenderer>();
        AssignFace();
    }

    void AssignFace()
    {
        int n = faceM.Count;
        m_render.material = faceM[Random.RandomRange(0, n)];
    }
}
