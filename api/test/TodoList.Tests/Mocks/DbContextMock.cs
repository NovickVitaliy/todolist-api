using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace TodoList.Tests.Mocks;

public static class DbContextMock
{
    public static (Mock<TContext> MockDbContext, Mock<DbSet<TEntity>> MockDbSet) GetMockDbContext<TEntity, TContext>(List<TEntity> entities, Expression<Func<TContext, DbSet<TEntity>>> dbSelectionExpressions) where TEntity : class where TContext : DbContext
    {
        IQueryable<TEntity> queryableData = entities.AsQueryable();
        Mock<DbSet<TEntity>> dbSetMock = new Mock<DbSet<TEntity>>();
        Mock<TContext> mockDbContext = new Mock<TContext>(new DbContextOptions<TContext>());

        dbSetMock.As<IQueryable<TEntity>>().Setup(x => x.ElementType).Returns(queryableData.ElementType);
        dbSetMock.As<IQueryable<TEntity>>().Setup(x => x.Expression).Returns(queryableData.Expression);
        dbSetMock.As<IQueryable<TEntity>>().Setup(x => x.Provider).Returns(queryableData.Provider);
        dbSetMock.As<IQueryable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(queryableData.GetEnumerator);
        dbSetMock.Setup(x => x.Add(It.IsAny<TEntity>())).Callback<TEntity>(entities.Add);
        dbSetMock.Setup(x => x.AddRange(It.IsAny<IEnumerable<TEntity>>())).Callback<IEnumerable<TEntity>>(entities.AddRange);
        dbSetMock.Setup(x => x.Remove(It.IsAny<TEntity>())).Callback<TEntity>(t => entities.Remove(t));
        dbSetMock.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<TEntity>>())).Callback<IEnumerable<TEntity>>(ts =>
        {
            foreach (var t in ts)
            {
                entities.Remove(t);
            }
        });
        
        mockDbContext.Setup(dbSelectionExpressions).ReturnsDbSet(dbSetMock.Object);

        return (mockDbContext, dbSetMock);
    }
}