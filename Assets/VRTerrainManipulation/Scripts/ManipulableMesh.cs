using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTerrainManipulation
{
    public class ManipulableMesh : MonoBehaviour
    {
        private Mesh mesh;
        private Vector3[] verts;

        void Start()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            verts = mesh.vertices;
        }

        void RecalculateMesh()
        {
            mesh.vertices = verts;
            GetComponent<MeshCollider>().sharedMesh = mesh;
            mesh.RecalculateNormals();
        }

        public void ModifyMesh(Vector3 pos, float radius, float str, bool inflate)
        {
            Vector3 distance;
            for (int v = 0; v < verts.Length; v++)
            {
                distance = verts[v] - pos;

                if (distance.sqrMagnitude < radius)
                {
                    if (inflate)
                        verts[v] = verts[v] + (Vector3.up * str);
                    else
                        verts[v] = verts[v] + (Vector3.down * str);
                }

            }
            RecalculateMesh();
        }

        public void SmoothMesh(Vector3 pos, float radius, float str)
        {
            float heightSum = 0f;
            int heightCount = 0;
            Vector3 distance;
            for (int v = 0; v < verts.Length; v++)
            {
                distance = verts[v] - pos;


                if (distance.sqrMagnitude < radius)
                {
                    heightSum += verts[v].y;
                    heightCount++;
                }

            }


            float heightAvr = heightSum / heightCount;

            for (int v = 0; v < verts.Length; v++)
            {
                distance = verts[v] - pos;

                if (distance.sqrMagnitude < radius)
                {
                    verts[v].y += (heightAvr - verts[v].y) * str;
                }

            }

            RecalculateMesh();
        }
    }
}