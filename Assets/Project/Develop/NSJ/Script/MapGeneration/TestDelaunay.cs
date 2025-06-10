using Procedural_Map_Generation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDelaunay : MonoBehaviour
{
    [SerializeField] private TestRoom[] _testRooms;

    DelaunayTriangulator _delaunayTriangulator = new DelaunayTriangulator();

    List<Triangle> _triangles;

    List<Edge> _mstEdges;
    private void Start()
    {
        List<Vertex> vertices = new List<Vertex>();
        foreach (TestRoom room in _testRooms)
        {
            vertices.Add(new Vertex(room.transform.position));
        }

         _triangles = _delaunayTriangulator.Triangulate(vertices);

         _mstEdges = MSTUtility.GetMST(_triangles);
    }

    private void OnDrawGizmos()
    {
        if (_triangles == null)
            return;

        Gizmos.color = Color.cyan;

        foreach (Triangle triangle in _triangles)
        {
            Gizmos.DrawLine(triangle.A.Pos, triangle.B.Pos);
            Gizmos.DrawLine(triangle.B.Pos, triangle.C.Pos);
            Gizmos.DrawLine(triangle.C.Pos, triangle.A.Pos);
        }

        Gizmos.color = Color.red;

        foreach (Edge edge in _mstEdges)
        {
            Gizmos.DrawLine(edge.A.Pos, edge.B.Pos);
        }
    }
}
