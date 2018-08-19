using UnityEngine;
using System.Collections;

    public static class Constants
    {
        public static float GameSpeed = 5f;
        public static Camera camera;
        public static AudioSource AudioCenter;
        public static float leftBound, RightBound, TopBound, BottomBound;
        static Constants()
        {
            camera = Camera.main;
            leftBound = camera.transform.position.x - (camera.orthographicSize * camera.aspect);
            RightBound = camera.transform.position.x + (camera.orthographicSize * camera.aspect);
            TopBound = camera.transform.position.y + camera.orthographicSize;
            AudioCenter = GameObject.Find("Destroyer").GetComponent<AudioSource>();
            BottomBound = camera.transform.position.y - camera.orthographicSize;
        }
    }
