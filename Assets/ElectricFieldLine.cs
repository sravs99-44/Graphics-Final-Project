using UnityEngine;

public class ElectricFieldLine : MonoBehaviour
{
    public Transform sourceCharge;
    public Transform sphereSurface;
    public Gradient positiveGradient; // Color for positive field lines
    public Gradient negativeGradient; // Color for negative field lines

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Start and end points
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }

    public void UpdateFieldLine(Vector3 chargePosition, Vector3 surfacePosition, bool isPositive)
    {
        lineRenderer.SetPosition(0, chargePosition);
        lineRenderer.SetPosition(1, surfacePosition);
        lineRenderer.colorGradient = isPositive ? positiveGradient : negativeGradient;
    }
}
