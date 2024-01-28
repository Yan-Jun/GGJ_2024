using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGJ.Outgame.UI
{
    public class TutorialHandler : MonoBehaviour
    {
        [SerializeField] private TutorialSetting _tutorialSetting; 

        private int _currentIndex;
        private int _pageTotal;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _currentIndex = 0;
            _pageTotal = _tutorialSetting.TutorialSprites.Length;

            _tutorialSetting.RightButton.onClick.AddListener(NextPage);
            _tutorialSetting.LeftButton.onClick.AddListener(PreviousPage);
            _tutorialSetting.BackButton.onClick.AddListener(Back);
            ShowPage(_currentIndex);
        }

        public void NextPage()
        {
            _currentIndex++;
            _currentIndex = _currentIndex >= _pageTotal ? _pageTotal - 1 : _currentIndex;
            ShowPage(_currentIndex);
        }

        public void PreviousPage()
        {
            _currentIndex--;
            _currentIndex = _currentIndex < 0 ? 0 : _currentIndex;
            ShowPage(_currentIndex);
        }

        public void ShowPage(int index)
        {
            _tutorialSetting.TutorialImage.sprite = _tutorialSetting.TutorialSprites[index];
            if (index == _pageTotal - 1)
            {
                _tutorialSetting.LeftButton.gameObject.SetActive(false);
                _tutorialSetting.RightButton.gameObject.SetActive(false);
                _tutorialSetting.BackButton.gameObject.SetActive(true);
            }
            else if (index == 0)
            {
                _tutorialSetting.LeftButton.gameObject.SetActive(false);
                _tutorialSetting.RightButton.gameObject.SetActive(true);
                _tutorialSetting.BackButton.gameObject.SetActive(false);
            }
            else
            {
                _tutorialSetting.LeftButton.gameObject.SetActive(true);
                _tutorialSetting.RightButton.gameObject.SetActive(true);
                _tutorialSetting.BackButton.gameObject.SetActive(false);
            }
        }

        public void Back()
        {
            SceneManager.LoadScene(_tutorialSetting.BackSceneName);
        }
        
        [System.Serializable]
        public struct TutorialSetting
        {
            public Image TutorialImage;
            public Sprite[] TutorialSprites;
            public Button RightButton;
            public Button LeftButton;
            public Button BackButton;
            public string BackSceneName;
        }
    }
}