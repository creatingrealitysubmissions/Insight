using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothedHandScript : MonoBehaviour
{
    [SerializeField]
    private GameObject RightController;
    public int filterSampleSize = 450;
    // Use this for initialization
    public List<Transform> transformsList = new List<Transform>();
    // Update is called once per frame

    //taken from https://forum.unity.com/threads/average-quaternions.86898/
    // assuming qArray.Length > 1
    Quaternion AverageQuaternion(Quaternion[] qArray)
    {
        Quaternion qAvg = qArray[0];
        float weight;
        for (int i = 1; i < qArray.Length; i++)
        {
            weight = 1.0f / (float)(i + 1);
            qAvg = Quaternion.Slerp(qAvg, qArray[i], weight);
        }
        return qAvg;
    }
    
    private int framesSoFar = 0;
    void Update()
    {
        Transform currentTransform = RightController.transform;
        transformsList.Add(currentTransform);
        int difference = transformsList.Count - filterSampleSize;
        if (difference != 0)
        {
            for (int i = 0; i < difference; i++)
            {
                transformsList.RemoveAt(transformsList.Count-1);
            }
            
        }
        int n = transformsList.Count;
        List<Quaternion> rotationsList = new List<Quaternion>();
        for (int i = 0; i < n; i++)
        {
            rotationsList.Add(transformsList[i].rotation);
        }
        Quaternion meanQuaternion = AverageQuaternion(rotationsList.ToArray());
      
        Vector3 positionSum = new Vector3(0,0,0);
        for (int i = 0; i < n; i++)
        {
            positionSum += transformsList[i].position;
        }
        Vector3 filteredPosition = positionSum / n;
        //Transform latestTransform = transformsList[transformsList.Count - 1];
        transform.SetPositionAndRotation(filteredPosition, meanQuaternion);
        framesSoFar += 1;
        if (framesSoFar == 250)
        {
            Debug.Log(n);
        }

    }
}