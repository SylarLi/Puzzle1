using UnityEngine;

public static class Style
{
    public const float QuadSize = 1;
    public const float QuadGap = 0.1f;

    public const float QuadRollAngle = 180f;
    public const float QuadRollDuration = 0.5f;
    public const float QuadRollDelay = 0.05f;

    public const float QuadTouchScale = 0.7f;
    public const float QuadTouchScaleDuration = 0.2f;

    public const float QuadShakeAngle = 45f;
    public const float QuadShakeDuration = 0.25f;

    public const float QuadConflictAngle = 90f;
    public const float QuadConflictDuration = 0.25f;

    public const float QuadSprinkleScale = 2.5f;
    public const float QuadSprinkleDepthAddition = -5f;
    public const float QuadSprinkleDuration = 0.5f;

    public const float PuzzleDepth = 20;

    public const string QuadUnifiedRotateId = "Rotate";
    public const string QuadUnifiedScaleId = "Scale";

    private static Material unlit;
    private static Material particleAdd;
    private static Material diffuse;


    public static Material GetQuadMaterial(VisionMaterial type)
    {
        Material material = null;
        switch (type)
        {
            case VisionMaterial.Unlit:
                {
                    material = unlit != null ? unlit : (unlit = new Material(Resources.Load<Shader>("QuadUnlit")));
                    break;
                }
            case VisionMaterial.ParticleAdd:
                {
                    material = particleAdd != null ? particleAdd : (particleAdd = new Material(Resources.Load<Shader>("QuadParticleAdd")));
                    break;
                }
            default:
                {
                    material = diffuse != null ? diffuse : (diffuse = new Material(Resources.Load<Shader>("QuadDiffuse")));
                    break;
                }
        }
        return material;
    } 

    public static Mesh GetQuadMesh()
    {
        float size = QuadSize;
        float halfSize = size * 0.5f;
        Mesh quadMesh = new Mesh();
        quadMesh.name = "QuadMesh";
        quadMesh.MarkDynamic();
        Vector3[] vertices = new Vector3[]
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
        Vector2[] uv = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        Color32[] colors32 = new Color32[vertices.Length];
        for (int i = vertices.Length - 1; i >= 0; i--)
        {
            uv[i] = i < 4 ? new Vector2(vertices[i].x / size + 0.5f, 0.5f - vertices[i].y / size) : new Vector2(0.5f - vertices[i].x / size, 0.5f - vertices[i].y / size);
            normals[i] = i < 4 ? -Vector3.forward : Vector3.forward;
            colors32[i] = Color.white;
        }
        quadMesh.vertices = vertices;
        quadMesh.uv = uv;
        quadMesh.normals = normals;
        quadMesh.colors32 = colors32;
        quadMesh.subMeshCount = 2;
        quadMesh.SetTriangles(new int[]
        {
            0, 1, 2,
            0, 3, 1,
        }, 0);
        quadMesh.SetTriangles(new int[]
        {
            6, 5, 4,
            5, 7, 4
        }, 1);
        return quadMesh;
    }

    public static Mesh GetArrowMesh()
    {
        float size = QuadSize;
        float up_y = size * 0.4f;
        float down_y = -size * 0.3f;
        float left_x = -size * 0.3f;
        float right_x = size * 0.3f;
        float center_y = -size * 0.1f;
        Mesh arrowMesh = new Mesh();
        arrowMesh.name = "ArrowMesh";
        arrowMesh.MarkDynamic();
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, up_y, 0),
            new Vector3(0, center_y, 0),
            new Vector3(left_x, down_y, 0),
            new Vector3(right_x, down_y, 0),

            new Vector3(0, up_y, 0),
            new Vector3(0, 0, 0),
            new Vector3(left_x, down_y, 0),
            new Vector3(right_x, down_y, 0),
        };
        Vector2[] uv = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        Color32[] colors32 = new Color32[vertices.Length];
        for (int i = vertices.Length - 1; i >= 0; i--)
        {
            uv[i] = i < 4 ? new Vector2(vertices[i].x / size + 0.5f, 0.5f - vertices[i].y / size) : new Vector2(0.5f - vertices[i].x / size, 0.5f - vertices[i].y / size);
            normals[i] = i < 4 ? -Vector3.forward : Vector3.forward;
            colors32[i] = Color.white;
        }
        arrowMesh.vertices = vertices;
        arrowMesh.uv = uv;
        arrowMesh.normals = normals;
        arrowMesh.colors32 = colors32;
        arrowMesh.subMeshCount = 2;
        arrowMesh.SetTriangles(new int[]
        {
            0, 1, 2,
            0, 3, 1,
        }, 0);
        arrowMesh.SetTriangles(new int[]
        {
            6, 5, 4,
            5, 7, 4
        }, 1);
        return arrowMesh;
    }

    public static Color32[] GetColors(QuadValue value, float alpha = 1)
    {
        Color32[] colors = new Color32[8];
        Color color1 = Color.white;
        Color color2 = Color.white;
        switch (value)
        {
            case QuadValue.Front:
                {
                    color1 = new Color(1, 0.5f, 0, alpha);
                    color2 = new Color(0, 0.5f, 1, alpha);
                    break;
                }
            case QuadValue.Back:
                {
                    color1 = new Color(0, 0.5f, 1, alpha);
                    color2 = new Color(1, 0.5f, 0, alpha);
                    break;
                }
            default:
                {
                    if ((value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                    {
                        color1 = color2 = new Color(0, 0.6f, 0, alpha);
                    }
                    break;
                }
        }
        for (int i = colors.Length - 1; i >= 0; i--)
        {
            colors[i] = i < 4 ? color1 : color2;
        }
        return colors;
    }

    public static Vector3 GetAngles(QuadValue value)
    {
        Vector3 angles = Vector3.zero;
        switch (value)
        {
            case QuadValue.Front:
            case QuadValue.Back:
                {
                    angles = Vector3.zero;
                    break;
                }
            case QuadValue.Left | QuadValue.Up:
                {
                    angles = new Vector3(0, 0, 45);
                    break;
                }
            case QuadValue.Right | QuadValue.Up:
                {
                    angles = new Vector3(0, 0, -45);
                    break;
                }
            case QuadValue.Left | QuadValue.Down:
                {
                    angles = new Vector3(0, 0, 135);
                    break;
                }
            case QuadValue.Right | QuadValue.Down:
                {
                    angles = new Vector3(0, 0, -135);
                    break;
                }
            case QuadValue.Left:
                {
                    angles = new Vector3(0, 0, 90);
                    break;
                }
            case QuadValue.Up:
                {
                    angles = new Vector3(0, 0, 0);
                    break;
                }
            case QuadValue.Right:
                {
                    angles = new Vector3(0, 0, -90);
                    break;
                }
            case QuadValue.Down:
                {
                    angles = new Vector3(0, 0, 180);
                    break;
                }
        }
        return angles;
    }

    public static bool GetTouchEnable(QuadValue value)
    {
        return value != QuadValue.Block;
    }
}
