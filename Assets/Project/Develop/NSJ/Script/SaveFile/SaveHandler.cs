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

            if (modelJson == string.Empty)
                return;

            SaveManager.Instance.RemoveModelData(modelJson);
        }

        /// <summary>
        /// 세이브 하기 전의 이벤트에 대해 구독합니다
        /// </summary>
        private void SubscribeSaveEvent() 
        {
            SaveManager.Instance.OnSaveBeforeEvent += SaveModel;
        }
        /// <summary>
        /// 객체가 파괴될 때 세이브 하기 전의 이벤트에 대해 구독을 끊습니다
        /// </summary>
        private void UnsubscribeSaveEvent()
        {
            if(SaveManager.Instance == null) return;
            SaveManager.Instance.OnSaveBeforeEvent -= SaveModel;
        }


    }
}