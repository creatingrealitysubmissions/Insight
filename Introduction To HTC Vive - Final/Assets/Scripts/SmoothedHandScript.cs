using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothedHandScript : MonoBehaviour
{
    [SerializeField]
    private GameObject RightController;
    public int filterSampleSize = 5;

    void MoveSelfToLocationOfInterest(Vector3 filteredPosition, Vector3 filteredEulerAngles)
    {
        this.transform.position = filteredPosition;
        this.transform.eulerAngles = filteredEulerAngles;
    }
    // Use this for initialization
    public List<Transform> transformsList = new List<Transform>();
    // Update is called once per frame
    void Update()
    {
        Transform currentTransform = RightController.transform;
        transformsList.Add(currentTransform);
        int difference = transformsList.Count - filterSampleSize;
        if (difference != 0)
        {
            for (int i = 0; i < difference; i++)
            {
                transformsList.RemoveAt(0);
            }
            
        }

   
        Vector3 xyzSum = new Vector3(0f, 0f, 0f);
        Vector3 eulerAnglesSum = new Vector3(0f, 0f, 0f);

        for (int i = 0; i < transformsList.Count; i++)
        {
            Quaternion dataPoint = transformsList[i].rotation;
            xyzSum += new Vector3(dataPoint.x, dataPoint.y, dataPoint.z);
            eulerAnglesSum += dataPoint.eulerAngles;
        }
        Vector3 filteredPosition = xyzSum / transformsList.Count;
        Vector3 filteredEulerAngles = eulerAnglesSum / transformsList.Count;
        Transform latestTransform = transformsList[transformsList.Count - 1];
        transform.SetPositionAndRotation(latestTransform.position, latestTransform.rotation);


    }
}