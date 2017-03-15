using System.Threading;
using System.Threading.Tasks;

namespace Paysera.RestClientCommon.Http.Task
{
    public class ApiTask<T> : IAvoidableTask
    {
        public CancellationTokenSource CancellationTokenSource { get; private set; }
        public Task<T> Task { get; private set; }

        public static ApiTask<TEntity> Create<TEntity>(Task<TEntity> finalTask, CancellationTokenSource cancellationTokenSource)
        {
            return new ApiTask<TEntity>()
            {
                Task = finalTask,
                CancellationTokenSource = cancellationTokenSource
            };
        }
    }
}
