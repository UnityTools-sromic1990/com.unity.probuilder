﻿using System.Collections.Generic;

namespace UnityEngine.ProBuilder
{
    public class PlaneShape : Shape
    {
        [Range(0.01f, 2f)]
        [SerializeField]
        float doorHeight = .5f;

        [Range(0.01f, 2f)]
        [SerializeField]
        float legWidth = .75f;

        public override void RebuildMesh(ProBuilderMesh mesh, Vector3 size)
        {
            float totalWidth = size.x;
            float totalHeight = size.y;
            float depth = size.z;

            float xLegCoord = totalWidth / 2f;
            legWidth = xLegCoord - legWidth;
            var ledgeHeight = totalHeight - doorHeight;

            // 8---9---10--11
            // |           |
            // 4   5---6   7
            // |   |   |   |
            // 0   1   2   3
            Vector3[] template = new Vector3[12]
            {
                new Vector3(-xLegCoord, 0f, depth),           // 0
                new Vector3(-legWidth, 0f, depth),            // 1
                new Vector3(legWidth, 0f, depth),             // 2
                new Vector3(xLegCoord, 0f, depth),            // 3
                new Vector3(-xLegCoord, ledgeHeight, depth),  // 4
                new Vector3(-legWidth, ledgeHeight, depth),   // 5
                new Vector3(legWidth, ledgeHeight, depth),    // 6
                new Vector3(xLegCoord, ledgeHeight, depth),   // 7
                new Vector3(-xLegCoord, totalHeight, depth),  // 8
                new Vector3(-legWidth, totalHeight, depth),   // 9
                new Vector3(legWidth, totalHeight, depth),    // 10
                new Vector3(xLegCoord, totalHeight, depth)    // 11
            };

            List<Vector3> points = new List<Vector3>();

            points.Add(template[0]);
            points.Add(template[1]);
            points.Add(template[4]);
            points.Add(template[5]);

            points.Add(template[2]);
            points.Add(template[3]);
            points.Add(template[6]);
            points.Add(template[7]);

            points.Add(template[4]);
            points.Add(template[5]);
            points.Add(template[8]);
            points.Add(template[9]);

            points.Add(template[6]);
            points.Add(template[7]);
            points.Add(template[10]);
            points.Add(template[11]);

            points.Add(template[5]);
            points.Add(template[6]);
            points.Add(template[9]);
            points.Add(template[10]);

            List<Vector3> reverse = new List<Vector3>();

            for (int i = 0; i < points.Count; i += 4)
            {
                reverse.Add(points[i + 1] - Vector3.forward * depth);
                reverse.Add(points[i + 0] - Vector3.forward * depth);
                reverse.Add(points[i + 3] - Vector3.forward * depth);
                reverse.Add(points[i + 2] - Vector3.forward * depth);
            }

            points.AddRange(reverse);

            points.Add(template[6]);
            points.Add(template[5]);
            points.Add(template[6] - Vector3.forward * depth);
            points.Add(template[5] - Vector3.forward * depth);

            points.Add(template[2] - Vector3.forward * depth);
            points.Add(template[2]);
            points.Add(template[6] - Vector3.forward * depth);
            points.Add(template[6]);

            points.Add(template[1]);
            points.Add(template[1] - Vector3.forward * depth);
            points.Add(template[5]);
            points.Add(template[5] - Vector3.forward * depth);

            mesh.GeometryWithPoints(points.ToArray());
        }
    }
}
