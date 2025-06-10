using System;

namespace Procedural_Map_Generation
{
    public class Edge : IEquatable<Edge>
    {
        // Vertex�� ����
        public Vertex A;
        public Vertex B;

        // Edge�� ����
        public float LengthSquared => (A.Pos - B.Pos).sqrMagnitude;

        public Edge(Vertex a, Vertex b)
        {
            A = a;
            B = b;
        }

        /// <summary>
        /// �� Edge�� �������� �˻�
        /// </summary>
        public bool Equals(Edge edge)
        {
            return (A.Equals(edge.A) && B.Equals(edge.B)) || (A.Equals(edge.B) && B.Equals(edge.A));
        }

        /// <summary>
        /// �� Edge�� �ؽ��ڵ带 ��ȯ
        /// </summary>
        public override int GetHashCode()
        {
            int hash = A.GetHashCode() ^ B.GetHashCode();
            return hash;
        }
    }
}