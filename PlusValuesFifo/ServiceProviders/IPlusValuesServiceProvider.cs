using PlusValuesFifo.Models;
using PlusValuesFifo.Services;

namespace PlusValuesFifo.ServiceProviders
{
    public interface IPlusValuesServiceProvider
    {
        IPlusValuesService GetPlusValuesService(AssetType assetType);
    }
}