using System;
using UnityEngine;

namespace GGJ.Ingame.Controller
{
    public class DrawColorSwitchController : MonoBehaviour
    {
        [SerializeField] private ColorWallSetting _colorWallSetting;

        public enum DrawType
        {
            Null,
            Green,
            Brown,
        }

        [SerializeField] private DrawType _drawType;

        public int _currentIndex;
        private int _typeCount;

        private ColorWallCreater _colorWallCreater;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _typeCount = Enum.GetNames(typeof(DrawType)).Length;
            _colorWallCreater = ColorWallCreater.i;
        }

        private void Update()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            if (Input.GetMouseButtonDown(1))
            {
                // Right click
                NextDrawType();
                AudioManger.i.Play("GetPoint", false);
            }
        }

        public void NextDrawType()
        {
            _currentIndex++;
            _currentIndex = _currentIndex >= _typeCount? 0 : _currentIndex;

            _drawType = (DrawType)_currentIndex;
            switch (_drawType)
            {
                case DrawType.Null:
                    _colorWallCreater.ObjectSelected(null);
                    break;
                case DrawType.Green:
                    _colorWallCreater.ObjectSelected(_colorWallSetting.Green);
                    break;
                case DrawType.Brown:
                    _colorWallCreater.ObjectSelected(_colorWallSetting.Brown);
                    break;
                default:
                    break;
            }
        }

        public void PreviousDrawType()
        {
            _currentIndex--;
            _currentIndex = _currentIndex < 0 ? _currentIndex - 1 : _currentIndex;
        }

        [System.Serializable]
        public struct ColorWallSetting
        {
            public ColorWallBase Green;
            public ColorWallBase Brown;
        }
    }
}