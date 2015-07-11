using Core;
using UnityEngine;

public interface IVision : IEventDispatcher
{
    float alpha { get; }

    float parentAlpha { get; set; }

    float localAlpha { get; set; }

    Vector3 localPosition { get; set; }

    Vector3 localEulerAngles { get; set; }

    Vector3 localScale { get; set; }

    VisionMaterial material { get; set; }

    bool touchEnable { get; set; }

    void Spark(VisionSpark spark);
}
