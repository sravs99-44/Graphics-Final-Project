using UnityEngine;

public class ChargeBehavior : MonoBehaviour
{
    public bool isPositive = true; // Set true for positive charge, false for negative
    public GameObject fieldLinePrefab; // Assign the FieldLine prefab

    void Start()
    {
        // Create multiple field lines around the charge
        for (int i = 0; i < 18; i++)
        {
            CreateFieldLine(i);
        }
    }

    void CreateFieldLine(int index)
    {
        // Instantiate a field line from the prefab
        GameObject line = Instantiate(fieldLinePrefab, transform.position, Quaternion.identity);
        LineRenderer lr = line.GetComponent<LineRenderer>();

        // Set start point at charge position
        lr.SetPosition(0, transform.position);

        // Calculate end position based on charge type and direction
        Vector3 direction = Quaternion.Euler(0, index * 45, 0) * Vector3.forward;
        if (!isPositive)
            direction = -direction;

        // Adjust distance of field lines
        lr.SetPosition(1, transform.position + direction * 2);
    }
}