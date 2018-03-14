using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothedHandScript : MonoBehaviour
{
    [SerializeField]
    private GameObject RightController;
    public int filterSampleSize = 100;
    // Use this for initialization
    // Update is called once per frame

    //taken from https://forum.unity.com/threads/average-quaternions.86898/
    // assuming qArray.Length > 1
    Quaternion AverageQuaternion(List<Quaternion> qList)
    {
        Quaternion qAvg = qList[0];
        float weight;
        for (int i = 1; i < qList.Count; i++)
        {
            weight = 1.0f / (float)(i + 1);
            qAvg = Quaternion.Slerp(qAvg, qList[i], weight);
        }
        return qAvg;
    }

    Vector3 AveragePosition(List<Vector3> tList)
    {
        Vector3 positionSum = new Vector3(0, 0, 0);
        int n = tList.Count;
        for (int i = 0; i < n; i++)
        {
            positionSum += tList[i];
        }
        Vector3 filteredPosition = positionSum / n;
        return(filteredPosition);
    }
    private List<Vector3> positionList = new List<Vector3>();
    private List<Quaternion> rotationList = new List<Quaternion>();
    public Vector3 filteredPosition;
    public Vector3 currentControllerPosition;
    public Quaternion currentControllerRotation;
    private Quaternion meanQuaternion;
    private Transform currentTransform;
    private int lowerBound;
    public bool filterOn = false;
    void LateUpdate()
    {
        currentTransform = RightController.transform;
        currentControllerPosition = currentTransform.position;
        currentControllerRotation = currentTransform.rotation;

        GameObject go_B = new GameObject();
        go_B.transform.position = currentTransform.position;
        go_B.transform.rotation = currentTransform.rotation;
        positionList.Add(go_B.transform.position);
        rotationList.Add(go_B.transform.rotation);


        if (filterOn==false)
        {
            transform.SetPositionAndRotation(currentTransform.position, currentTransform.rotation);
        }
       else if (positionList.Count < filterSampleSize)
        {
            transform.SetPositionAndRotation(currentTransform.position, currentTransform.rotation);
            Debug.Log("Filling sample set " + Time.frameCount.ToString() + "/" + filterSampleSize.ToString());
        }
        else if(positionList.Count >= filterSampleSize)
        {
            lowerBound = (positionList.Count - filterSampleSize) - 1;
            List<Vector3> subsetOfPositions = positionList.GetRange(lowerBound, filterSampleSize);
            filteredPosition = AveragePosition(subsetOfPositions);
            meanQuaternion = AverageQuaternion(rotationList.GetRange(lowerBound, filterSampleSize));
            transform.SetPositionAndRotation(filteredPosition, meanQuaternion);
        }
        else{Debug.Log("???");}} 
        
}