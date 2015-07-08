using UnityEngine;

public static class LevelViewUtil
{
    private static Shader quadShader;

    public static Shader GetQuadShader()
    {
        if (quadShader == null)
        {
            quadShader = Resources.Load<Shader>("Quad");
        }
        return quadShader;
    }

    public static Mesh GetQuadMesh(float size = 1)
    {
        float halfSize = size / 2f;
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(-halfSize, halfSize, 0),
            new Vector3(halfSize, -halfSize, 0),
            new Vector3(-halfSize, -halfSize, 0),
            new Vector3(halfSize, halfSize, 0),

            new Vector3(-halfSize, halfSize, 0),
            new Vector3(halfSize, -halfSize, 0),
            new Vector3(-halfSize, -halfSize, 0),
            new Vector3(halfSize, halfSize, 0),
        };
        mesh.normals = new Vector3[]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,

            Vector3.forward,
            Vector3.forward,
            Vector3.forward,
            Vector3.forward,
        };
        mesh.colors = new Color[]
        {
            Color.white,
            Color.white,
            Color.white,
            Color.white,

            Color.white,
            Color.white,
            Color.white,
            Color.white,
        };
        mesh.subMeshCount = 2;
        mesh.SetTriangles(new int[]
        {
            0, 1, 2,
            0, 3, 1,
        }, 0);
        mesh.SetTriangles(new int[]
        {
            6, 5, 4,
            5, 7, 4
        }, 1);
        return mesh;
    }
}
