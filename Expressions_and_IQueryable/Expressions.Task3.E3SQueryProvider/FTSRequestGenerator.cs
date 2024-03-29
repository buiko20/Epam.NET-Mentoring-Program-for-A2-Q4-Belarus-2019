﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Expressions.Task3.E3SQueryProvider.Attributes;
using Expressions.Task3.E3SQueryProvider.Models.Request;

namespace Expressions.Task3.E3SQueryProvider
{
    public class FtsRequestGenerator
    {
        private const string FtsSearchTemplate = @"/searchFts";
        private readonly string _baseAddress;

        public FtsRequestGenerator(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public Uri GenerateRequestUrl<T>(string query = "*", int start = 0, int limit = 10)
        {
            return GenerateRequestUrl(typeof(T), query, start, limit);
        }

        public Uri GenerateRequestUrl(Type type, string query = "*", int start = 0, int limit = 10)
        {
            string metaTypeName = GetMetaTypeName(type);

            var ftsQueryRequest = new FTSQueryRequest
            {
                Statements = GetStatements(query),
                Start = start,
                Limit = limit
            };

            var ftsQueryRequestString = JsonConvert.SerializeObject(ftsQueryRequest);

            Uri uri = BindByName($"{_baseAddress}{FtsSearchTemplate}",
                new Dictionary<string, string>
                {
                    { "metaType", metaTypeName },
                    { "query", ftsQueryRequestString }
                });

            return uri;
        }

        private static Uri BindByName(string baseAddress, IDictionary<string, string> queryParams)
            => new Uri(QueryHelpers.AddQueryString(baseAddress, queryParams));

        private static string GetMetaTypeName(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(E3SMetaTypeAttribute), false);

            if (attributes.Length == 0)
            {
                throw new Exception($"Entity {type.FullName} do not have attribute E3SMetaType");
            }

            return ((E3SMetaTypeAttribute)attributes[0]).Name;
        }

        private static List<Statement> GetStatements(string query)
        {
            string[] statements = query.Split(ExpressionToFtsRequestTranslator.AndSeparator, StringSplitOptions.RemoveEmptyEntries);
            return statements.Select(s => new Statement { Query = s }).ToList();
        }
    }
}
