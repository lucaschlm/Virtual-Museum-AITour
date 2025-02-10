using UnityEngine;

public class ButtonLabelArrow : MonoBehaviour
{
    public Transform buttonPosition;
    public Transform labelPosition;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        if (buttonPosition != null && labelPosition != null)
        {
            lineRenderer.SetPosition(0, buttonPosition.position);
            lineRenderer.SetPosition(1, labelPosition.position);
        }
    }
}
