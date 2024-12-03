#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace UnityEditor.MapEditor
{
    [ExecuteInEditMode]
    public class MapLayoutBuilder : MonoBehaviour
    {
        public Transform[] GridTransforms
        {
            get
            {
                return gridTransform.GetComponents<Transform>();
            }
        }
        public bool EditState { get; set; } = true;

        public Vector2 MapSize = new Vector2(100, 100);
        private Transform gridTransform;
        private MeshFilter gridMeshFilter;
        private List<Vector3> gridVerticeList;

        [Header("Tiles:")]
        public Color TileColor = new Color32(172, 255, 255, 255);
        public Vector3Int TileScale { get; set; } = Vector3Int.zero;

        public Vector3 HoveredPosition { get; set; }
        public Vector3 Floor { get; set; } = Vector3.zero;

        private Vector3 tileHalfHeight;

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

            this.Reset();

            this.UpdateGridMesh();

            this.ValidateTileScale();

            busy = false;
        }

        /// <summary>
        /// 리셋해주는 함수
        /// </summary>
        public void Reset()
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
        }

        /// <summary>
        /// 맵사이즈 안에서 배치하게 제약거는 함수
        /// </summary>
        private void ValidateTileScale()
        {
            int x = (int)Mathf.Clamp(TileScale.x, 1, MapSize.x);
            int y = (int)Mathf.Clamp(TileScale.y, 1, Mathf.Infinity);
            int z = (int)Mathf.Clamp(TileScale.z, 1, MapSize.y);

            TileScale = new Vector3Int(x, y, z);

            tileHalfHeight = Vector3.up * TileScale.y / 2f;
        }

        /// <summary>
        /// 그룹매쉬 업데이트 해주는 함수
        /// </summary>
        public void UpdateGridMesh()
        {
            int xSize = (int)MapSize.x - (int)(TileScale.x - 1);
            int zSize = (int)MapSize.y - (int)(TileScale.z - 1);

            List<Vector3> allVertices = new List<Vector3>();

            for (int i = 0; i < zSize; i++)
            {
                for (int j = 0; j < xSize; j++)
                {
                    float x = j - MapSize.x / 2f + TileScale.x / 2f;
                    float z = i - MapSize.y / 2f + TileScale.z / 2f;

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

                Bounds vertexBounds = new Bounds(globalVertex, (Vector3)TileScale * .95f);

                Collider[] placedCollidersInReach = Physics.OverlapBox(globalVertex, (Vector3)TileScale / 2f);

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
            }

            gridMeshFilter.sharedMesh.Clear();

            gridMeshFilter.sharedMesh.vertices = gridVerticeList.ToArray();
        }

        public void UpFloor()
        {
            Floor += Vector3.up;
        }

        public void DownFloor()
        {
            if(Floor != Vector3.zero)
                Floor += Vector3.down;
        }

        private void OnDrawGizmosSelected()
        {
            if (!this.EditState)
            {
                return;
            }

            Color c1;
            Color c2;

            c1 = c2 = new Color(TileColor.r, TileColor.g, TileColor.b, .01f);

            for (int i = 0; i < gridVerticeList.Count; i++)
            {
                DrawGizmoCube(gridVerticeList[i] + tileHalfHeight, c1, c2);

            }

            c1 = new Color(TileColor.r, TileColor.g, TileColor.b, .25f);
            c2 = new Color(TileColor.r, TileColor.g, TileColor.b, .5f);

            DrawGizmoCube(this.HoveredPosition + tileHalfHeight, c1, c2);
        }
        
        /// <summary>
        /// 기즈모 큐브형태 띄워주는 함수
        /// </summary>
        private void DrawGizmoCube(Vector3 center, Color c1, Color c2)
        {
            Gizmos.color = c1;

            Gizmos.DrawCube(center, TileScale);

            Gizmos.color = c2;

            Gizmos.DrawWireCube(center, TileScale);
        }
    }
}
#endif