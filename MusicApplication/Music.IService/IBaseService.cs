using System.Linq.Expressions;

namespace Music.IService;

public interface IBaseService<T,TDto> where T : class
{
     T MapToEntity(TDto map);
     ValueTask<bool> AddAsync(T t);
     ValueTask<bool> UpdateAsync(T t);
     ValueTask<int> GetCountAsync(Expression<Func<T, bool>> whereLambda);
     Task<T> GetFirstOrDefultAsync(Expression<Func<T, bool>> whereLambda);

     Task<List<T>> GetListByPageOrderBy(Expression<Func<T, bool>> whereLambda, int pageSize, int pageIndex,
          Expression<Func<T, object>> orderLambda);
     Task<List<T>> GetListByPageOrderByDesc(Expression<Func<T, bool>> whereLambda, int pageSize, int pageIndex,
         Expression<Func<T, object>> orderLambda);
}