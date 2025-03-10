using UnityEngine;
using UnityEngine.Events;


namespace Trigger
{ 
	public class TriggerCollider : MonoBehaviour
	{
		public string availableTriggerTag;
		
		 public UnityEvent onTriggerEnterEvent;
		 
		 public UnityEvent onTriggerExitEvent;
		
		private void OnTriggerEnter(Collider other)
		{
			if(other.CompareTag(availableTriggerTag)) onTriggerEnterEvent.Invoke();
		}

		private void OnTriggerExit(Collider other)
		{
			if(other.CompareTag(availableTriggerTag)) onTriggerExitEvent.Invoke();
		}
	}
}