namespace WebAppAgroAPI.Repositories
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository { get; }
        SubsidyApplicationRepository SubsidyApplicationRepository { get; }
        ParcelRepository ParcelRepository { get; }
        FarmerRepository FarmerRepository { get; }
        Task<bool> SaveAsync();


    }
}
