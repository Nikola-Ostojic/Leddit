using DynamicData;
using DynamicData.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mobile.Core.Extensions
{
    public static class DynamicDataExtensions
    {
        public static void EditDiff<TModel, TKey>(this SourceCache<TModel, TKey> sourceCache, IEnumerable<TModel> items, int? offset = null, int pageSize = 25) where TModel : ModelWithIdBase<TKey>
        {
            var keyComparer = new KeyComparer<TModel, TKey>();
            Func<TModel, TModel, bool> areEqual = EqualityComparer<TModel>.Default.Equals;

            sourceCache.Edit(innerCache =>
            {
                var originalItems = offset == null
                    ? innerCache.KeyValues.AsArray()
                    : innerCache.KeyValues.Skip((int)offset).Take(pageSize).AsArray();
                var newItems = innerCache.GetKeyValues(items).AsArray();

                var removes = originalItems.Except(newItems, keyComparer).ToArray();
                var adds = newItems.Except(originalItems, keyComparer).ToArray();
                var intersect = newItems
                    .Select(kvp => new
                    {
                        Original = originalItems
                            .Where(x => keyComparer.Equals(kvp, x))
                            .Select(found => new { found.Key, found.Value })
                            .FirstOrDefault(),
                        NewItem = kvp
                    })
                    .Where(x => x.Original != null && !areEqual(x.Original.Value, x.NewItem.Value))
                    .Select(x => new KeyValuePair<TKey, TModel>(x.NewItem.Key, x.NewItem.Value))
                    .ToArray();

                //Now we are invalidating the cache if there are items to be removed and the sum of intersections is greater
                //than or equal to the page size on the first page.
                if (offset == 0 && removes.Any() && removes.Count() + intersect.Count() >= pageSize)
                {
                    innerCache.Clear();
                }

                innerCache.Remove(removes.Select(kvp => kvp.Key));
                innerCache.AddOrUpdate(adds.Union(intersect));
            });
        }
    }

    internal class KeyComparer<TObject, TKey> : IEqualityComparer<KeyValuePair<TKey, TObject>>
    {
        public bool Equals(KeyValuePair<TKey, TObject> x, KeyValuePair<TKey, TObject> y) => x.Key.Equals(y.Key);

        public int GetHashCode(KeyValuePair<TKey, TObject> obj) => obj.Key.GetHashCode();
    }

    public abstract class ModelWithIdBase<TId> : IEntity
    {
        public virtual TId Id { get; set; }

        object IEntity.Key
        {
            get => Id;
            set => Id = (TId)value;
        }

        public static bool operator ==(ModelWithIdBase<TId> e1, ModelWithIdBase<TId> e2)
        {
            if (ReferenceEquals(e1, null))
                return ReferenceEquals(e2, null);

            return e1.Equals(e2);
        }

        public static bool operator !=(ModelWithIdBase<TId> e1, ModelWithIdBase<TId> e2) => !(e1 == e2);

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object obj)
        {
            var @base = obj as ModelWithIdBase<TId>;
            return @base != null &&
                   EqualityComparer<TId>.Default.Equals(Id, @base.Id);
        }
    }

    public interface IEntity
    {
        object Key { get; set; }
    }
}
