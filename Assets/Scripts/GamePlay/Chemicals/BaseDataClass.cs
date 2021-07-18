namespace MonstersDataManagement
{
    public abstract class BaseDataClass<T> where T : new()
    {
        public virtual void Save(T model)
        {

        }

        public virtual T Load()
        {

            var model= new T();

            return model;
        }

    }

    public class InventoryData : BaseDataClass<InventoryData>
    {

    }
}