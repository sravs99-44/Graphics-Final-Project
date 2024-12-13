using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DragAndDropCharges : MonoBehaviour
{
    public float charge = 1.0f; // Charge value (positive for field lines)
    public Transform volumeCenter; // Center of the simulation sphere
    public float volumeRadius = 5.0f; // Radius of the simulation sphere
    public TextMeshProUGUI fluxTextUI; // UI to display flux
    public int fieldLineResolution = 120; // Points per field line
    public int fieldLineCount = 12; // Number of field lines for positive charges
    public float forceScalingFactor = 1.0f; // Controls bending intensity

    private Camera mainCamera;
    private Rigidbody rb;
    private bool isDragging = false;
    private static List<DragAndDropCharges> charges = new List<DragAndDropCharges>();
    private List<LineRenderer> fieldLines = new List<LineRenderer>();

    private const float k = 8.99e9f; // Coulomb constant (N·m²/C²)
    private const float epsilon0 = 8.854e-12f;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();


        //float totalChargeInside = 0.0f;

        // Assign charge based on the tag
        if (gameObject.CompareTag("positive"))
        {
            charge = 10000000.10f; // Positive charge
            //totalChargeInside += 1000;
        }
        else if (gameObject.CompareTag("negative"))
        {
            charge = -7000000.10f; // Negative charge
            //totalChargeInside -= 1000;
        }
        else
        {
            Debug.LogWarning($"GameObject {gameObject.name} has no valid charge tag (positive/negative). Defaulting to neutral.");
            charge = 0.0f;
        }

        // Register this charge
        charges.Add(this);

        //fluxTextUI.text = "Electric Flux: " + flux.ToString("F2");

        // Create field lines only for positive charges
        if (charge > 0)
        {
            CreateFieldLines();
        }
    }

    void OnDestroy()
    {
        charges.Remove(this);
        ClearFieldLines();
    }

    void Update()
    {
        if (!isDragging && charge > 0)
        {
            UpdateFieldLines();
        }

        // After updating field lines or whenever appropriate, compute and display flux
        CalculateAndDisplayElectricFlux();
    }

    void OnMouseDown()
    {
        isDragging = true;
        rb.isKinematic = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition();
            float distanceFromCenter = Vector3.Distance(targetPosition, volumeCenter.position);
            Debug.Log("Volume Center: " + volumeCenter.position + " Distance: " + distanceFromCenter);

            // Allow dragging within the sphere only
            if (distanceFromCenter <= volumeRadius)
            {
                transform.position = targetPosition;
            }
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.isKinematic = false;

        float distanceFromCenter = Vector3.Distance(transform.position, volumeCenter.position);

        // Enable or disable field lines based on sphere containment
        if (charge > 0)
        {
            EnableFieldLines(distanceFromCenter <= volumeRadius);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return transform.position;
    }

    private List<Vector3> initialDirections = new List<Vector3>();

    private void CreateFieldLines()
    {
        int latitudes = 6; // Number of latitudes
        int linesPerLatitude = 6; // Number of lines per latitude
        float polarStep = Mathf.PI / (latitudes + 1);
        float azimuthalStep = 2 * Mathf.PI / linesPerLatitude;

        for (int i = 0; i <= latitudes; i++)
        {
            float polarAngle = i * polarStep;

            for (int j = 0; j < linesPerLatitude; j++)
            {
                float azimuthalAngle = j * azimuthalStep;

                float x = Mathf.Sin(polarAngle) * Mathf.Cos(azimuthalAngle);
                float y = Mathf.Sin(polarAngle) * Mathf.Sin(azimuthalAngle);
                float z = Mathf.Cos(polarAngle);

                Vector3 direction = new Vector3(x, y, z).normalized;
                direction = -direction; // Inverted for inward flow

                GameObject lineObject = new GameObject($"FieldLine_{i}_{j}");
                lineObject.transform.parent = transform;

                LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
                lineRenderer.positionCount = fieldLineResolution;
                lineRenderer.startWidth = 0.02f;
                lineRenderer.endWidth = 0.02f;
                lineRenderer.useWorldSpace = true;

                Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
                lineMaterial.color = Color.red;
                lineRenderer.material = lineMaterial;

                fieldLines.Add(lineRenderer);
                initialDirections.Add(direction);
            }
        }

        EnableFieldLines(false);
    }

    private void UpdateFieldLines()
    {
        for (int i = 0; i < fieldLines.Count; i++)
        {
            LineRenderer line = fieldLines[i];
            Vector3 startPoint = transform.position;
            Vector3 direction = initialDirections[i];

            Vector3[] points = new Vector3[fieldLineResolution];
            points[0] = startPoint;

            for (int j = 1; j < fieldLineResolution; j++)
            {
                Vector3 lastPoint = points[j - 1];

                if (Vector3.Distance(lastPoint, volumeCenter.position) >= volumeRadius)
                {
                    for (int k = j; k < fieldLineResolution; k++)
                    {
                        points[k] = lastPoint;
                    }
                    break;
                }

                Vector3 field = CalculateNetElectricField(lastPoint);

                direction = (direction + field.normalized).normalized;

                Vector3 nextPoint = lastPoint + direction * (volumeRadius / fieldLineResolution);

                nextPoint = ClampToSphere(nextPoint, volumeCenter.position, volumeRadius);

                points[j] = nextPoint;
            }

            line.SetPositions(points);
        }
    }

    private Vector3 CalculateNetElectricField(Vector3 position)
    {
        if (Vector3.Distance(position, volumeCenter.position) > volumeRadius)
        {
            return Vector3.zero;
        }

        Vector3 netField = Vector3.zero;

        foreach (var otherCharge in charges)
        {
            if (Vector3.Distance(otherCharge.transform.position, volumeCenter.position) > volumeRadius)
            {
                continue;
            }

            Vector3 direction = otherCharge.transform.position - position;
            float distanceSquared = direction.sqrMagnitude;

            if (distanceSquared > 0)
            {
                float fieldStrength = k * Mathf.Abs(otherCharge.charge) / distanceSquared;

                if (otherCharge.charge > 0)
                {
                    if (charge > 0) netField -= direction.normalized * fieldStrength;
                }
                else if (otherCharge.charge < 0)
                {
                    netField += direction.normalized * fieldStrength;
                }
            }
        }

        return netField;
    }

    private void EnableFieldLines(bool enabled)
    {
        foreach (var line in fieldLines)
        {
            line.enabled = enabled;
        }
    }

    private void ClearFieldLines()
    {
        foreach (var line in fieldLines)
        {
            Destroy(line.gameObject);
        }
        fieldLines.Clear();
    }

    private Vector3 ClampToSphere(Vector3 point, Vector3 sphereCenter, float sphereRadius)
    {
        Vector3 direction = point - sphereCenter;
        float distance = direction.magnitude;

        if (distance > sphereRadius)
        {
            return sphereCenter + direction.normalized * sphereRadius;
        }
        return point;
    }

    // New method to calculate and display electric flux
    private void CalculateAndDisplayElectricFlux()
    {
        // Sum charges inside the sphere
        float totalChargeInside = 0.0f;
        int flux = 0; // Start with zero flux

        foreach (var ch in charges)
        {
            float dist = Vector3.Distance(ch.transform.position, volumeCenter.position);
            if (dist <= volumeRadius)
            {
                // Accumulate the enclosed charge
                totalChargeInside += ch.charge;

                // Add additional flux based on the tag
                if (ch.gameObject.CompareTag("positive"))
                {
                    flux += 1000;
                }
                else if (ch.gameObject.CompareTag("negative"))
                {
                    flux -= 1000;
                }
            }
        }

        // Now add the standard flux from Q_enclosed using Gauss’s law
        //flux += totalChargeInside / epsilon0;

        // Display it
        if (fluxTextUI != null)
        {
            fluxTextUI.text = "Electric Flux: " + flux.ToString();
        }
    }


}