using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Procedural_Map_Generation;

public class TestRoom : MonoBehaviour
{
    private Vertex _vertex;

    private void Awake()
    {
        _vertex = new Vertex(transform.position);
    }
}
