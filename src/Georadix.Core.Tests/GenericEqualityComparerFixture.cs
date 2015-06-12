namespace Georadix.Core
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public class GenericEqualityComparerFixture
    {
        private static Model sampleModel = new Model(
            DateTime.Now, Guid.NewGuid(), new Random().Next(), new Random().Next().ToString(), sampleSubModel);

        private static SubModel sampleSubModel = new SubModel(
            DateTime.Now, Guid.NewGuid(), new Random().Next(), new Random().Next().ToString());

        public static IEnumerable<object[]> EqualsWithModelsScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        sampleModel,
                        sampleModel,
                        true
                    },
                    new object[]
                    {
                        new Model(),
                        new Model(),
                        true
                    },
                    new object[]
                    {
                        new Model(
                            new DateTime(2015, 06, 12),
                            new Guid("{2c623f7b-8dfe-4ed3-a1e6-90de4ff1a87f}"),
                            15,
                            "A simple string",
                            new SubModel(new DateTime(1234567890), Guid.Empty, -100, string.Empty)),
                        new Model(
                            new DateTime(2015, 06, 12),
                            new Guid("{2c623f7b-8dfe-4ed3-a1e6-90de4ff1a87f}"),
                            15,
                            "A simple string",
                            new SubModel(new DateTime(1234567890), Guid.Empty, -100, string.Empty)),
                        true
                    },
                    new object[]
                    {
                        null,
                        null,
                        true
                    },
                    new object[]
                    {
                        null,
                        new Model(),
                        false
                    },
                    new object[]
                    {
                        new Model() { SubModelProp = new SubModel() },
                        new Model(),
                        false
                    },
                    new object[]
                    {
                        new Model(),
                        new Model() { GuidProp = Guid.NewGuid() },
                        false
                    },
                    new object[]
                    {
                        new Model() { SubModelProp = new SubModel() { DateTimeProp = DateTime.Now } },
                        new Model() { SubModelProp = new SubModel() },
                        false
                    }
                };
            }
        }

        public static IEnumerable<object[]> EqualsWithSubModelsScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        sampleSubModel,
                        sampleSubModel,
                        true
                    },
                    new object[]
                    {
                        new SubModel(),
                        new SubModel(),
                        true
                    },
                    new object[]
                    {
                        new SubModel(new DateTime(9876543210), Guid.Empty, 999, "SubModel string"),
                        new SubModel(new DateTime(9876543210), Guid.Empty, 999, "SubModel string"),
                        true
                    },
                    new object[]
                    {
                        new SubModel(),
                        null,
                        false
                    },
                    new object[]
                    {
                        new SubModel(),
                        new SubModel() { DateTimeProp = DateTime.Now },
                        false
                    },
                    new object[]
                    {
                        new SubModel() { IntProp = 10 },
                        new SubModel() { IntProp = -10 },
                        false
                    }
                };
            }
        }

        [Theory]
        [PropertyData("EqualsWithModelsScenarios")]
        public void EqualsWithModelsReturnsExpectedResult(Model x, Model y, bool expected)
        {
            var sut = new GenericEqualityComparer<Model>();

            Assert.Equal(expected, sut.Equals(x, y));
        }

        [Theory]
        [PropertyData("EqualsWithSubModelsScenarios")]
        public void EqualsWithSubModelsReturnsExpectedResult(SubModel x, SubModel y, bool expected)
        {
            var sut = new GenericEqualityComparer<SubModel>();

            Assert.Equal(expected, sut.Equals(x, y));
        }

        [Fact]
        public void GetHashCodeForNullObjectReturnsZero()
        {
            var sut = new GenericEqualityComparer<Model>();

            Assert.Equal(0, sut.GetHashCode(null));
        }

        [Fact]
        public void GetHashCodeForValidObjectReturnsObjectHashCode()
        {
            var sut = new GenericEqualityComparer<Model>();
            var model = new Model();

            Assert.Equal(model.GetHashCode(), sut.GetHashCode(model));
        }

        public class Model
        {
            public Model()
            {
            }

            public Model(DateTime? dateTimeProp, Guid guidProp, int intProp, string stringProp, SubModel subModelProp)
            {
                this.DateTimeProp = dateTimeProp;
                this.GuidProp = guidProp;
                this.IntProp = intProp;
                this.StringProp = stringProp;
                this.SubModelProp = subModelProp;
            }

            public DateTime? DateTimeProp { get; private set; }

            public Guid GuidProp { get; set; }

            public int IntProp { get; private set; }

            public string StringProp { get; set; }

            public SubModel SubModelProp { get; set; }
        }

        public class SubModel
        {
            public SubModel()
            {
            }

            public SubModel(DateTime? dateTimeProp, Guid guidProp, int intProp, string stringProp)
            {
                this.DateTimeProp = dateTimeProp;
                this.GuidProp = guidProp;
                this.IntProp = intProp;
                this.StringProp = stringProp;
            }

            public DateTime? DateTimeProp { get; set; }

            public Guid GuidProp { get; private set; }

            public int IntProp { get; set; }

            public string StringProp { get; private set; }
        }
    }
}