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
        /// 세이브ID 매칭 실패로인한 데이터 로드 실패시에 강제 타입 캐스팅 로드용 메소드
        /// </summary>
        public bool TryReload<TModel>(TModel model, out TModel returnModel) where TModel : BaseModel, ICopyable<TModel>
        {
            returnModel = null;

            foreach (string json in Models)
            {
                SaveEntry entry = FromJson<SaveEntry>(json);
                TModel loadModel = FromJson<TModel>(entry.Json);

                if (loadModel != null) 
                {
                    // 일치한다고 판단
                    returnModel = loadModel;
                    RemoveModelData(json);
                    return true;
                }
            }

            return false;
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

        /// <summary>
        /// 클래스를 Json화 합니다
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        private string ToJson<T>(T instance) where T : class
        {
            string json = JsonUtility.ToJson(instance);
            return json;
        }

        /// <summary>
        /// json을 클래스화 합니다
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        private T FromJson<T>(string json)
        {
            T model = JsonUtility.FromJson<T>(json);
            return model;
        }
    }
}