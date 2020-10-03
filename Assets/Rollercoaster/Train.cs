﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{

    public List<TrainCar> cars;

    public TrainCar carPrefab;
    public TrainCar frontCarPrefab;

    public float carWidth;

    public float frontCarWidth;

    public int numberOfCars;

    // Start is called before the first frame update
    void Start()
    {

        Vector3 offset = new Vector3(0, 0, 0);
        
        for (int i = 0; i < numberOfCars; i++)
        {

            TrainCar prefab = carPrefab;
            float xoffset = carWidth;

            if ( i == 0 ) {
                prefab = frontCarPrefab;
                xoffset = frontCarWidth;
            }

            TrainCar car = Instantiate(prefab, transform);
            car.transform.localPosition = offset;
            offset -= new Vector3(xoffset, 0, 0);


        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}