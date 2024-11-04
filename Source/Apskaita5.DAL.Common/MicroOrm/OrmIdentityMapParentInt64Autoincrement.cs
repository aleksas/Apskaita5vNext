﻿using System;

namespace Apskaita5.DAL.Common.MicroOrm
{
    public sealed class OrmIdentityMapParentInt64Autoincrement<T> : OrmIdentityMapBase<T> where T : class
    {


        public OrmIdentityMapParentInt64Autoincrement(string tableName, string primaryKeyFieldName,
            string primaryKeyPropName, Func<T> factoryMethod, Func<T, long?> primaryKeyGetter, 
            Action<T, long?> primaryKeySetter, string fetchQueryToken = null, string fetchAllQueryToken = null, 
            string initQueryToken = null, bool scopeIsFlag = false)
            : base(tableName, null, primaryKeyFieldName, primaryKeyPropName, true, fetchQueryToken,
                  null, fetchAllQueryToken, initQueryToken, null, scopeIsFlag, factoryMethod)
        {
            PrimaryKeyGetter = primaryKeyGetter ?? throw new ArgumentNullException(nameof(primaryKeyGetter));
            PrimaryKeySetter = primaryKeySetter ?? throw new ArgumentNullException(nameof(primaryKeySetter));
        }


        /// <summary>
        /// Gets a primary key value.
        /// </summary>
        public Func<T, long?> PrimaryKeyGetter { get; }

        /// <summary>
        /// Sets a primary key value.
        /// </summary>
        public Action<T, long?> PrimaryKeySetter { get; }


        public override bool PrimaryKeyUpdatable => false;


        internal override SqlParam GetPrimaryKeyParamForInsert(T instance)
        {
            throw new NotSupportedException();
        }

        internal override SqlParam GetPrimaryKeyParamForUpdateSet(T instance)
        {
            throw new NotSupportedException();
        }

        internal override SqlParam GetPrimaryKeyParamForUpdateWhere(T instance, string paramName)
        {
            var value = PrimaryKeyGetter(instance);
            if (!value.HasValue) throw new InvalidOperationException(string.Format(
                "Entity {0} doesn't have a primary key, i.e. its a new entity.", typeof(T).FullName));
            return new SqlParam(PrimaryKeyFieldName, value.Value);
        }

        internal override void SetPrimaryKeyAutoIncrementValue(T instance, long nid)
        {
            PrimaryKeySetter(instance, nid);
        }

        internal override void LoadPrimaryKeyValue(T instance, LightDataRow row)
        {
            PrimaryKeySetter(instance, row.GetInt64(PrimaryKeyPropName));
        }

        internal override void UpdatePrimaryKey(T instance)
        {
            throw new NotSupportedException();
        }

        internal override void DeletePrimaryKey(T instance)
        {
            PrimaryKeySetter(instance, null);
        }

    }
}