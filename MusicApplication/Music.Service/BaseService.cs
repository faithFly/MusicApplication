using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Music.DbMigrator;
using Music.IService;

namespace MusicService;

public class BaseService<T,TDto> : IBaseService<T,TDto>
where T : class
{
    protected readonly MusicDbContext _dbContext;
    protected readonly IMapper _mapper;

    public BaseService(MusicDbContext _dbContext,IMapper mapper)
    {
        this._dbContext = _dbContext;
        this._mapper = mapper;
    }

    public T MapToEntity(TDto map)
    {
      return _mapper.Map<T>(map);
    }

    public async ValueTask<bool> AddAsync(T t)
    {
        await _dbContext.AddAsync(t);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async ValueTask<int> GetCountAsync(Expression<Func<T, bool>> whereLambda)
    {
       return await _dbContext.Set<T>().Where(whereLambda).CountAsync();
    }

    public async Task<T> GetFirstOrDefultAsync(Expression<Func<T, bool>> whereLambda)
    {
        return await _dbContext.Set<T>().Where(whereLambda).FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetListByPageOrderBy(Expression<Func<T, bool>> whereLambda, int pageSize, int pageIndex, Expression<Func<T, object>> orderLambda)
    {
        return await _dbContext.Set<T>().AsNoTracking().Where(whereLambda)
            .OrderBy(orderLambda)
            .Skip(pageSize*(pageIndex-1))
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<T>> GetListByPageOrderByDesc(Expression<Func<T, bool>> whereLambda, int pageSize, int pageIndex, Expression<Func<T, object>> orderLambda)
    {
        return await _dbContext.Set<T>().AsNoTracking().Where(whereLambda)
            .OrderByDescending(orderLambda)
            .Skip(pageSize*(pageIndex-1))
            .Take(pageSize)
            .ToListAsync();
    }
}