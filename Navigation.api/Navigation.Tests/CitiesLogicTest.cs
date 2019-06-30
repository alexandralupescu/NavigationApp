/**************************************************************************
 *                                                                        *
 *  File:        CitiesLogicTest.cs                                       *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 *                                                                        *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/
using FluentAssertions;
using Moq;
using Navigation.Business.Logic.Implementations;
using Navigation.DataAccess.Collections;
using Navigation.DataAccess.Services;
using Xunit;

namespace Navigation.Tests
{
    /// <summary>
    /// CitiesLogicTest class.
    /// </summary>
    /// <remarks>
    /// Unit testing is a method of veryfing the correct functionality of individual source
    /// code units. An unit is the smallest part of an application tested. The purpose of 
    /// testing the units is to isolate each part of a program and to show that the
    /// individual parts are correct. Testing an unit helps to discover problems in the development 
    /// cycle in time.
    /// </remarks>
    public class CitiesLogicTest
    {
        private Mock<IDbService<Cities>> repositoryMock;

        /// <summary>
        /// CitiesLogicTest constructor.
        /// </summary>
        private void SetUpTests()
        {
            /* Constructor is called for every test. */
            repositoryMock = new Mock<IDbService<Cities>>();
        }
       

        /// <summary>
        /// Do some clean after the test.
        /// Dealocate memory.
        /// </summary>
        private void TearDownTests()
        {
            repositoryMock = null;
        }


        /// <summary>
        /// Test method that will verify if a city Id exists in the database.
        /// </summary>
        /// <remarks>
        /// The [Fact] attribute is used by the xUnit.net test runner to identify a 'normal' unit test:
        /// a test method that takes no method arguments.
        ///</remarks>
        [Fact]
        public void When_GetCityByIdIsCalled_Then_ShouldReturnOkResult()
        {
            /* Steps: */

            /* 1. Arrange the objects, creating and setting them up as necessary. */
            string lookupId = "5c9f102a441a3e949c3085a5";


            SetUpTests();
            repositoryMock
                .Setup(r => r.GetByIdAsync(lookupId))
                .ReturnsAsync(new Cities{ Id = lookupId });

            var sut = new CitiesLogic(repositoryMock.Object);

            /* 2. Act on a object. */
            var result = sut.GetByCityIdAsync(lookupId);

            /* 3. Assert that something is expected. */
            result.Should().NotBe(false);

            TearDownTests();
        }

        /// <summary>
        /// Test method that will verify DeleteAsync method that will delete a documents
        /// based on Id.
        /// </summary>
        [Fact]
        public void When_DeleteCityByIdIsCalled_Then_ShouldReturnOkResult()
        {
            /* Steps: */

            /* 1. Arrange the objects, creating and setting them up as necessary. */
            string lookupId = "5c9f102a441a3e949c3085a5";
            bool flagResponse = true;


            SetUpTests();
            repositoryMock
                .Setup(r => r.DeleteAsync(lookupId))
                .ReturnsAsync(flagResponse);

            var sut = new CitiesLogic(repositoryMock.Object);

            /* 2. Act on a object. */
            var result = sut.DeleteCityAsync(lookupId);

            /* 3. Assert that something is expected. */
            result.Should().NotBe(false);

            TearDownTests();
        }

    }
}
