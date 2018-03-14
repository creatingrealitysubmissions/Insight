using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Get the latest webcam shot from outside "Friday's" in Times Square
public class GetImage : MonoBehaviour
{
	public string url = "http://serhan.io/images/bg/p00.jpg";

	IEnumerator Start()
	{
		// Start a download of the given URL
		using (WWW www = new WWW(url))
		{
			// Wait for download to complete
			yield return www;

			// create sprite
			Sprite sp = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
			// assign texture
			Image img = this.GetComponent<Image>();
			img.sprite = sp;
//			Renderer renderer = GetComponent<Renderer>();
//			renderer.material.mainTexture = www.texture;
		}
	}
}