using SharpCompare.Extensions;
using SharpCompare.Factory;
using SharpCompare.Interfaces;
using Xunit;

namespace SharpCompare.Tests.Net8
{
    public class SharpCompareReflectionServiceTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: false);

        private class Pessoa
        {
            public string Nome { get; set; }
            public int Idade { get; set; }

            [IgnoreComparison]
            public string Senha { get; set; }
        }

        private class Familia
        {
            public string Sobrenome { get; set; }
            public List<Pessoa> Membros { get; set; }
        }

        [Fact]
        public void IsEqual_SameValues_ReturnsTrue()
        {
            var pessoa1 = new Pessoa { Nome = "João", Idade = 30 };
            var pessoa2 = new Pessoa { Nome = "João", Idade = 30 };

            Assert.True(_comparer.IsEqual(pessoa1, pessoa2));
        }

        [Fact]
        public void IsEqual_DifferentValues_ReturnsFalse()
        {
            var pessoa1 = new Pessoa { Nome = "João", Idade = 30 };
            var pessoa2 = new Pessoa { Nome = "Maria", Idade = 25 };

            Assert.False(_comparer.IsEqual(pessoa1, pessoa2));
        }

        [Fact]
        public void IsEqual_NullObject_ReturnsFalse()
        {
            var pessoa = new Pessoa { Nome = "João", Idade = 30 };

            Assert.False(_comparer.IsEqual(pessoa, null));
        }

        [Fact]
        public void IsEqual_DifferentTypes_ReturnsFalse()
        {
            var pessoa = new Pessoa { Nome = "João", Idade = 30 };
            var outraClasse = new { Nome = "João", Idade = 30 };

            Assert.False(_comparer.IsEqual(pessoa, outraClasse));
        }

        [Fact]
        public void IsEqual_IgnoreProperty_ReturnsTrue()
        {
            var pessoa1 = new Pessoa { Nome = "João", Idade = 30, Senha = "1234" };
            var pessoa2 = new Pessoa { Nome = "João", Idade = 30, Senha = "5678" };

            Assert.True(_comparer.IsEqual(pessoa1, pessoa2));
        }

        [Fact]
        public void IsEqual_CompareLists_ReturnsTrue()
        {
            var familia1 = new Familia
            {
                Sobrenome = "Silva",
                Membros = new List<Pessoa>
                {
                    new Pessoa { Nome = "João", Idade = 30 },
                    new Pessoa { Nome = "Maria", Idade = 25 }
                }
            };

            var familia2 = new Familia
            {
                Sobrenome = "Silva",
                Membros = new List<Pessoa>
                {
                    new Pessoa { Nome = "João", Idade = 30 },
                    new Pessoa { Nome = "Maria", Idade = 25 }
                }
            };

            Assert.True(_comparer.IsEqual(familia1, familia2));
        }

        [Fact]
        public void IsEqual_CompareLists_ReturnsFalse()
        {
            var familia1 = new Familia
            {
                Sobrenome = "Silva",
                Membros = new List<Pessoa>
                {
                    new Pessoa { Nome = "João", Idade = 30 },
                    new Pessoa { Nome = "Maria", Idade = 25 }
                }
            };

            var familia2 = new Familia
            {
                Sobrenome = "Silva",
                Membros = new List<Pessoa>
                {
                    new Pessoa { Nome = "João", Idade = 30 },
                    new Pessoa { Nome = "Carlos", Idade = 40 }
                }
            };

            Assert.False(_comparer.IsEqual(familia1, familia2));
        }
    }
}
