using System.Threading;

namespace Paysera.RestClientCommon.Http.Task
{
    public interface IAvoidableTask
    {
        CancellationTokenSource CancellationTokenSource { get; }
    }
}
