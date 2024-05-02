public interface IView<T> where T : ModelBase
{
    void UpdateView(T data);
}
