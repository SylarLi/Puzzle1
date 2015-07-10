using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform master;

    private void Update()
    {
        if (master != null)
        {
            transform.parent = master.parent;
            transform.localPosition = master.localPosition;
            transform.localRotation = master.localRotation;
            transform.localScale = master.localScale;
        }
    }
}
