using NSJ_MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public class SaveHandler : MonoBehaviour, ISaveProvidable
    {
        ISavable _savable;

        private void Awake()
        {
            _savable = GetComponent<ISavable>();

            SubscribeSaveEvent();
        }

        private void OnDestroy()
        {
            UnsubscribeSaveEvent();
        }

        public void SaveModel()
        {
            if (_savable == null)
                return;

            // Json 받기
            string modelJson = _savable.Save();

            // 빈 Json (저장 안함) 이면 넘기기
            if (modelJson == string.Empty)
                return;

            // Json 저장
            SaveManager.Instance.AddSaveModelData(modelJson);
        }

        public void LoadModel()
        {
            if (_savable == null)
                return;

            // Json불러오기
            GameData data = SaveManager.Instance.Data;

            List<string> saveEntrys = data.Models;

            // Json 넘기기
            string modelJson = _savable.Load(saveEntrys);
            SaveManager.Instance.RemoveModelData(modelJson);
        }

        private void SubscribeSaveEvent() 
        {
            SaveManager.Instance.OnSaveBeforeEvent += SaveModel;
        }
        private void UnsubscribeSaveEvent()
        {
            if(SaveManager.Instance == null) return;
            SaveManager.Instance.OnSaveBeforeEvent -= SaveModel;
        }


    }
}