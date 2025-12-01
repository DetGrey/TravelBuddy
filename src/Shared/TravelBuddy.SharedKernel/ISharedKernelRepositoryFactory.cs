namespace TravelBuddy.SharedKernel
{
    public interface ISharedKernelRepositoryFactory
    {
        ISharedKernelRepository GetSharedKernelRepository();
    }
}