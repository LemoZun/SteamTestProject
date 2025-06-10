using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public class Triangle 
    {
        public Vertex A;
        public Vertex B;
        public Vertex C;

        public readonly List<Edge> Edges;

        private Vector2 _centerPos;
        private float _radiusSpr;

        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;

            Edges = new List<Edge>
            {
                new Edge(A, B),
                new Edge(B, C),
                new Edge(C, A)
            };

            CalculateCenter();
        }

        /// <summary>
        /// �ش� ������ �ﰢ���� ���ԵǾ� �ִ��� �˻�
        /// </summary>
        public bool ContainVertex(Vertex vertex)
        {
            return A == vertex || B == vertex || C == vertex;
        }

        /// <summary>
        /// �ش� ������ �ﰢ���� �������� ���ԵǾ� �ִ��� �˻�
        /// </summary>
        public bool IsPointInsideCirCle(Vector2 pos)
        {
            float distanceSpr = (pos - _centerPos).sqrMagnitude;

            // pos�� ������ �ȿ� �ִ��� �˻�
            return distanceSpr < _radiusSpr;
        }

        private void CalculateCenter()
        {
            Vector2 A = this.A.Pos;
            Vector2 B = this.B.Pos;
            Vector2 C = this.C.Pos;

            float D = 2 * (A.x * (B.y - C.y) +
                           B.x * (C.y - A.y) +
                           C.x * (A.y - B.y));

            float centerX = ((A.x * A.x + A.y * A.y) * (B.y - C.y) +
                             (B.x * B.x + B.y * B.y) * (C.y - A.y) +
                             (C.x * C.x + C.y * C.y) * (A.y - B.y)) / D;

            float centerY = ((A.x * A.x + A.y * A.y) * (C.x - B.x) +
                             (B.x * B.x + B.y * B.y) * (A.x - C.x) +
                             (C.x * C.x + C.y * C.y) * (B.x - A.x)) / D;

            _centerPos = new Vector2(centerX, centerY);
            _radiusSpr = (A - _centerPos).sqrMagnitude;
        }
    }
}