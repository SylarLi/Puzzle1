using UnityEngine;

public static class Style
{
    public const float PuzzleDepth = 20;

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

    public static readonly Vector3[] QuadVertics = new Vector3[]
    {
        new Vector3(-QuadSize / 2, QuadSize / 2, 0),
        new Vector3(QuadSize / 2, -QuadSize / 2, 0),
        new Vector3(-QuadSize / 2, -QuadSize / 2, 0),
        new Vector3(QuadSize / 2, QuadSize / 2, 0),

        new Vector3(-QuadSize / 2, QuadSize / 2, 0),
        new Vector3(QuadSize / 2, -QuadSize / 2, 0),
        new Vector3(-QuadSize / 2, -QuadSize / 2, 0),
        new Vector3(QuadSize / 2, QuadSize / 2, 0),  
    };
    public static readonly Vector2[] QuadUV = new Vector2[]
    {
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),
        new Vector2(1, 1),

        new Vector2(1, 1),
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(0, 1),
    };
    public static readonly Vector3[] QuadNormals = new Vector3[] 
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
    public static readonly Color32[] QuadColors = new Color32[]
    {
        new Color32(255, 255, 255, 255),
        new Color32(255, 255, 255, 255),
        new Color32(255, 255, 255, 255),
        new Color32(255, 255, 255, 255),

        new Color32(255, 255, 255, 255),
        new Color32(255, 255, 255, 255),
        new Color32(255, 255, 255, 255),
        new Color32(255, 255, 255, 255),
    };
    public static readonly Vector2 QuadUVTilling = new Vector2(0.25f, 0.25f);

    public const string QuadUnifiedRotateId = "Rotate";
    public const string QuadUnifiedScaleId = "Scale";

    private static Texture texture;
    private static Material unlit;
    private static Material particleAdd;
    private static Material diffuse;

    private static Vector2[] quadNormalizedUV;


    public static Material GetQuadMaterial(VisionMaterial type)
    {
        if (texture == null)
        {
            texture = Resources.Load<Texture>("Texture");
        }
        Material material = null;
        switch (type)
        {
            case VisionMaterial.Unlit:
                {
                    if (unlit == null)
                    {
                        unlit = new Material(Resources.Load<Shader>("QuadUnlit"));
                        unlit.SetTexture("_MainTex", texture);
                    }
                    material = unlit;
                    break;
                }
            case VisionMaterial.ParticleAdd:
                {
                    if (particleAdd == null)
                    {
                        particleAdd = new Material(Resources.Load<Shader>("QuadParticleAdd"));
                        particleAdd.SetTexture("_MainTex", texture);
                    }
                    material = particleAdd;
                    break;
                }
            default:
                {
                    if (diffuse == null)
                    {
                        diffuse = new Material(Resources.Load<Shader>("QuadDiffuse"));
                        diffuse.SetTexture("_MainTex", texture);
                    }
                    material = diffuse;
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
        quadMesh.vertices = QuadVertics;
        quadMesh.uv = QuadUV;
        quadMesh.normals = QuadNormals;
        quadMesh.colors32 = QuadColors;
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

    /// <summary>
    /// 正面和反面的UVOffset
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector2[] GetQuadUVOffsets(QuadValue value)
    {
        Vector2[] offsets = new Vector2[2];
        switch (value)
        {
            case QuadValue.Front:
                {
                    offsets[0] = new Vector2(0, 0);
                    offsets[1] = new Vector2(0.25f, 0);
                    break;
                }
            case QuadValue.Back:
                {
                    offsets[0] = new Vector2(0.25f, 0);
                    offsets[1] = new Vector2(0, 0);
                    break;
                }
            case QuadValue.Block:
                {
                    offsets[0] = offsets[1] = new Vector2(0.75f, 0);
                    break;
                }
            default:
                {
                    if ((value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                    {
                        offsets[0] = offsets[1] = new Vector2(0.5f, 0);
                    }
                    break;
                }
        }
        return offsets;
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
