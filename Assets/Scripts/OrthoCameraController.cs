using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StellarisTool
{
    public class OrthoCameraController : MonoBehaviour
    {
        public Camera cam;
        public static float panSpeed = 1f;
        public static float zoomSpeed = -5f;

        public static bool Draging = false;

        private void Awake()
        {
            //cam = GetComponent<Camera>();
        }

        private float GetMovePan()
        {
            var x = cam.orthographicSize;
            //var y = 2.21212413f * Mathf.Pow(x, -6) - 4.91890551f * Mathf.Pow(x, -5) + 3.51659865f * Mathf.Pow(x, -2) - 8.54474553f * Mathf.Pow(x, -1) + 3.04621515f * x - 3.57646725f * Mathf.Pow(x, -1) + 38;
            var y = Mathf.Lerp(30, 850, (x - 1) / 35) * panSpeed;
            //Debug.Log($"Pan:{x},{y}");
            return y;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Draging = true;
                // 平移相机位置
                //Debug.Log("InDrag");

                //var curPos = new Vector3(-Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y"));
                //Debug.Log($"diff Pos : {curPos} {curPos - dragStartPos}");
                //transform.position += panSpeed * (curPos - dragStartPos) * Time.deltaTime;
                //dragStartPos = new Vector3(-Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y"));

                Vector3 pan = new Vector3(-Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y"));
                pan *= GetMovePan() * Time.deltaTime;
                pan = Quaternion.Euler(0, transform.eulerAngles.y, 0) * pan;
                transform.Translate(pan, Space.World);
            } else
            {
                Draging = false;
            }
            // 1-30
            // 10-250
            // 30-750
            // 50-1400
            // 75 2000
            // 100 2600
            // 缩放相机大小
            float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            cam.orthographicSize += zoom * cam.orthographicSize;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1f, 37f);
        }

        //private void LateUpdate()
        //{
        //    // 保持相机朝向为 (0, -1, 0)
        //    transform.rotation = Quaternion.Euler(90, 0, 0);
        //}
    }
}
