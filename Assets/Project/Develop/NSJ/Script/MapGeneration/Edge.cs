using System;

namespace Procedural_Map_Generation
{
    public class Edge : IEquatable<Edge>
    {
        // Vertex간 연결
        public Vertex A;
        public Vertex B;

        // Edge의 길이
        public float LengthSquared => (A.Pos - B.Pos).sqrMagnitude;

        public Edge(Vertex a, Vertex b)
        {
            A = a;
            B = b;
        }

        /// <summary>
        /// 두 Edge가 동일한지 검사
        /// </summary>
        public bool Equals(Edge edge)
        {
            return (A.Equals(edge.A) && B.Equals(edge.B)) || (A.Equals(edge.B) && B.Equals(edge.A));
        }

        /// <summary>
        /// 두 Edge의 해시코드를 반환
        /// </summary>
        public override int GetHashCode()
        {
            int hash = A.GetHashCode() ^ B.GetHashCode();
            return hash;
        }
    }
}