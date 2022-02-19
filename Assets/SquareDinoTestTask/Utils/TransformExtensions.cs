using System;
using Cinemachine;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SquareDinoTestTask.Utils
{
    [Serializable]
    public static class TransformExtensions
    {
        private static CinemachineVirtualCamera _camera;
        private static string CameraForEditorTag = "CameraForEditor";

        [ContextMenu("Set Camera Here")]
        public static void SetCameraHere(this Transform tr)
        {
            if (_camera == null)
            {
                var cameras = Object.FindObjectsOfType<CinemachineVirtualCamera>();
                foreach (var virtualCamera in cameras)
                {
                    if(!virtualCamera.CompareTag(CameraForEditorTag)) continue;
                    _camera = virtualCamera;
                }
            }
            _camera.ForceCameraPosition(tr.position, tr.rotation);
        }
    }
}