using UnityEngine;

public class ObjectArrayLayout : MonoBehaviour
{
    public Vector3 positionChange;
    public Vector3 rotationChange;
    public Vector3 scaleMultiplier = Vector3.one;
    
    public void Layout()
    {
        var l = transform.childCount;
        if (l > 0)
        {
            var first = transform.GetChild(0);
            var curPos = first.localPosition;
            var curRot = first.localEulerAngles;
            var curScale = first.localScale;
            for (var i = 1; i < l; i++)
            {
                curPos += positionChange;
                curRot += rotationChange;
                curScale = new Vector3(curScale.x * scaleMultiplier.x, curScale.y * scaleMultiplier.y, curScale.z * scaleMultiplier.z);
                var child = transform.GetChild(i);
                child.localPosition = curPos;
                child.localEulerAngles = curRot;
                child.localScale = curScale;
            }
        }
    }
}
