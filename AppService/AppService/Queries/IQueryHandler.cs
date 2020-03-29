using AppService.Interfaces;

namespace AppService.Queries
{
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Execute(TQuery query);
    }
}