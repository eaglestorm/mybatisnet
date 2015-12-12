
using System;
using System.IO;
using MyBatis.Common.Resources;
using NUnit.Framework;

using MyBatis.Common.Exceptions;

namespace MyBatis.Common.Test.Fixtures.Resources
{
    [TestFixture] 
    public class AssemblyResourceLoaderTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void SetUp()
        {
        }


        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        {
        }

        #endregion

        private readonly IResourceLoader loader = new AssemblyResourceLoader();
        private readonly string assemblyName = "MyBatis.Common.Test";
        private readonly string resPath = "Fixtures.Utilities";

        [Test]
        public void Test_good_and_bad_assembly_uri_format()
        {
            Assert.IsTrue(loader.Accept(new Uri("assembly://something/")));
            Assert.IsFalse(loader.Accept(new Uri("file://something")));
            Assert.IsFalse(loader.Accept(new Uri("http://ibatis.apache.org/")));
        }

        [Test]
        public void Valid_loading_of_assembly_resource()
        {
            using (IResource resource = loader.Create(new Uri("assembly://" + assemblyName + "/" + resPath + "/sample.txt")))
            {
                Assert.IsNotNull(resource);

                using (TextReader reader = new StreamReader(resource.Stream))
                {
                    string line = reader.ReadLine();
                    Assert.That(line, Is.EqualTo("hello"));
                }
            }
        }

        /// <summary>
        /// Use incorrect format for an assembly resource.  Using
        /// comma delimited instead of '/'.
        /// </summary>
        [Test]        
        public void Mal_formed_resource_name_should_raise_UriFormatException()
        {
            string uri = "assembly://" + assemblyName + "," + resPath + "/sample.txt";
            Assert.Throws<UriFormatException>(delegate { loader.Create(new Uri(uri)); });
        }


        /// <summary>
        /// Use the correct format but with an invalid assembly name.
        /// </summary>
        [Test]        
        public void Invalid_assembly_name_should_raise_FileNotFoundException()
        {
            string uri = "assembly://Xyz.Invalid.Assembly/" + resPath + "/sample.txt";

            Assert.Throws<FileNotFoundException>(delegate { loader.Create(new Uri(uri)); });           
        }

        /// <summary>
        /// Use correct assembly name, but incorrect namespace and resource name.
        /// </summary>
        [Test]
        public void Invalid_resource_name_should_raise_ResourceException()
        {
             string uri = "assembly://" + assemblyName + "/Xyz/InvalidResource.txt";

            Assert.Throws<ResourceException>(delegate { loader.Create(new Uri(uri)); });             
        }
    }
}
