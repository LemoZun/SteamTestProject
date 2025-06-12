namespace NSJ_SaveUtility
{

    /// <summary>
    /// 복사 가능한 모델을 정의합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICopyable<T>
    {
        /// <summary>
        /// 모델의 데이터를 복사합니다.
        /// </summary>
        /// <param name="model"></param>
        void CopyFrom(T model);
    }
}