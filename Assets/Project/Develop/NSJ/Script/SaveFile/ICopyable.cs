namespace NSJ_SaveUtility
{

    /// <summary>
    /// ���� ������ ���� �����մϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICopyable<T>
    {
        /// <summary>
        /// ���� �����͸� �����մϴ�.
        /// </summary>
        /// <param name="model"></param>
        void CopyFrom(T model);
    }
}