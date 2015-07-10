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
    public const float QuadSprinkleDuration = 0.7f;

    public const float PuzzleDepth = 20;

    public const string QuadUnifiedRotateId = "Rotate";
    public const string QuadUnifiedScaleId = "Scale";

    private static Shader quadShader;

    private static Material quadMaterial;

    public static Shader GetQuadShader()
    {
        if (quadShader == null)
        {
            quadShader = Resources.Load<Shader>("Quad");
        }
        return quadShader;
    }

    public static Material GetQuadMaterial()
    {
        if (quadMaterial == null)
        {
            quadMaterial = new Material(GetQuadShader());
        }
        return quadMaterial;
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

    public static Color GetColor(QuadValue value, float alpha = 1)
    {
        Color color = Color.white;
        switch (value)
        {
            case QuadValue.Front:
                {
                    color = new Color(1, 0.5f, 0, alpha);
                    break;
                }
            case QuadValue.Back:
                {
                    color = new Color(0, 0.5f, 1, alpha);
                    break;
                }
            case QuadValue.Block:
                {
                    color = new Color(0.4f, 0.2f, 0, alpha);
                    break;
                }
            default:
                {
                    if ((value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                    {
                        color = new Color(0, 0.6f, 0, alpha);
                    }
                    break;
                }
        }
        return color;
    }

    public static Vector3 GetAngles(QuadValue value)
    {
        Vector3 angles = Vector3.zero;
        switch (value)
        {
            case QuadValue.Front:
            case QuadValue.Back:
            case QuadValue.Block:
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
}
