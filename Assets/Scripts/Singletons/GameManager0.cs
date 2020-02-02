using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager0 : MonoBehaviour
{
    public static GameManager0 Instance = null;
    public SpaceShip spaceship;
    public GameObject end;

    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI percentFixed;

    private string DISTANCE = "Distance";
    private string PERCENTAGE = "Percentage Fixed : ";

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        distanceText.text = DISTANCE + " : " + this.calculateDistance() + " m";
        percentFixed.text = PERCENTAGE + this.GetPercentageFixed().ToString() + " %";
    }

    private string calculateDistance()
    {
        float dist = Vector3.Distance(spaceship.transform.position, end.transform.position);
        return dist.ToString();
    }

    public float GetPercentageFixed()
    {
        return spaceship.GetPercentFixed();
    }

}
