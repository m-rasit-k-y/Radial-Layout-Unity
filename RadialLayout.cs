using UnityEngine;
using UnityEngine.UI;
public class RadialLayout : LayoutGroup
{ 
    public float fDistance;
    
    [Range(0f,360f)]
    public float minAngle;
    
    [Range(0f,360f)]
    public float maxAngle;
    
    [Range(-360f,360f)]
    public float startAngle;

    protected override void OnEnable() 
    { base.OnEnable(); CalculateRadial(); }
    
    
    public override void SetLayoutHorizontal(){}

    public override void SetLayoutVertical(){}

    public override void CalculateLayoutInputVertical()
    {
        CalculateRadial();
    }
    public override void CalculateLayoutInputHorizontal()
    { 
        CalculateRadial();
    }
    #if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        CalculateRadial();
    }
    #endif
    private void CalculateRadial()
    {
        m_Tracker.Clear();
        if (transform.childCount == 0) return;

        var fOffsetAngle = (maxAngle - minAngle) / (transform.childCount -1);

        var fAngle = startAngle;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = (RectTransform)transform.GetChild(i);

            if (child != null)
            {
                //Adding the elements to the tracker stops the user from modifiying their positions via the editor.
                m_Tracker.Add(this, child, 
                    DrivenTransformProperties.Anchors |
                    DrivenTransformProperties.AnchoredPosition |
                    DrivenTransformProperties.Pivot |
                    DrivenTransformProperties.Rotation);

                var vPos = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0);
                child.localPosition = vPos * fDistance;
                //Force objects to be center aligned, this can be changed however I'd suggest you keep all of the objects with the same anchor points.
                child.anchorMin = child.anchorMax = child.pivot = new Vector2(0.5f, 0.5f);

                // Calculate and set rotation
                var rotationZ = fAngle + 90f;
                child.localRotation = Quaternion.Euler(0f, 0f, rotationZ);

                fAngle += fOffsetAngle;
            }
        }
    }

}
