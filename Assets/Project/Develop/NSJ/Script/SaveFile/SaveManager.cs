using NSJ_MVVM;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NSJ_SaveUtility
{
    public class SaveManager : SingleTon<SaveManager>
    {
        public GameData Data;

        public List<string> Models => Data.Models;

        public event UnityAction OnSaveBeforeEvent;
        public event UnityAction OnSaveEvent;
        public event UnityAction<List<string>> OnLoadEvent;

        /// <summary>
        /// 모델이 세이브 이벤트를 구독합니다
        /// </summary>
        public static void RegisterModel<TModel>(TModel model) where TModel : BaseModel
        {
            SetSingleton();
            model.SubscribeSaveEvent<TModel>();
        }

        /// <summary>
        /// 모델이 세이브 이벤트를 해제합니다
        /// </summary>
        public static void UnRegisterModel<TModel>(TModel model) where TModel : BaseModel
        {
            model.UnsubscribeSaveEvent<TModel>();
        }


        /// <summary>
        /// 데이터를 저장합니다
        /// </summary>
        /// <param name="saveNumber"></param>
        /// <returns></returns>
        public bool SaveData(int saveNumber = int.MinValue)
        {
            Data.Models.Clear();
            OnSaveBeforeEvent?.Invoke();

            if (saveNumber == int.MinValue)
            {
                saveNumber = Data.SaveNumber;
            }

            DateTime now = DateTime.Now;
            Data.LastSaveTime = now.ToString("yyyy-MM-dd HH:mm:ss");

            bool success = SaveUtility.Save(ref Data, saveNumber);


            if (success)
            {
                OnSaveEvent?.Invoke();
            }

            return success;
        }

        /// <summary>
        /// 데이터를 로드합니다
        /// </summary>
        /// <param name="saveNumber"></param>
        /// <returns></returns>
        public bool LoadData(int saveNumber)
        {
            Data = SaveUtility.Load(saveNumber, out bool success);

            if (success == false)
            {
                if (Data == null)
                {
                    Data = new GameData();
                    Data.SaveNumber = saveNumber;
                }
            }
            OnLoadEvent?.Invoke(Data.Models);

            return success;
        }

        /// <summary>
        /// 모델 데이터를 추가합니다.
        /// </summary>
        public void AddSaveModelData(string saveJson)
        {
            Data.Models.Add(saveJson);
        }

        /// <summary>
        /// 모델 데이터를 제거합니다.
        /// </summary>
        public void RemoveModelData(string saveJson)
        {
            Data.Models.Remove(saveJson);
        }
    }
}