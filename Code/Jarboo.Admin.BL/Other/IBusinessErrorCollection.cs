namespace Jarboo.Admin.BL.Other
{
    public interface IBusinessErrorCollection
    {
        void Add(string property, string error);
        bool HasErrors();
    }
}
