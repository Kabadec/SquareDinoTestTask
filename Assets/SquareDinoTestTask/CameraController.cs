using System;
using System.Diagnostics;
using Cinemachine;
using SquareDinoTestTask.Utils;
using UnityEngine;

namespace SquareDinoTestTask
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private CinemachineVirtualCamera _cameraFirst;
        [SerializeField] private CinemachineVirtualCamera _cameraSecond;
        [SerializeField] private Animator _animator;
        
        private static readonly int NextCameraKey = Animator.StringToHash("next-camera");

        private CinemachineVirtualCamera _currentCamera;

        private void Start()
        {
            _currentCamera = _cameraFirst;
        }

        public void CameraMoveTo(Transform cameraTransform)
        {
            if (_currentCamera == _cameraFirst)
            {
                SetCamera(_cameraSecond, cameraTransform);
            }
            else if (_currentCamera == _cameraSecond)
            {
                SetCamera(_cameraFirst, cameraTransform);
            }
            
            _animator.SetTrigger(NextCameraKey);
        }

        private void SetCamera(CinemachineVirtualCamera assignCamera, Transform newTransform)
        {
            _currentCamera = assignCamera;
            assignCamera.transform.position = newTransform.position;
            assignCamera.transform.rotation = newTransform.rotation;
        }

    }
}