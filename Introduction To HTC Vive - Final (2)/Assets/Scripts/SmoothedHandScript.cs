using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothedHandScript : MonoBehaviour
{
    [SerializeField]
    private GameObject RightController;
    private int filterSampleSize = 100;
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
    private int num_samples;
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



        if (positionList.Count < filterSampleSize)
        {
            transform.SetPositionAndRotation(currentTransform.position, currentTransform.rotation);
            Debug.Log("not enough samples, currently = " + Time.frameCount.ToString());
        }
        else if(positionList.Count >= filterSampleSize)
        {
            lowerBound = (positionList.Count - filterSampleSize) - 1;
            num_samples = filterSampleSize;
            Debug.Log("lowerbound " + lowerBound + "up to " + num_samples + "AND LISTLEN IS: " + positionList.Count);
            List<Vector3> subsetOfPositions = positionList.GetRange(lowerBound, num_samples);
            filteredPosition = AveragePosition(subsetOfPositions);
           // Debug.Log(filteredPosition);
            meanQuaternion = AverageQuaternion(rotationList.GetRange(lowerBound, num_samples));
           transform.SetPositionAndRotation(filteredPosition, meanQuaternion);
        }
        else
        {
            Debug.Log("???");
        }
            
            //  if (framesSoFar == 250)
            //  {
            //      Debug.Log(framesSoFar);
            //  }
        } 
        
}