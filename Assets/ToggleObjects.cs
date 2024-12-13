using UnityEngine;
using UnityEngine.UI;

public class ToggleObjects : MonoBehaviour
{
    public GameObject Sphere;
    public GameObject Cube;
    //public GameObject cylinder;

    public Button Spherebutton;
    public Button Cubebutton;
    //public Button cylinderButton;

    private void Start()
    {
        // Assign button listeners
        Spherebutton.onClick.AddListener(ShowSphere);
        Cubebutton.onClick.AddListener(ShowCube);
        //cylinderButton.onClick.AddListener(ShowCylinder);

        // Hide all objects initially
        HideAllObjects();
    }

    // Method to show only the sphere
    private void ShowSphere()
    {
        HideAllObjects();
        Sphere.SetActive(true);
    }

    // Method to show only the cube
    private void ShowCube()
    {
        HideAllObjects();
        Cube.SetActive(true);
    }

    // Method to show only the cylinder
    //private void ShowCylinder()
    //{
    //    HideAllObjects();
     //   cylinder.SetActive(true);
    //}

    // Method to hide all objects
    private void HideAllObjects()
    {
        Sphere.SetActive(false);
        Cube.SetActive(false);
        //cylinder.SetActive(false);
    }
}
