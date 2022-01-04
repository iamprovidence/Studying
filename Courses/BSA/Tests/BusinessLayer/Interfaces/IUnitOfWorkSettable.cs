namespace BusinessLayer.Interfaces
{
    public interface IUnitOfWorkSettable
    {
        void SetUnitOfWork(DataAccessLayer.Interfaces.IUnitOfWork unitOfWork);
    }
}
