using System;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SquareDinoTestTask
{
    public class SettingCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _rotation;
        
        private const string CameraForEditorTag = "CameraForEditor";
        
        //EditorApplication.CallbackFunction 
        private void OnValidate()
        {
            if (_camera == null)
            {
                var cameras = FindObjectsOfType<CinemachineVirtualCamera>();
                foreach (var virtualCamera in cameras)
                {
                    if(!virtualCamera.CompareTag(CameraForEditorTag)) continue;
                    _camera = virtualCamera;
                }
            }
            
            //_camera.transform.position = transform.position;
            //_camera.transform.rotation = transform.rotation;
            //transform.position = _position;
            //transform.rotation = Quaternion.Euler(_rotation);
            //Debug.Log(Random.value);
            _camera.ForceCameraPosition(transform.position, transform.rotation);
        }
        
        //private override Trans
    }
}