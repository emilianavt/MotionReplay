using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
 
public class InvertedToggleEvent : MonoBehaviour{
 
    //wont show in inspector
    //public    UnityEngine.Events.UnityEvent<bool> toggleEvent;
 
    //this will show in inspector
    [System.Serializable]
    public class UnityEventBool : UnityEngine.Events.UnityEvent<bool>{}
    public    UnityEventBool onValueChangedInverse;
 
 
    private void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener( (on)=>{ onValueChangedInverse.Invoke(!on); } );
        GetComponent<Toggle>().onValueChanged.Invoke(GetComponent<Toggle>().isOn);
    }
}