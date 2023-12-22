using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Breaks the mesh of an object into pieces.
/// </summary>
public class MeshBreaker : MonoBehaviour
{
    [Header("Mesh Breaker Settings")]
    [Tooltip("How many times the mesh will be cut"), SerializeField]
    private int _cutCascades = 1;
    [Tooltip("How much force will be applied to the broken pieces"), SerializeField]
    private float _explodeForce = 0;

    private bool _edgeSet = false;
    private Vector3 _edgeVertex = Vector3.zero;
    private Vector2 _edgeUV = Vector2.zero;
    private Plane _edgePlane = new();

    /// <summary>
    /// Breaks the mesh into pieces.
    /// </summary>
    public void BreakMesh()
    {
        Mesh originalMesh = GetComponent<MeshFilter>().mesh;
        originalMesh.RecalculateBounds();
        List<PartMesh> parts = new();
        List<PartMesh> subParts = new();

        PartMesh mainPart = new()
        {
            UV = originalMesh.uv,
            Vertices = originalMesh.vertices,
            Normals = originalMesh.normals,
            Triangles = new int[originalMesh.subMeshCount][],
            Bounds = originalMesh.bounds
        };
        for (int i = 0; i < originalMesh.subMeshCount; i++)
            mainPart.Triangles[i] = originalMesh.GetTriangles(i);

        parts.Add(mainPart);

        for (int c = 0; c < _cutCascades; c++)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                Bounds bounds = parts[i].Bounds;
                bounds.Expand(0.5f);

                Plane plane = new(Random.onUnitSphere, 
                new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)));

                subParts.Add(GenerateMesh(parts[i], plane, true));
                subParts.Add(GenerateMesh(parts[i], plane, false));
            }
            parts = new List<PartMesh>(subParts);
            subParts.Clear();
        }

        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].MakeGameobject(this);
            parts[i].GameObject.GetComponent<Rigidbody>().AddForceAtPosition(parts[i].Bounds.center * _explodeForce, transform.position);
        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    /// <summary>
    /// Generates a mesh from the original mesh and a plane.
    /// </summary>
    private PartMesh GenerateMesh(PartMesh original, Plane plane, bool left)
    {
        PartMesh partMesh = new() { };
        Ray ray1 = new();
        Ray ray2 = new();


        for (int i = 0; i < original.Triangles.Length; i++)
        {
            int[] triangles = original.Triangles[i];
            _edgeSet = false;

            for (int j = 0; j < triangles.Length; j += 3)
            {
                bool sideA = plane.GetSide(original.Vertices[triangles[j]]) == left;
                bool sideB = plane.GetSide(original.Vertices[triangles[j + 1]]) == left;
                bool sideC = plane.GetSide(original.Vertices[triangles[j + 2]]) == left;

                int sideCount = (sideA ? 1 : 0) +
                                (sideB ? 1 : 0) +
                                (sideC ? 1 : 0);
                if (sideCount == 0)
                {
                    continue;
                }
                if (sideCount == 3)
                {
                    partMesh.AddTriangle(i,
                                         original.Vertices[triangles[j]], original.Vertices[triangles[j + 1]], original.Vertices[triangles[j + 2]],
                                         original.Normals[triangles[j]], original.Normals[triangles[j + 1]], original.Normals[triangles[j + 2]],
                                         original.UV[triangles[j]], original.UV[triangles[j + 1]], original.UV[triangles[j + 2]]);
                    continue;
                }

                int singleIndex = sideB == sideC ? 0 : sideA == sideC ? 1 : 2;

                ray1.origin = original.Vertices[triangles[j + singleIndex]];
                Vector3 dir1 = original.Vertices[triangles[j + ((singleIndex + 1) % 3)]] - original.Vertices[triangles[j + singleIndex]];
                ray1.direction = dir1;
                plane.Raycast(ray1, out float enter1);
                float lerp1 = enter1 / dir1.magnitude;

                ray2.origin = original.Vertices[triangles[j + singleIndex]];
                Vector3 dir2 = original.Vertices[triangles[j + ((singleIndex + 2) % 3)]] - original.Vertices[triangles[j + singleIndex]];
                ray2.direction = dir2;
                plane.Raycast(ray2, out float enter2);
                float lerp2 = enter2 / dir2.magnitude;

                AddEdge(i,
                        partMesh,
                        left ? plane.normal * -1f : plane.normal,
                        ray1.origin + ray1.direction.normalized * enter1,
                        ray2.origin + ray2.direction.normalized * enter2,
                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 2) % 3)]], lerp2));

                if (sideCount == 1)
                {
                    partMesh.AddTriangle(i,
                                        original.Vertices[triangles[j + singleIndex]],
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        ray2.origin + ray2.direction.normalized * enter2,
                                        original.Normals[triangles[j + singleIndex]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        original.UV[triangles[j + singleIndex]],
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 2) % 3)]], lerp2));
                    
                    continue;
                }

                if (sideCount == 2)
                {
                    partMesh.AddTriangle(i,
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        original.Vertices[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.Vertices[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.Normals[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.Normals[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.UV[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.UV[triangles[j + ((singleIndex + 2) % 3)]]);
                    partMesh.AddTriangle(i,
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        original.Vertices[triangles[j + ((singleIndex + 2) % 3)]],
                                        ray2.origin + ray2.direction.normalized * enter2,
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.Normals[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.UV[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 2) % 3)]], lerp2));
                    continue;
                }


            }
        }

        partMesh.FillArrays();

        return partMesh;
    }

    /// <summary>
    /// Adds an edge to the mesh.
    /// </summary>
    private void AddEdge(int subMesh, PartMesh partMesh, Vector3 normal, Vector3 vertex1, Vector3 vertex2, Vector2 uv1, Vector2 uv2)
    {
        if (!_edgeSet)
        {
            _edgeSet = true;
            _edgeVertex = vertex1;
            _edgeUV = uv1;
        }
        else
        {
            _edgePlane.Set3Points(_edgeVertex, vertex1, vertex2);

            partMesh.AddTriangle(subMesh,
                                _edgeVertex,
                                _edgePlane.GetSide(_edgeVertex + normal) ? vertex1 : vertex2,
                                _edgePlane.GetSide(_edgeVertex + normal) ? vertex2 : vertex1,
                                normal,
                                normal,
                                normal,
                                _edgeUV,
                                uv1,
                                uv2);
        }
    }
    /// <summary>
    /// Destroys the pieces after a set amount of time.
    /// </summary>
    public class DestroyAfterTime : MonoBehaviour
    {
        private readonly float _lifetime = 10f; 

        private void Awake()
        {
            Destroy(gameObject, _lifetime);
        }
    }
    
    /// <summary>
    /// A class that holds the mesh data of the piece that gets broken off.
    /// </summary>
    public class PartMesh
    {
        private readonly List<Vector3> _verticies = new();
        private readonly List<Vector3> _normals = new();
        private readonly List<List<int>> _triangles = new();
        private readonly List<Vector2> _uvs = new();
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public int[][] Triangles;
        public Vector2[] UV;
        public GameObject GameObject;
        public Bounds Bounds = new();

        public PartMesh()
        {

        }

        public void AddTriangle(int submesh, Vector3 vert1, Vector3 vert2, Vector3 vert3, Vector3 normal1, Vector3 normal2, Vector3 normal3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            if (_triangles.Count - 1 < submesh)
                _triangles.Add(new List<int>());

            _triangles[submesh].Add(_verticies.Count);
            _verticies.Add(vert1);
            _triangles[submesh].Add(_verticies.Count);
            _verticies.Add(vert2);
            _triangles[submesh].Add(_verticies.Count);
            _verticies.Add(vert3);
            _normals.Add(normal1);
            _normals.Add(normal2);
            _normals.Add(normal3);
            _uvs.Add(uv1);
            _uvs.Add(uv2);
            _uvs.Add(uv3);

            Bounds.min = Vector3.Min(Bounds.min, vert1);
            Bounds.min = Vector3.Min(Bounds.min, vert2);
            Bounds.min = Vector3.Min(Bounds.min, vert3);
            Bounds.max = Vector3.Min(Bounds.max, vert1);
            Bounds.max = Vector3.Min(Bounds.max, vert2);
            Bounds.max = Vector3.Min(Bounds.max, vert3);
        }

        public void FillArrays()
        {
            Vertices = _verticies.ToArray();
            Normals = _normals.ToArray();
            UV = _uvs.ToArray();
            Triangles = new int[_triangles.Count][];
            for (int i = 0; i < _triangles.Count; i++)
                Triangles[i] = _triangles[i].ToArray();
        }
        /// <summary>
        /// Creates a broken off piece from the mesh data.
        /// </summary>
        public void MakeGameobject(MeshBreaker original)
        {
            GameObject = new GameObject(original.name);
            GameObject.transform.SetPositionAndRotation(original.transform.position, original.transform.rotation);
            GameObject.transform.localScale = original.transform.localScale;

            Mesh mesh = new()
            {
                name = original.GetComponent<MeshFilter>().mesh.name,

                vertices = Vertices,
                normals = Normals,
                uv = UV
            };

            for (int i = 0; i < Triangles.Length; i++)
                mesh.SetTriangles(Triangles[i], i, true);

            Bounds = mesh.bounds;
            
            MeshRenderer renderer = GameObject.AddComponent<MeshRenderer>();
            renderer.materials = original.GetComponent<MeshRenderer>().materials;

            MeshFilter filter = GameObject.AddComponent<MeshFilter>();
            filter.mesh = mesh;

            MeshCollider collider = GameObject.AddComponent<MeshCollider>();
            collider.convex = true;

            GameObject.AddComponent<Rigidbody>();
            GameObject.AddComponent<DestroyAfterTime>();

            MeshBreaker meshBreaker = GameObject.AddComponent<MeshBreaker>();

            meshBreaker._cutCascades = original._cutCascades;
            meshBreaker._explodeForce = original._explodeForce;

        }

    }
}
