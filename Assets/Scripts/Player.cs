using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

	public int numCollected;
	private int maxCollectable;
	private Rigidbody rb;
	//public Text setCollectedText;
	//public Text completeText;
    public float collectableCenterPointX = 0.30f;

	// Use this for initialization
	void Start () {
		numCollected = 0;
		maxCollectable = 15;
		rb = GetComponent<Rigidbody> ();
		//completeText.text = "";
		//SetNumberCollectedText(); 
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other) {
        Debug.Log("trigger");
		if (other.gameObject.CompareTag ("Collectable")) {
            // set the pick up to invisible after collectable touched
            float newLocation = Random.Range(collectableCenterPointX -0.2f, collectableCenterPointX + 0.2f);
            other.gameObject.transform.SetPositionAndRotation(new Vector3(newLocation,other.gameObject.transform.position.y, other.gameObject.transform.position.z),other.gameObject.transform.rotation);

			numCollected += 1;
			//SetNumberCollectedText ();
		}
	}

	//void SetNumberCollectedText() 
	//{
	//	setCollectedText.text = "Collected " + numCollected.ToString () + " of " + maxCollectable.ToString(); 

	//	if (numCollected >= maxCollectable) {
	//		completeText.text = "You're done!";
 //           //other.gameObject.SetActive(false);

 //       }

	//}
}
