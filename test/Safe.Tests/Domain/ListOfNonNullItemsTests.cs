using Safe.Core.Domain;
using System;
using Xunit;

namespace Safe.Tests.Domain
{
    public class ListOfNonNullItemsTests
    {
        private readonly ListOfNonNullItems<string> _list = new ListOfNonNullItems<string> 
        {
            "aaa",
            "bbb"
        };

        [Fact]
        public void Indexer_get()
        {
            Assert.Equal("aaa", _list[0]);
        }

        [Fact]
        public void Indexer_set_not_null()
        {
            _list[0] = "ccc";

            Assert.Equal("ccc", _list[0]);
        }

        [Fact]
        public void Indexer_set_null()
        {
            Assert.Throws<ArgumentNullException>(() => _list[0] = null);
        }

        [Fact]
        public void Count()
        {
            Assert.Equal(2, _list.Count);
        }

        [Fact]
        public void IsReadOnly()
        {
            Assert.False(_list.IsReadOnly);
        }

        [Fact]
        public void Add_not_null()
        {
            _list.Add("ccc");

            Assert.Equal(3, _list.Count);
            Assert.Equal("ccc", _list[2]);
        }

        [Fact]
        public void Add_null()
        {
            Assert.Throws<ArgumentNullException>(() => _list.Add(null));
        }

        [Fact]
        public void Clear()
        {
            _list.Clear();

            Assert.Empty(_list);
        }

        [Fact]
        public void Contains()
        {
            Assert.Contains("aaa", _list);
            Assert.DoesNotContain("ccc", _list);
        }

        [Fact]
        public void CopyTo()
        {
            var buffer = new string[2];

            _list.CopyTo(buffer, 0);

            Assert.Equal("aaa", buffer[0]); 
            Assert.Equal("bbb", buffer[1]);
        }

        [Fact]
        public void GetEnumerator()
        {
            var enumerator = _list.GetEnumerator();

            Assert.NotNull(enumerator);
        }

        [Fact]
        public void IndexOf()
        {
            Assert.Equal(1, _list.IndexOf("bbb"));
            Assert.Equal(-1, _list.IndexOf("ccc"));
        }

        [Fact]
        public void Insert_not_null()
        {
            _list.Insert(1, "ccc");

            Assert.Equal("ccc", _list[1]);
        }

        [Fact]
        public void Insert_null()
        {
            Assert.Throws<ArgumentNullException>(() => _list.Insert(1, null));
        }

        [Fact]
        public void Remove()
        {
            Assert.True(_list.Remove("aaa"));
            Assert.Single(_list);
        }

        [Fact]
        public void RemoveAt()
        {
            _list.RemoveAt(0);
            Assert.Single(_list);
        }
    }
}
