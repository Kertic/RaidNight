using System;
using UnityEngine;

namespace Code.Systems
{
    public static class Utils
    {
        public static class Vector2
        {
            /*
             * Returns the angle of the vector a center point and an another point
             */
            public static float GetRotationOfObjectOnCircle(UnityEngine.Vector2 positionOfObject, UnityEngine.Vector2 positionOfCircleCenter)
            {
                float angle = positionOfObject.y >= positionOfCircleCenter.y
                    ? UnityEngine.Vector2.Angle(UnityEngine.Vector2.right, (positionOfObject - positionOfCircleCenter).normalized)
                    : 180.0f - UnityEngine.Vector2.Angle(UnityEngine.Vector2.right, (positionOfObject - positionOfCircleCenter).normalized) + 180.0f;
                return angle;
            }
        }

        public static class Vector3
        {
            public static UnityEngine.Vector3 GetPositionInRotatingCircle(int indexOfObject, int totalObjectsInCircle, float rotationOffset, float radius, UnityEngine.Vector3 center)
            {
                /* Distance around the circle */
                float radians = 2 * MathF.PI / totalObjectsInCircle * indexOfObject;

                /* Get the vector direction */
                float vertical = MathF.Sin(radians + rotationOffset);
                float horizontal = MathF.Cos(radians + rotationOffset);

                UnityEngine.Vector3 spawnDir = new(horizontal, vertical, 0);

                /* Get the spawn position */
                UnityEngine.Vector3 spawnPos = center + spawnDir * radius; // Radius is just the distance away from the point
                return spawnPos;
            }
        }
    }
}