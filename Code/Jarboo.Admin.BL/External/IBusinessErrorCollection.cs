namespace Jarboo.Admin.BL.External
{
    public interface IBusinessErrorCollection
    {
        void Add(string property, string error);
        bool HasErrors();
    }
}
