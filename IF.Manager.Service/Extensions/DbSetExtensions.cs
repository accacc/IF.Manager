using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Service.Extensions
{
    public static class DbSetExtensions
    {
        public static async Task<List<TEntity>> FindRecursiveAsync<TEntity, TKey>(
            this DbSet<TEntity> source,
            Expression<Func<TEntity, bool>> rootSelector,
            Func<TEntity, TKey> getEntityKey,
            Func<TEntity, TKey> getChildKeyToParent)
            where TEntity : class
        {
            var alreadyProcessed = new HashSet<TKey>();

            List<TEntity> result = await source.Where(rootSelector).ToListAsync();

            List<TEntity> currentRoots = result;
            while (currentRoots.Count > 0)
            {
                TKey[] currentParentKeys = currentRoots.Select(getEntityKey).Except(alreadyProcessed).ToArray();
                alreadyProcessed.UnionWith(currentParentKeys);

                Expression<Func<TEntity, bool>> childPredicate = x => currentParentKeys.Contains(getChildKeyToParent(x));
                currentRoots = await source.Where(childPredicate).ToListAsync();
            }

            return result;
        }
    }
}