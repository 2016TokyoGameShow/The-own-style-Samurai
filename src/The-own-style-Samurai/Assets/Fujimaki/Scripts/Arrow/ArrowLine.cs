using UnityEngine;
using System.Collections;

public class ArrowLine : MonoBehaviour {


    private LineRenderer lineRendrer;

	void Start () {
        lineRendrer = GetComponent<LineRenderer>();
	}

    public IEnumerator animateLineRenderer()
    {
        float length = 0;
        while (length < 20)
        {
            length += Time.deltaTime*60;
            lineRendrer.SetPosition(1, new Vector3(0, 0, length));
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator endLine()
    {
        float scale = 1;
        while (scale > 0)
        {
            scale -= Time.deltaTime*3;
            lineRendrer.SetWidth(scale, scale);
            yield return new WaitForEndOfFrame();
        }

        lineRendrer.SetWidth(0, 0);
    }
	
	void Update () {
	
	}
}
