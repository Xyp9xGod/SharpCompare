﻿using SharpCompare.Factory;
using SharpCompare.Interfaces;

namespace SharpCompare.Tests.Reflection
{
    public class SharpCompareNullableTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: false);

        [Fact]
        public void CompareNullableTypes_ShouldHandleNullValuesCorrectly()
        {
            int? value1 = null;
            int? value2 = 5;
            Assert.False(_comparer.IsEqual(value1, value2));
            Assert.True(_comparer.IsEqual(null, null));
        }
    }
}
