﻿using UnityEngine;

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

    public const float PuzzleDepth = 20;

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

    public static Color GetColor(QuadValue value)
    {
        Color color = Color.white;
        switch (value)
        {
            case QuadValue.Front:
                {
                    color = new Color(1, 0.5f, 0);
                    break;
                }
            case QuadValue.Back:
                {
                    color = new Color(0, 0.5f, 1);
                    break;
                }
            case QuadValue.Block:
                {
                    color = new Color(0.4f, 0.2f, 0);
                    break;
                }
        }
        return color;
    }
}