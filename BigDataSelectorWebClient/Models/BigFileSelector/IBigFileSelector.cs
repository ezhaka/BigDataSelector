using BigDataSelectorWebClient.Models.BigFileSelector.Result;

namespace BigDataSelectorWebClient.Models.BigFileSelector
{
    public interface IBigFileSelector
    {
        BigFileSelectorResult SelectTopElements();
    }
}