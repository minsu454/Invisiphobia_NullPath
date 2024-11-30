#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace UnityEditor.MapEditor
{
    [ExecuteInEditMode]
    public class MapBuilder : MonoBehaviour
    {
        public Transform[] GridTransforms
        {
            get
            {
                return gridTransform.GetComponents<Transform>();
            }
        }

        public bool Dragging { get; set; }
        public bool EditState { get; set; } = true;

        public Vector2 MapSize = new Vector2(100, 100);
        private Transform gridTransform;
        private MeshFilter gridMeshFilter;
        private List<Vector3> gridVerticeList;

        [Header("Tiles:")]
        public Color tileColor = new Color32(172, 255, 255, 255);
        public Vector3Int tileScale = new Vector3Int(1, 1, 1);

        private Vector3 tileHalfHeight;

        public Vector3 HoveredPosition { get; set; }
        public List<Vector3> SelectedPositionList { get; set; }

        public bool busy = false;

        private void OnValidate()
        {
            if (gridTransform != null)
            {
                this.Update();
            }
        }

        public void Update()
        {
            if (busy || !this.EditState)
            {
                return;
            }

            busy = true;

            this.PseudoAwake();

            this.UpdateGridMesh();

            this.ValidateTileScale();

            busy = false;
        }

        public void PseudoAwake()
        {
            gridTransform = transform.Find("Grid");

            if (gridTransform == null)
            {
                GameObject gridGameObject = new GameObject("Grid");

                gridGameObject.AddComponent<MeshFilter>();

                gridGameObject.AddComponent<MeshRenderer>();

                gridTransform = gridGameObject.transform;

                gridTransform.SetParent(this.transform);
            }

            gridTransform.hideFlags = HideFlags.NotEditable;

            gridMeshFilter = gridTransform.GetComponent<MeshFilter>();

            if (gridMeshFilter.sharedMesh == null)
            {
                gridMeshFilter.sharedMesh = new Mesh() { name = "Grid" };
            }

            if (this.SelectedPositionList == null)
            {
                this.SelectedPositionList = new List<Vector3>();
            }
        }

        private void ValidateTileScale()
        {
            int x = (int)Mathf.Clamp(tileScale.x, 1, MapSize.x);
            int y = (int)Mathf.Clamp(tileScale.y, 1, Mathf.Infinity);
            int z = (int)Mathf.Clamp(tileScale.z, 1, MapSize.y);

            tileScale = new Vector3Int(x, y, z);

            tileHalfHeight = Vector3.up * tileScale.y / 2f;
        }

        public void UpdateGridMesh()
        {
            int xSize = (int)MapSize.x - (int)(tileScale.x - 1);
            int zSize = (int)MapSize.y - (int)(tileScale.z - 1);

            List<Vector3> allVertices = new List<Vector3>();

            for (int i = 0; i < zSize; i++)
            {
                for (int j = 0; j < xSize; j++)
                {
                    float x = j - MapSize.x / 2f + tileScale.x / 2f;
                    float z = i - MapSize.y / 2f + tileScale.z / 2f;

                    Vector3 rayOrigin = new Vector3(x, int.MaxValue, z);

                    RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector3.down, Mathf.Infinity);

                    foreach (RaycastHit hit in hits)
                    {
                        Vector3 vertex = new Vector3(x, hit.point.y, z);

                        allVertices.Add(vertex);
                    }
                }
            }

            gridVerticeList = new List<Vector3>(allVertices);

            foreach (Vector3 vertex in allVertices)
            {
                bool isValid = true;

                Vector3 globalVertex = vertex + tileHalfHeight;

                Bounds vertexBounds = new Bounds(globalVertex, (Vector3)tileScale * .95f);

                Collider[] placedCollidersInReach = Physics.OverlapBox(globalVertex, (Vector3)tileScale / 2f);

                foreach (Collider placedTileCollider in placedCollidersInReach)
                {
                    if (placedTileCollider.bounds.Intersects(vertexBounds))
                    {
                        isValid = false;

                        break;
                    }
                }

                if (!isValid)
                {
                    gridVerticeList.Remove(vertex);

                    continue;
                }

                foreach (Vector3 selectedPosition in this.SelectedPositionList)
                {
                    Bounds selectedBounds = new Bounds(selectedPosition + tileHalfHeight, (Vector3)tileScale * 0.95f);

                    if (selectedBounds.Intersects(vertexBounds))
                    {
                        isValid = false;

                        break;
                    }
                }

                if (!isValid)
                {
                    gridVerticeList.Remove(vertex);
                }
            }

            gridMeshFilter.sharedMesh.Clear();

            gridMeshFilter.sharedMesh.vertices = gridVerticeList.ToArray();
        }

        private void OnDrawGizmosSelected()
        {
            if (!this.EditState)
            {
                return;
            }

            Gizmos.color = new Color(tileColor.r, tileColor.g, tileColor.b, .01f);

            for (int i = 0; i < gridVerticeList.Count; i++)
            {
                Gizmos.DrawCube(gridVerticeList[i] + tileHalfHeight, tileScale);

                Gizmos.DrawWireCube(gridVerticeList[i] + tileHalfHeight, tileScale);
            }

            if (!this.Dragging)
            {
                Gizmos.color = new Color(tileColor.r, tileColor.g, tileColor.b, .25f);

                Gizmos.DrawCube(this.HoveredPosition + tileHalfHeight, tileScale);

                Gizmos.color = new Color(tileColor.r, tileColor.g, tileColor.b, .5f);

                Gizmos.DrawWireCube(this.HoveredPosition + tileHalfHeight, tileScale);
            }
            else
            {
                foreach (Vector3 selectedPosition in this.SelectedPositionList)
                {
                    Gizmos.color = new Color(tileColor.r, tileColor.g, tileColor.b, .25f);

                    Gizmos.DrawCube(selectedPosition + tileHalfHeight, tileScale);

                    Gizmos.color = new Color(tileColor.r, tileColor.g, tileColor.b, .5f);

                    Gizmos.DrawWireCube(selectedPosition + tileHalfHeight, tileScale);
                }
            }
        }
    }
}
#endif