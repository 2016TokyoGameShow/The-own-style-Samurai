using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyDownEvent : MonoBehaviour
{
    [SerializeField]
    KeyCode key;

    [SerializeField]
    List<EventDelegate>	pushed;
    
    void Awake()
    {
        
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            Push();
        }
    }

    void Push()
    {
        if (pushed == null)
		{
            return;
        }
		List<EventDelegate> mTemp = pushed;
		pushed = new List<EventDelegate>();

		// Notify the listener delegates
		EventDelegate.Execute(mTemp);

		// Re-add the previous persistent delegates
		for (int i = 0; i < mTemp.Count; ++i)
		{
			EventDelegate ed = mTemp[i];
			if (ed != null && !ed.oneShot) EventDelegate.Add(pushed, ed, ed.oneShot);
		}
		mTemp = null;
    }
}