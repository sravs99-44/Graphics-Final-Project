using UnityEngine;

public class DrawLinesToSphere : MonoBehaviour
{
    public Transform volumeCenter;  // Center of the volume sphere
    public float volumeRadius = 5.0f;  // Radius of the volume sphere

    private LineRenderer[] lineRenderers;
    private int lineCount = 8; // Number of lines in different directions

    void Start()
    {
        // Initialize LineRenderers for each direction
        lineRenderers = new LineRenderer[lineCount];
        for (int i = 0; i < lineCount; i++)
        {
            GameObject lineObj = new GameObject("Line" + i);
            lineRenderers[i] = lineObj.AddComponent<LineRenderer>();

            // Set line appearance
            //lineRenderers[i].material = new Material(Shader.Find("Sprites/Default"));
            //lineRenderers[i].startColor = Color.white;
            lineRenderers[i].material = new Material(Shader.Find("Unlit/Color"));
            lineRenderers[i].material.color = Color.white;

            //lineRenderers[i].endColor = Color.white;
            lineRenderers[i].startWidth = 0.5f;
            lineRenderers[i].endWidth = 0.5f;
        }
    }

    void Update()
    {
        // Check if the charge is within the volume sphere
        if (Vector3.Distance(transform.position, volumeCenter.position) <= volumeRadius)
        {
            // Draw lines from the charge to the inside surface of the sphere
            DrawLines();
        }
        else
        {
            // Disable lines if the charge is outside the volume sphere
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.enabled = false;
            }
        }
    }

    private void DrawLines()
    {
        Vector3 chargePosition = transform.position;

        // Enable each line renderer
        foreach (var lineRenderer in lineRenderers)
        {
            lineRenderer.enabled = true;
        }

        // Directions for the lines
        Vector3[] directions = {
            Vector3.up, Vector3.down, Vector3.left, Vector3.right,
            Vector3.forward, Vector3.back,
            new Vector3(1, 1, 1).normalized, new Vector3(-1, -1, -1).normalized
        };

        for (int i = 0; i < directions.Length; i++)
        {
            // Calculate the end point on the sphere's inner surface
            Vector3 endPosition = chargePosition + directions[i] * (volumeRadius - Vector3.Distance(chargePosition, volumeCenter.position));

            // Set line renderer positions
            lineRenderers[i].SetPosition(0, chargePosition);  // Start at charge position
            lineRenderers[i].SetPosition(1, endPosition);      // End at the calculated point
        }
    }
}
