using UnityEngine;

namespace Proyecto.Utilities
{
    public static class ExtesionMethods
    {
        public static void LimitX(this Transform t,float min, float max)
        {
            var newX = Mathf.Clamp(t.transform.position.x, min, max);
            t.transform.position = new Vector3(newX, t.transform.position.y, t.transform.position.z);
        }
        
        public static void LimitY(this Transform t,float min, float max)
        {
            var newY = Mathf.Clamp(t.transform.position.y, min, max);
            t.transform.position = new Vector3(t.transform.position.x, newY, t.transform.position.z);
        }
        
        public static void LimitZ(this Transform t,float min, float max)
        {
            var newZ = Mathf.Clamp(t.transform.position.z, min, max);
            t.transform.position = new Vector3(t.transform.position.x, t.transform.position.y, newZ);
        }

        public static void SetPosition(this Transform t, Vector3 position)
        {
            t.transform.position = position;
        }

        public static void SetRotation(this Transform t, Quaternion rotation)
        {
            t.transform.rotation = rotation;
        }
        
        public static void AddY(this Transform t, float y)
        {
            t.transform.position = new Vector3(t.transform.position.x, t.transform.position.y + y, t.transform.position.z);
        }

        public static void SetRotationX(this Transform t, float x)
        {
            t.transform.localRotation = Quaternion.Euler(x, t.transform.localRotation.y, t.transform.localRotation.z);
        }

        public static void SetRotationZ(this Transform t, float z)
        {
            t.transform.localRotation = Quaternion.Euler(t.transform.localRotation.x, t.transform.localRotation.y, z);
        }
    }
}